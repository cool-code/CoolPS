using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{
    #region Unchecked Array
    [StructLayout(LayoutKind.Sequential)]
    /// <summary>
    /// Lightweight, unchecked wrapper over a managed array.
    /// </summary>
    /// <remarks>
    /// - High-performance wrapper: intentionally omits bounds checks and other safety checks.
    /// - Intended for use on .NET Framework 4.7+ and .NET 7+ (PowerShell scenarios).
    /// - Callers must ensure indices are valid; out-of-range accesses are undefined behavior.
    /// </remarks>
    public readonly partial struct Array<T>
    {
        #region Fields and Constructor
        private readonly Array _array;
        internal readonly uint _offset;
        internal readonly uint _rank;
        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing arrays.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe Array(Array array)
        {
            _array = array;
            _offset = Unsafe.GetBaseSize(array) - (uint)(3 * sizeof(IntPtr));
            _rank = _offset / (2 * sizeof(int));
        }
        #endregion

        #region Properties and Indexer
        public int Rank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (int)(_rank ^ ((_rank - 1) >> 31));
            }
        }
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)_array.GetLength();
        }
        public long LongLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _array.GetLength();
        }
        public bool IsFixedSize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => true;
        }
        public bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }
        public bool IsSynchronized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }
        public object SyncRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _array;
        }
        public bool IsSZArray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _rank == 0;
        }
        public bool IsMDArray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _rank > 0;
        }
        // The rank of the array if it's a multi-dimensional array; otherwise, 0 for single-dimensional zero-based arrays.
        public int MDArrayRank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)_rank;
        }
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                if (_rank == 0) return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref rawData), index);
                index -= Unsafe.Add(ref Unsafe.As<byte, int>(ref rawData), _rank);
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)((uint)index * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                if (_rank == 0) return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref rawData), index);
                index -= Unsafe.Add(ref Unsafe.As<byte, uint>(ref rawData), _rank);
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(index * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[long index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                if (_rank == 0) return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref rawData), (nint)index);
                index -= Unsafe.Add(ref Unsafe.As<byte, int>(ref rawData), _rank);
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(index * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[ulong index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                if (_rank == 0) return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref rawData), (nuint)index);
                index -= Unsafe.Add(ref Unsafe.As<byte, uint>(ref rawData), 1);
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, _offset + (nuint)(index * (ulong)Unsafe.SizeOf<T>())));
            }
        }
        private ref T GetReference(scoped ref int index, int indexLength)
        {
            if (indexLength == 1) return ref this[index];
            ref byte rawData = ref _array.GetRawArrayData();
            ref int length = ref Unsafe.As<byte, int>(ref rawData);
            ref int bound = ref Unsafe.Add(ref length, _rank);
            int flattenedIndex = index - bound;
            unchecked
            {
                for (int i = 1; i < indexLength - 1; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                    flattenedIndex += Unsafe.Add(ref index, i) - Unsafe.Add(ref bound, i);
                }
                for (int i = indexLength - 1; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += Unsafe.Add(ref index, indexLength - 1) - Unsafe.Add(ref bound, indexLength - 1);
            }
            return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
        }
        public ref T this[params int[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref GetReference(ref indices.GetReference(), indices.Length);
        }
        private ref T GetReference(scoped ref uint index, int indexLength)
        {
            if (indexLength == 1) return ref this[index];
            ref byte rawData = ref _array.GetRawArrayData();
            ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
            ref uint bound = ref Unsafe.Add(ref length, _rank);
            uint flattenedIndex = index - bound;
            unchecked
            {
                for (int i = 1; i < indexLength - 1; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                    flattenedIndex += Unsafe.Add(ref index, i) - Unsafe.Add(ref bound, i);
                }
                for (int i = indexLength - 1; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += Unsafe.Add(ref index, indexLength - 1) - Unsafe.Add(ref bound, indexLength - 1);
            }
            return ref Unsafe.Add(ref GetReference(), flattenedIndex);
        }
        public ref T this[params uint[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref GetReference(ref indices.GetReference(), indices.Length);
        }
        private ref T GetReference(scoped ref long index, int indexLength)
        {
            if (indexLength == 1) return ref this[index];
            ref byte rawData = ref _array.GetRawArrayData();
            ref int length = ref Unsafe.As<byte, int>(ref rawData);
            ref int bound = ref Unsafe.Add(ref length, _rank);
            long flattenedIndex = index - bound;
            unchecked
            {
                for (int i = 1; i < indexLength - 1; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                    flattenedIndex += Unsafe.Add(ref index, i) - Unsafe.Add(ref bound, i);
                }
                for (int i = indexLength - 1; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += Unsafe.Add(ref index, indexLength - 1) - Unsafe.Add(ref bound, indexLength - 1);
            }
            return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
        }
        public ref T this[params long[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref GetReference(ref indices.GetReference(), indices.Length);
        }
        private ref T GetReference(scoped ref ulong index, int indexLength)
        {
            if (indexLength == 1) return ref this[index];
            ref byte rawData = ref _array.GetRawArrayData();
            ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
            ref uint bound = ref Unsafe.Add(ref length, _rank);
            ulong flattenedIndex = index - bound;
            unchecked
            {

                for (int i = 1; i < indexLength - 1; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                    flattenedIndex += Unsafe.Add(ref index, i) - Unsafe.Add(ref bound, i);
                }
                for (int i = indexLength - 1; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += Unsafe.Add(ref index, indexLength - 1) - Unsafe.Add(ref bound, indexLength - 1);
            }
            return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
        }
        public ref T this[params ulong[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref GetReference(ref indices.GetReference(), indices.Length);
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref _array.GetRawArrayData(), _offset));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Array ToArray() => _array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(uint dimension)
        {
            if (_rank == 0 && dimension == 0) return Length;
            if (dimension >= _rank) return 0;
            return Unsafe.Add(ref Unsafe.As<byte, int>(ref _array.GetRawArrayData()), dimension);
        }
        public int GetLength(int dimension)
        {
            if (_rank == 0 && dimension == 0) return Length;
            if ((uint)dimension >= _rank) ThrowIndexOutOfRange();
            return Unsafe.Add(ref Unsafe.As<byte, int>(ref _array.GetRawArrayData()), dimension);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(uint dimension) => GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(uint dimension)
        {
            if (_rank == 0 && dimension == 0) return 0;
            if (dimension >= _rank) return 0;
            return Unsafe.Add(ref Unsafe.As<byte, int>(ref _array.GetRawArrayData()), dimension + _rank);
        }
        public int GetLowerBound(int dimension)
        {
            if (_rank == 0 && dimension == 0) return 0;
            if ((uint)dimension >= _rank) ThrowIndexOutOfRange();
            return Unsafe.Add(ref Unsafe.As<byte, int>(ref _array.GetRawArrayData()), (uint)dimension + _rank);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(uint dimension)
        {
            if (_rank == 0 && dimension == 0) return Length - 1;
            if (dimension >= _rank) return -1;
            ref int length = ref Unsafe.Add(ref Unsafe.As<byte, int>(ref _array.GetRawArrayData()), dimension);
            ref int lowerBound = ref Unsafe.Add(ref length, _rank);
            return length + lowerBound - 1;
        }
        public int GetUpperBound(int dimension)
        {
            if (_rank == 0 && dimension == 0) return Length - 1;
            if ((uint)dimension >= _rank) ThrowIndexOutOfRange();
            ref int length = ref Unsafe.Add(ref Unsafe.As<byte, int>(ref _array.GetRawArrayData()), dimension);
            ref int lowerBound = ref Unsafe.Add(ref length, _rank);
            return length + lowerBound - 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params int[] indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params uint[] indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params long[] indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params ulong[] indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params int[] indices) => this[indices] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params uint[] indices) => this[indices] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params long[] indices) => this[indices] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params ulong[] indices) => this[indices] = value;
        #endregion
        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array<T>(Array value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array(Array<T> value) => value._array;
        #endregion
        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayEnumerator<T> GetEnumerator() => new(_array, _offset);
        #endregion
    }
    #endregion
}