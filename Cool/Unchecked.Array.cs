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
        /// The low WORD of the first field is the component size for array and string types.
        /// </summary>
        [FieldOffset(0)]
        public ushort ComponentSize;

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
    private sealed class RawArray
    {
        internal byte Placeholder;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe MethodTable* GetMethodTable() => Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref Placeholder, (nint)sizeof(IntPtr))).MethodTable;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T GetReference<T>()
        {
            ref byte placeholder = ref Placeholder;
            uint baseSize = Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, (nint)baseSize - (nint)(sizeof(IntPtr) * 2)));
        }
        public unsafe uint BaseSize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetMethodTable()->BaseSize;
        }
        public unsafe uint ComponentSize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetMethodTable()->ComponentSize;
        }
        public unsafe bool IsSZArray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => BaseSize == (uint)(3 * sizeof(IntPtr));
        }
        public unsafe bool IsMultiDimensionalArray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => BaseSize > (uint)(3 * sizeof(IntPtr));
        }
        // Returns rank of multi-dimensional array rank, 0 for sz arrays
        public unsafe int MultiDimensionalArrayRank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)((BaseSize - (uint)(3 * sizeof(IntPtr))) / (2 * sizeof(int)));
        }
        public unsafe int Rank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                uint rank = (BaseSize - (uint)(3 * sizeof(IntPtr))) / (2 * sizeof(int));
                return (int)(rank ^ ((rank - 1) >> 31));
            }
        }
        public uint Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.As<byte, uint>(ref Placeholder);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int GetLength(int dimension)
        {
            ref byte placeholder = ref Placeholder;
            uint baseSize = Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            int rank = (int)((baseSize - (uint)(3 * sizeof(IntPtr))) / (2 * sizeof(int)));
            if (dimension == 0 && rank == 0) return Unsafe.As<byte, int>(ref placeholder);
            if (dimension < 0 || dimension >= rank) throw new IndexOutOfRangeException(nameof(dimension));
            return Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)(dimension * sizeof(int)) + (nint)sizeof(IntPtr)));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int GetLowerBound(int dimension)
        {
            ref byte placeholder = ref Placeholder;
            uint baseSize = Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            int rank = (int)((baseSize - (uint)(3 * sizeof(IntPtr))) / (2 * sizeof(int)));
            if (dimension == 0 && rank == 0) return 0;
            if (dimension < 0 || dimension >= rank) throw new IndexOutOfRangeException(nameof(dimension));
            return Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)((dimension + rank) * sizeof(int)) + (nint)sizeof(IntPtr)));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int GetUpperBound(int dimension)
        {
            ref byte placeholder = ref Placeholder;
            uint baseSize = Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            int rank = (int)((baseSize - (uint)(3 * sizeof(IntPtr))) / (2 * sizeof(int)));
            if (dimension == 0 && rank == 0) return Unsafe.As<byte, int>(ref placeholder) - 1;
            if (dimension < 0 || dimension >= rank) throw new IndexOutOfRangeException(nameof(dimension));
            int length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)(dimension * sizeof(int)) + (nint)sizeof(IntPtr)));
            int lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)((dimension + rank) * sizeof(int)) + (nint)sizeof(IntPtr)));
            return length + lowerBound - 1;
        }
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // private unsafe uint GetFlattenedIndex(params int[] indices)
        // {
        //     ref int index = ref indices[0];
        //     ref byte placeholder = ref Placeholder;
        //     uint baseSize = Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
        //     int rank = (int)((baseSize - (uint)(3 * sizeof(IntPtr))) / (2 * sizeof(int)));
        //     uint flattenedIndex = 0;
        //     for (int i = 0; i < rank; i++)
        //     {
        //         int length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)(i * sizeof(int)) + (nint)sizeof(IntPtr)));
        //         int lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)((i + rank) * sizeof(int)) + (nint)sizeof(IntPtr)));
        //         flattenedIndex = (flattenedIndex * (uint)length) + (uint)(Unsafe.Add(ref index, i) - lowerBound);
        //     }
        //     return rank == 0 ? (uint)index : flattenedIndex;
        // }
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // private unsafe uint GetFlattenedIndex(params uint[] indices)
        // {
        //     ref uint index = ref indices[0];
        //     ref byte placeholder = ref Placeholder;
        //     uint baseSize = Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
        //     int rank = (int)((baseSize - (uint)(3 * sizeof(IntPtr))) / (2 * sizeof(int)));
        //     uint flattenedIndex = 0;
        //     for (int i = 0; i < rank; i++)
        //     {
        //         int length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)(i * sizeof(int)) + (nint)sizeof(IntPtr)));
        //         int lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)((i + rank) * sizeof(int)) + (nint)sizeof(IntPtr)));
        //         flattenedIndex = (flattenedIndex * (uint)length) + (Unsafe.Add(ref index, i) - (uint)lowerBound);
        //     }
        //     return rank == 0 ? index : flattenedIndex;
        // }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T Get<T>(params uint[] indices)
        {
            ref uint index = ref indices[0];
            ref byte placeholder = ref Placeholder;
            uint baseSize = Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            int rank = (int)((baseSize - (uint)(3 * sizeof(IntPtr))) / (2 * sizeof(int)));
            uint flattenedIndex = 0;
            for (int i = 0; i < rank; i++)
            {
                int length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)(i * sizeof(int)) + (nint)sizeof(IntPtr)));
                int lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)((i + rank) * sizeof(int)) + (nint)sizeof(IntPtr)));
                flattenedIndex = (flattenedIndex * (uint)length) + (Unsafe.Add(ref index, i) - (uint)lowerBound);
            }
            flattenedIndex = rank == 0 ? index : flattenedIndex;
            nint offset = (nint)flattenedIndex * Unsafe.SizeOf<T>();
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, (nint)baseSize + offset - (nint)(sizeof(IntPtr) * 2)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T Get<T>(params int[] indices)
        {
            ref int index = ref indices[0];
            ref byte placeholder = ref Placeholder;
            uint baseSize = Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            int rank = (int)((baseSize - (uint)(3 * sizeof(IntPtr))) / (2 * sizeof(int)));
            uint flattenedIndex = 0;
            for (int i = 0; i < rank; i++)
            {
                int length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)(i * sizeof(int)) + (nint)sizeof(IntPtr)));
                int lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)((i + rank) * sizeof(int)) + (nint)sizeof(IntPtr)));
                flattenedIndex = (flattenedIndex * (uint)length) + (uint)(Unsafe.Add(ref index, i) - lowerBound);
            }
            flattenedIndex = rank == 0 ? (uint)index : flattenedIndex;
            nint offset = (nint)flattenedIndex * Unsafe.SizeOf<T>();
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, (nint)baseSize + offset - (nint)(sizeof(IntPtr) * 2)));
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
        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing arrays.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Array(Array array) => _array = array;
        #endregion

        #region Properties and Indexer
        public int Rank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.As<RawArray>(_array).Rank;
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
        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref GetReference(), index);
        }
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref GetReference(), (nint)index);
        }
        public ref T this[params int[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(indices);
        }
        public ref T this[params uint[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(indices);
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref _array.GetReference<T>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Array ToArray() => _array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => Unsafe.As<RawArray>(_array).GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => Unsafe.As<RawArray>(_array).GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => Unsafe.As<RawArray>(_array).GetLowerBound(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => Unsafe.As<RawArray>(_array).GetUpperBound(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index) => this[(uint)index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index) => this[(uint)index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index) => this[(uint)index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index) => this[(uint)index] = value;
        #endregion
        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array<T>(Array value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array(Array<T> value) => value._array;
        #endregion
        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new(_array);
        public unsafe ref struct Enumerator
        {
#if NETFRAMEWORK
            private readonly RawArray _array;
            private readonly nint _baseSize;
#else
            private readonly ref T _baseRef;
#endif
            private readonly uint _length;
            private uint _index;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(Array array)
            {
#if NETFRAMEWORK
                _array = Unsafe.As<RawArray>(array);
                ref byte baseRef = ref _array.Placeholder;
                _baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref baseRef, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
#else
                ref byte baseRef = ref Unsafe.As<RawArray>(array).Placeholder;
                nint baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref baseRef, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
                _baseRef = ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref baseRef, (nint)baseSize - (nint)(sizeof(IntPtr) * 2)));
#endif
                _length = Unsafe.As<byte, uint>(ref baseRef);
                _index = uint.MaxValue;
            }
            public readonly ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK
                get
                {
                    ref byte baseRef = ref _array.Placeholder;
                    nint size = _baseSize;
                    return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref baseRef, size - (nint)(sizeof(IntPtr) * 2) + ((nint)_index * Unsafe.SizeOf<T>())));
                }
#else
                get => ref Unsafe.Add(ref _baseRef, _index);
#endif
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++_index < _length;
        }
        #endregion
    }
    #endregion
}