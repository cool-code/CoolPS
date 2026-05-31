using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct MethodTable
    {
        /// <summary>
        /// The base size of the type (used when allocating an instance on the heap).
        /// </summary>
        [FieldOffset(4)]
        public uint BaseSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    private unsafe struct PMethodTable
    {
        internal MethodTable* MethodTable;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal sealed class RawArray
    {
        internal readonly IntPtr LengthAndPadding;
        internal byte Data;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe nint GetOffset() => BaseSize - (sizeof(IntPtr) * 3);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T GetReference<T>(nint offset) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Data, offset));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T GetReference<T>() => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Data, BaseSize - (sizeof(IntPtr) * 3)));
        public unsafe nint BaseSize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref Data, (nint)sizeof(IntPtr) * 2)).MethodTable->BaseSize;
        }
        public unsafe uint Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.SubtractByteOffset(ref Unsafe.As<byte, uint>(ref Data), (nint)sizeof(IntPtr));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetLength(nint offset, int dimension)
        {
            if (offset == 0 && dimension == 0) return (int)Length;
            nint dim = dimension;
            nint rank = offset / (2 * sizeof(int));
            if (dim < 0 || dim >= rank) throw new IndexOutOfRangeException(nameof(dimension));
            return Unsafe.Add(ref Unsafe.As<byte, int>(ref Data), dim);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetLowerBound(nint offset, int dimension)
        {
            if (offset == 0 && dimension == 0) return 0;
            nint dim = dimension;
            nint rank = offset / (2 * sizeof(int));
            if (dim < 0 || dim >= rank) throw new IndexOutOfRangeException(nameof(dimension));
            return Unsafe.Add(ref Unsafe.As<byte, int>(ref Data), dim + rank);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetUpperBound(nint offset, int dimension)
        {
            if (offset == 0 && dimension == 0) return (int)Length - 1;
            nint dim = dimension;
            nint rank = offset / (2 * sizeof(int));
            if (dim < 0 || dim >= rank) throw new IndexOutOfRangeException(nameof(dimension));
            int length = Unsafe.Add(ref Unsafe.As<byte, int>(ref Data), dim);
            int lowerBound = Unsafe.Add(ref Unsafe.As<byte, int>(ref Data), dim + rank);
            return length + lowerBound - 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T GetElement<T>(nint offset, nint index) => ref Unsafe.Add(ref GetReference<T>(offset), index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T GetElement<T>(nint offset, uint index) => ref Unsafe.Add(ref GetReference<T>(offset), index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T Get<T>(nint offset, uint index)
        {
            if (offset == 0) return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref Data), index);
            if (offset > sizeof(int) * 2) return ref GetElement<T>(offset, index);
            index -= Unsafe.Add(ref Unsafe.As<byte, uint>(ref Data), 1);
            return ref GetElement<T>(offset, index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T Get<T>(nint offset, int index)
        {
            if (offset == 0) return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref Data), (uint)index);
            if (offset > sizeof(int) * 2) return ref GetElement<T>(offset, (uint)index);
            index -= Unsafe.Add(ref Unsafe.As<byte, int>(ref Data), 1);
            return ref GetElement<T>(offset, (uint)index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint GetFlattenedIndex(nint offset, uint index1, uint index2)
        {
            ref uint lengthStart = ref Unsafe.As<byte, uint>(ref Data);
            ref uint lowerBoundStart = ref Unsafe.AddByteOffset(ref lengthStart, offset >> 1);
            index2 -= Unsafe.Add(ref lowerBoundStart, 1);
            index1 -= Unsafe.Add(ref lowerBoundStart, 0);
            index1 *= Unsafe.Add(ref lengthStart, 1);
            return (uint)(index1 + index2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint GetFlattenedIndex(nint offset, int index1, int index2)
        {
            ref int lengthStart = ref Unsafe.As<byte, int>(ref Data);
            ref int lowerBoundStart = ref Unsafe.AddByteOffset(ref lengthStart, offset >> 1);
            index2 -= Unsafe.Add(ref lowerBoundStart, 1);
            index1 -= Unsafe.Add(ref lowerBoundStart, 0);
            index1 *= Unsafe.Add(ref lengthStart, 1);
            return (uint)(index1 + index2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T Get<T>(nint offset, int index1, int index2)
        {
            ref T reference = ref GetReference<T>(offset);
            uint index = GetFlattenedIndex(offset, index1, index2);
            return ref Unsafe.Add(ref reference, index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T Get<T>(nint offset, uint index1, uint index2)
        {
            ref T reference = ref GetReference<T>(offset);
            uint index = GetFlattenedIndex(offset, index1, index2);
            return ref Unsafe.Add(ref reference, index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint GetFlattenedIndex(nint offset, uint index1, uint index2, uint index3)
        {
            ref uint lengthStart = ref Unsafe.As<byte, uint>(ref Data);
            ref uint lowerBoundStart = ref Unsafe.AddByteOffset(ref lengthStart, offset >> 1);
            index3 -= Unsafe.Add(ref lowerBoundStart, 2);
            index2 -= Unsafe.Add(ref lowerBoundStart, 1);
            index1 -= Unsafe.Add(ref lowerBoundStart, 0);
            index1 *= Unsafe.Add(ref lengthStart, 1);
            index1 += index2;
            index1 *= Unsafe.Add(ref lengthStart, 2);
            return (uint)(index1 + index3);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint GetFlattenedIndex(nint offset, int index1, int index2, int index3)
        {
            ref int lengthStart = ref Unsafe.As<byte, int>(ref Data);
            ref int lowerBoundStart = ref Unsafe.AddByteOffset(ref lengthStart, offset >> 1);
            index3 -= Unsafe.Add(ref lowerBoundStart, 2);
            index2 -= Unsafe.Add(ref lowerBoundStart, 1);
            index1 -= Unsafe.Add(ref lowerBoundStart, 0);
            index1 *= Unsafe.Add(ref lengthStart, 1);
            index1 += index2;
            index1 *= Unsafe.Add(ref lengthStart, 2);
            return (uint)(index1 + index3);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T Get<T>(nint offset, int index1, int index2, int index3)
        {
            ref T reference = ref GetReference<T>(offset);
            uint index = GetFlattenedIndex(offset, index1, index2, index3);
            return ref Unsafe.Add(ref reference, index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T Get<T>(nint offset, uint index1, uint index2, uint index3)
        {
            ref T reference = ref GetReference<T>(offset);
            uint index = GetFlattenedIndex(offset, index1, index2, index3);
            return ref Unsafe.Add(ref reference, index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T Get<T>(nint offset, params int[] indices)
        {
            ref int index = ref indices[0];
            nint rank = offset / (2 * sizeof(int));
            nint flattenedIndex = 0;
            for (nint i = 0; i < rank; i++)
            {
                nint length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref Data, i * sizeof(int)));
                nint lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref Data, (i + rank) * sizeof(int)));
                flattenedIndex = (flattenedIndex * length) + ((nint)Unsafe.Add(ref index, i) - lowerBound);
            }
            flattenedIndex = rank == 0 ? index : flattenedIndex;
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Data, offset + (flattenedIndex * Unsafe.SizeOf<T>())));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T Get<T>(nint offset, params uint[] indices)
        {
            ref uint index = ref indices[0];
            nint rank = offset / (2 * sizeof(int));
            nint flattenedIndex = 0;
            for (nint i = 0; i < rank; i++)
            {
                nint length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref Data, i * sizeof(int)));
                nint lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref Data, (i + rank) * sizeof(int)));
                flattenedIndex = (flattenedIndex * length) + ((nint)Unsafe.Add(ref index, i) - lowerBound);
            }
            flattenedIndex = rank == 0 ? (nint)index : flattenedIndex;
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Data, offset + (flattenedIndex * Unsafe.SizeOf<T>())));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T Get<T>(nint offset, params long[] indices)
        {
            ref long index = ref indices[0];
            nint rank = offset / (2 * sizeof(int));
            nint flattenedIndex = 0;
            for (nint i = 0; i < rank; i++)
            {
                nint length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref Data, i * sizeof(int)));
                nint lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref Data, (i + rank) * sizeof(int)));
                flattenedIndex = (flattenedIndex * length) + ((nint)Unsafe.Add(ref index, i) - lowerBound);
            }
            flattenedIndex = rank == 0 ? (nint)index : flattenedIndex;
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Data, offset + (flattenedIndex * Unsafe.SizeOf<T>())));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T Get<T>(nint offset, params ulong[] indices)
        {
            ref ulong index = ref indices[0];
            nint rank = offset / (2 * sizeof(int));
            nint flattenedIndex = 0;
            for (nint i = 0; i < rank; i++)
            {
                nint length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref Data, i * sizeof(int)));
                nint lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref Data, (i + rank) * sizeof(int)));
                flattenedIndex = (flattenedIndex * length) + ((nint)Unsafe.Add(ref index, i) - lowerBound);
            }
            flattenedIndex = rank == 0 ? (nint)index : flattenedIndex;
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Data, offset + (flattenedIndex * Unsafe.SizeOf<T>())));
        }
    }

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
    public readonly struct Array<T>
    {
        #region Fields and Constructor
        private readonly Array _array;
        internal readonly nint _offset;
        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing arrays.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Array(Array array)
        {
            _array = array;
            _offset = Unsafe.As<RawArray>(_array).GetOffset();
        }
        #endregion

        #region Properties and Indexer
        public int Rank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int rank = MDArrayRank;
                return rank + ((rank - 1) >> 31 & 1);
            }
        }
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)Unsafe.As<RawArray>(_array).Length;
        }
        public long LongLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.As<RawArray>(_array).Length;
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
            get => _offset == 0;
        }
        public bool IsMDArray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _offset > 0;
        }
        // The rank of the array if it's a multi-dimensional array; otherwise, 0 for single-dimensional zero-based arrays.
        public int MDArrayRank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)(_offset / (2 * sizeof(int)));
        }
        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, index);
        }
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, index);
        }
        public ref T this[ulong index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, (uint)index);
        }
        public ref T this[long index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, (uint)index);
        }
        public ref T this[uint index1, uint index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, index1, index2);
        }
        public ref T this[int index1, int index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, index1, index2);
        }
        public ref T this[ulong index1, ulong index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, (uint)index1, (uint)index2);
        }
        public ref T this[long index1, long index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, (uint)index1, (uint)index2);
        }
        public ref T this[uint index1, uint index2, uint index3]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, index1, index2, index3);
        }
        public ref T this[int index1, int index2, int index3]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, index1, index2, index3);
        }
        public ref T this[ulong index1, ulong index2, ulong index3]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, (uint)index1, (uint)index2, (uint)index3);
        }
        public ref T this[long index1, long index2, long index3]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, (uint)index1, (uint)index2, (uint)index3);
        }
        public ref T this[params int[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, indices);
        }
        public ref T this[params uint[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, indices);
        }
        public ref T this[params long[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, indices);
        }
        public ref T this[params ulong[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(_offset, indices);
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref Unsafe.As<RawArray>(_array).GetReference<T>(_offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Array ToArray() => _array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => Unsafe.As<RawArray>(_array).GetLength(_offset, dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => Unsafe.As<RawArray>(_array).GetLength(_offset, dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => Unsafe.As<RawArray>(_array).GetLowerBound(_offset, dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => Unsafe.As<RawArray>(_array).GetUpperBound(_offset, dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3) => this[index1, index2, index3];
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
        public void SetValue(T value, int index1, int index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3) => this[index1, index2, index3] = value;
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