using System;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Cool;

public sealed class ArrayPool<T>
{
    public static readonly ArrayPool<T> Shared = new();

    private sealed class Bucket
    {
        // Slot struct: encapsulates the array and its version number, using a struct array for memory contiguity
        private struct Slot
        {
            // no need for volatile, as it's protected by the sequencing of Sequence
            internal T[]? Array;
            // The key to perfectly solving the timing issue: using volatile to ensure memory visibility of the version number
            internal volatile int Sequence;
        }

        private readonly Slot[] _slots;
        private readonly int _mask;
        private int _head = 0;
        private int _tail = 0;

        internal Bucket(int capacity)
        {
            _slots = new Slot[capacity];
            _mask = capacity - 1;
            // init each slot's sequence to its index, meaning it's ready for the first push at that index
            for (int i = 0; i < capacity; i++)
            {
                _slots[i].Sequence = i;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool TryPush(T[] array)
        {
            SpinWait spin = new();
            while (true)
            {
                int tail = Volatile.Read(ref _tail);
                int index = tail & _mask;
                int seq = _slots[index].Sequence;

                // compute the difference between the slot's sequence and the tail index to determine the slot's state
                int diff = seq - tail;

                if (diff == 0)
                {
                    // only when seq == tail, it proves the slot is empty and it's the turn for current tail to write
                    if (Interlocked.CompareExchange(ref _tail, tail + 1, tail) == tail)
                    {
                        _slots[index].Array = array;
                        // core: atomically increment the sequence number to notify Pop threads that the data is ready for reading
                        _slots[index].Sequence = tail + 1;
                        return true;
                    }
                }
                else if (diff < 0)
                {
                    // queue is full (tail has advanced a full cycle ahead of seq), reject the push and let GC handle it
                    return false;
                }

                // if diff > 0, it means other threads are writing, spin wait for the slot to become available
                spin.SpinOnce();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T[]? TryPop()
        {
            SpinWait spin = new();
            while (true)
            {
                int head = Volatile.Read(ref _head);
                int index = head & _mask;
                int seq = _slots[index].Sequence;

                // compute the difference between the slot's sequence and the head index to determine the slot's state
                int diff = seq - (head + 1);

                if (diff == 0)
                {
                    // only when seq == head + 1, it proves the Push thread has completely written the data
                    if (Interlocked.CompareExchange(ref _head, head + 1, head) == head)
                    {
                        T[]? result = _slots[index].Array;
                        _slots[index].Array = null; // clear the reference to avoid dangling


                        // core: update the sequence number to the next cycle's preparatory value, notifying Push threads that the slot is empty and ready for writing
                        _slots[index].Sequence = head + _slots.Length;
                        return result;
                    }
                }
                else if (diff < 0)
                {
                    // queue is empty (seq hasn't been advanced to head + 1 by the Push thread), return null to trigger new allocation
                    return null;
                }

                // if diff > 0, it means other threads are concurrently popping or the slot is not ready, spin wait for the slot to become ready
                spin.SpinOnce();
            }
        }
    }

    private readonly Bucket[] _buckets;
    public ArrayPool(int maxArrayLength = 1024 * 1024, int maxArraysPerBucket = 16)
    {
        if (maxArrayLength <= 0) throw new ArgumentOutOfRangeException(nameof(maxArrayLength));
        if (maxArraysPerBucket <= 0) throw new ArgumentOutOfRangeException(nameof(maxArraysPerBucket));

        var maxBucketIndex = SelectBucketIndex((uint)maxArrayLength);
        int capacity = 1;
        while (capacity < maxArraysPerBucket) capacity <<= 1;

        _buckets = new Bucket[maxBucketIndex + 1];
        for (int i = 0; i < _buckets.Length; i++)
        {
            _buckets[i] = new Bucket(capacity);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] Rent(int minimumLength)
    {
        if (minimumLength <= 0) return [];

        int index = SelectBucketIndex((uint)minimumLength);
        if (index >= _buckets.Length) return new T[minimumLength];

        T[]? array = _buckets[index].TryPop();
        return array ?? new T[1 << index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Return(T[] array, bool clearArray = false)
    {
        if (array == null || array.Length == 0) return;

        if (clearArray)
        {
            Array.Clear(array, 0, array.Length);
        }

        int index = SelectBucketIndex((uint)array.Length);
        if (index < _buckets.Length && array.Length == (1 << index))
        {
            _buckets[index].TryPush(array);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int SelectBucketIndex(uint value)
    {
        if (value <= 1) return 0;
        value--;
        int c = 0;
        if (value >= 1 << 16) { value >>= 16; c += 16; }
        if (value >= 1 << 8) { value >>= 8; c += 8; }
        if (value >= 1 << 4) { value >>= 4; c += 4; }
        if (value >= 1 << 2) { value >>= 2; c += 2; }
        if (value >= 1 << 1) { value >>= 1; c += 1; }
        return c + (int)value;
    }
}