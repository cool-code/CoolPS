using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cool;

public sealed class ArrayPool<T>
{
    public static readonly ArrayPool<T> Shared = new();

    /// <summary>The default maximum length of each array in the pool (2^20).</summary>
    private const int DefaultMaxArrayLength = 1024 * 1024;
    /// <summary>The default maximum number of arrays per bucket that are available for rent.</summary>
    private const int DefaultMaxNumberOfArraysPerBucket = 50;
    /// <summary>Pre-allocated empty array used when arrays of length 0 are requested.</summary>
    private static readonly T[] s_emptyArray = [];

    private readonly Bucket[] _buckets;

    public ArrayPool() : this(DefaultMaxArrayLength, DefaultMaxNumberOfArraysPerBucket) { }

    public ArrayPool(int maxArrayLength, int maxArraysPerBucket)
    {
        if (maxArrayLength <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxArrayLength));
        }
        if (maxArraysPerBucket <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxArraysPerBucket));
        }

        // Our bucketing algorithm has a min length of 2^4 and a max length of 2^30.
        // Constrain the actual max used to those values.
        const int MinimumArrayLength = 0x10, MaximumArrayLength = 0x40000000;
        if (maxArrayLength > MaximumArrayLength)
        {
            maxArrayLength = MaximumArrayLength;
        }
        else if (maxArrayLength < MinimumArrayLength)
        {
            maxArrayLength = MinimumArrayLength;
        }

        // Create the buckets.
        int maxBuckets = SelectBucketIndex(maxArrayLength);
        var buckets = new Bucket[maxBuckets + 1];
        for (int i = 0; i < buckets.Length; i++)
        {
            buckets[i] = new Bucket(GetMaxSizeForBucket(i), maxArraysPerBucket);
        }
        _buckets = buckets;
    }
    public T[] Rent(int minimumLength)
    {
        // Arrays can't be smaller than zero.  We allow requesting zero-length arrays (even though
        // pooling such an array isn't valuable) as it's a valid length array, and we want the pool
        // to be usable in general instead of using `new`, even for computed lengths.
        if (minimumLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumLength));
        }
        else if (minimumLength == 0)
        {
            // No need for events with the empty array. Our pool is effectively infinite
            // and we'll never allocate for rents and never store for returns.
            return s_emptyArray;
        }

        int index = SelectBucketIndex(minimumLength);

        T[]? buffer;
        if (index < _buckets.Length)
        {
            // Search for an array starting at the 'index' bucket. If the bucket is empty, bump up to the
            // next higher bucket and try that one, but only try at most a few buckets.
            const int MaxBucketsToTry = 2;
            int i = index;
            do
            {
                // Attempt to rent from the bucket.  If we get a buffer from it, return it.
                buffer = _buckets[i].Rent();
                if (buffer != null)
                {
                    return buffer;
                }
            }
            while (++i < _buckets.Length && i != index + MaxBucketsToTry);

            // The pool was exhausted for this buffer size.  Allocate a new buffer with a size corresponding
            // to the appropriate bucket.
            buffer = new T[_buckets[index]._bufferLength];
        }
        else
        {
            // The request was for a size too large for the pool.  Allocate an array of exactly the requested length.
            // When it's returned to the pool, we'll simply throw it away.
            buffer = new T[minimumLength];
        }

        return buffer;
    }

    public void Return(T[] array, bool clearArray = false)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        else if (array.Length == 0)
        {
            // Ignore empty arrays.  When a zero-length array is rented, we return a singleton
            // rather than actually taking a buffer out of the lowest bucket.
            return;
        }

        // Determine with what bucket this array length is associated
        int bucket = SelectBucketIndex(array.Length);

        // If we can tell that the buffer was allocated, drop it. Otherwise, check if we have space in the pool
        if (bucket < _buckets.Length)
        {
            // Clear the array if the user requests
            if (clearArray)
            {
                Array.Clear(array, 0, array.Length);
            }

            // Return the buffer to its bucket.  If the bucket is full, the buffer will be dropped on the floor.
            _buckets[bucket].Return(array);
        }
    }
    /// <summary>Provides a thread-safe bucket containing buffers that can be Rent'd and Return'd.</summary>
    private sealed class Bucket
    {
        internal readonly int _bufferLength;
        private readonly T[]?[] _buffers;

        private SpinLock _lock; // do not make this readonly; it's a mutable struct
        private int _index;

        /// <summary>
        /// Creates the pool with numberOfBuffers arrays where each buffer is of bufferLength length.
        /// </summary>
        internal Bucket(int bufferLength, int numberOfBuffers)
        {
            _lock = new SpinLock(false); // only enable thread tracking if debugger is attached; it adds non-trivial overheads to Enter/Exit
            _buffers = new T[numberOfBuffers][];
            _bufferLength = bufferLength;
        }

        /// <summary>Takes an array from the bucket.  If the bucket is empty, returns null.</summary>
        internal T[]? Rent()
        {
            T[]?[] buffers = _buffers;
            T[]? buffer = null;

            // While holding the lock, grab whatever is at the next available index and
            // update the index.  We do as little work as possible while holding the spin
            // lock to minimize contention with other threads.  The try/finally is
            // necessary to properly handle thread aborts on platforms which have them.
            bool lockTaken = false, allocateBuffer = false;
            try
            {
                _lock.Enter(ref lockTaken);

                if (_index < buffers.Length)
                {
                    buffer = buffers[_index];
                    buffers[_index++] = null;
                    allocateBuffer = buffer == null;
                }
            }
            finally
            {
                if (lockTaken) _lock.Exit(false);
            }

            // While we were holding the lock, we grabbed whatever was at the next available index, if
            // there was one.  If we tried and if we got back null, that means we hadn't yet allocated
            // for that slot, in which case we should do so now.
            if (allocateBuffer)
            {
                buffer = new T[_bufferLength];
            }

            return buffer;
        }

        /// <summary>
        /// Attempts to return the buffer to the bucket.  If successful, the buffer will be stored
        /// in the bucket; otherwise, the buffer won't be stored.
        /// </summary>
        internal void Return(T[] array)
        {
            // Check to see if the buffer is the correct size for this bucket
            if (array.Length != _bufferLength)
            {
                throw new ArgumentException($"The length of the buffer being returned was {array.Length}, which doesn't match the bucket's buffer length of {_bufferLength}.", nameof(array));
            }

            // While holding the spin lock, if there's room available in the bucket,
            // put the buffer into the next available slot.  Otherwise, we just drop it.
            // The try/finally is necessary to properly handle thread aborts on platforms
            // which have them.
            bool lockTaken = false;
            try
            {
                _lock.Enter(ref lockTaken);

                if (_index != 0)
                {
                    _buffers[--_index] = array;
                }
            }
            finally
            {
                if (lockTaken) _lock.Exit(false);
            }
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int SelectBucketIndex(int bufferSize)
    {
        uint bitsRemaining = ((uint)bufferSize - 1) >> 4;
        int poolIndex = 0;
        if (bitsRemaining > 0xFFFF) { bitsRemaining >>= 16; poolIndex = 16; }
        if (bitsRemaining > 0xFF) { bitsRemaining >>= 8; poolIndex += 8; }
        if (bitsRemaining > 0xF) { bitsRemaining >>= 4; poolIndex += 4; }
        if (bitsRemaining > 0x3) { bitsRemaining >>= 2; poolIndex += 2; }
        if (bitsRemaining > 0x1) { bitsRemaining >>= 1; poolIndex += 1; }

        return poolIndex + (int)bitsRemaining;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetMaxSizeForBucket(int binIndex)
    {
        return 16 << binIndex;
    }
}