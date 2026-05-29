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
        internal byte Placeholder;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe MethodTable* GetMethodTable() => Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref Placeholder, (nint)sizeof(IntPtr))).MethodTable;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe nint GetOffset()
        {
            ref byte placeholder = ref Placeholder;
            nint baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            return baseSize - (sizeof(IntPtr) * 2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T GetReference<T>(nint offset)
        {
            ref byte placeholder = ref Placeholder;
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, offset));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T GetReference<T>()
        {
            ref byte placeholder = ref Placeholder;
            nint baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, baseSize - (sizeof(IntPtr) * 2)));
        }
        public unsafe uint BaseSize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetMethodTable()->BaseSize;
        }
        public uint Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte placeholder = ref Placeholder;
                return Unsafe.As<byte, uint>(ref placeholder);
            }
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int GetLength(int dimension)
        {
            ref byte placeholder = ref Placeholder;
            nint dim = dimension;
            nint baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            nint rank = (baseSize - (3 * sizeof(IntPtr))) / (2 * sizeof(int));
            if (dim == 0 && rank == 0) return Unsafe.As<byte, int>(ref placeholder);
            if (dim < 0 || dim >= rank) throw new IndexOutOfRangeException(nameof(dimension));
            return Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (dim * sizeof(int)) + sizeof(IntPtr)));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int GetLowerBound(int dimension)
        {
            ref byte placeholder = ref Placeholder;
            nint dim = dimension;
            nint baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            nint rank = (baseSize - (3 * sizeof(IntPtr))) / (2 * sizeof(int));
            if (dim == 0 && rank == 0) return 0;
            if (dim < 0 || dim >= rank) throw new IndexOutOfRangeException(nameof(dimension));
            return Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, ((dim + rank) * sizeof(int)) + sizeof(IntPtr)));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int GetUpperBound(int dimension)
        {
            ref byte placeholder = ref Placeholder;
            nint dim = dimension;
            nint baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            nint rank = (baseSize - (3 * sizeof(IntPtr))) / (2 * sizeof(int));
            if (dim == 0 && rank == 0) return Unsafe.As<byte, int>(ref placeholder) - 1;
            if (dim < 0 || dim >= rank) throw new IndexOutOfRangeException(nameof(dimension));
            int length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (dim * sizeof(int)) + sizeof(IntPtr)));
            int lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, ((dim + rank) * sizeof(int)) + sizeof(IntPtr)));
            return length + lowerBound - 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ref T GetElement<T>(nint index, nint offset)
        {
            ref byte placeholder = ref Placeholder;
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, (index * Unsafe.SizeOf<T>()) + offset));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T Get<T>(nint index, nint offset)
        {
            ref byte placeholder = ref Placeholder;
            if (offset == sizeof(IntPtr)) return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, (index * Unsafe.SizeOf<T>()) + sizeof(IntPtr)));
            if (offset > sizeof(IntPtr) * 2) return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, (index * Unsafe.SizeOf<T>()) + offset));
            nint lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (nint)((2 * sizeof(int)) + sizeof(IntPtr))));
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, ((index - lowerBound) * Unsafe.SizeOf<T>()) + offset));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T Get<T>(params int[] indices)
        {
            ref int index = ref indices[0];
            ref byte placeholder = ref Placeholder;
            nint baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            nint rank = (baseSize - (3 * sizeof(IntPtr))) / (2 * sizeof(int));
            nint flattenedIndex = 0;
            for (nint i = 0; i < rank; i++)
            {
                nint length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (i * sizeof(int)) + sizeof(IntPtr)));
                nint lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, ((i + rank) * sizeof(int)) + sizeof(IntPtr)));
                flattenedIndex = (flattenedIndex * length) + ((nint)Unsafe.Add(ref index, i) - lowerBound);
            }
            flattenedIndex = rank == 0 ? index : flattenedIndex;
            nint offset = flattenedIndex * Unsafe.SizeOf<T>();
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, baseSize + offset - (nint)(sizeof(IntPtr) * 2)));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T Get<T>(params uint[] indices)
        {
            ref uint index = ref indices[0];
            ref byte placeholder = ref Placeholder;
            nint baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            nint rank = (baseSize - (3 * sizeof(IntPtr))) / (2 * sizeof(int));
            nint flattenedIndex = 0;
            for (nint i = 0; i < rank; i++)
            {
                nint length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (i * sizeof(int)) + sizeof(IntPtr)));
                nint lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, ((i + rank) * sizeof(int)) + sizeof(IntPtr)));
                flattenedIndex = (flattenedIndex * length) + ((nint)Unsafe.Add(ref index, i) - lowerBound);
            }
            flattenedIndex = rank == 0 ? (nint)index : flattenedIndex;
            nint offset = flattenedIndex * Unsafe.SizeOf<T>();
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, baseSize + offset - (sizeof(IntPtr) * 2)));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T Get<T>(params long[] indices)
        {
            ref long index = ref indices[0];
            ref byte placeholder = ref Placeholder;
            nint baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            nint rank = (baseSize - (3 * sizeof(IntPtr))) / (2 * sizeof(int));
            nint flattenedIndex = 0;
            for (nint i = 0; i < rank; i++)
            {
                nint length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (i * sizeof(int)) + sizeof(IntPtr)));
                nint lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, ((i + rank) * sizeof(int)) + sizeof(IntPtr)));
                flattenedIndex = (flattenedIndex * length) + ((nint)Unsafe.Add(ref index, i) - lowerBound);
            }
            flattenedIndex = rank == 0 ? (nint)index : flattenedIndex;
            nint offset = flattenedIndex * Unsafe.SizeOf<T>();
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, baseSize + offset - (sizeof(IntPtr) * 2)));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref T Get<T>(params ulong[] indices)
        {
            ref ulong index = ref indices[0];
            ref byte placeholder = ref Placeholder;
            nint baseSize = (nint)Unsafe.As<byte, PMethodTable>(ref Unsafe.SubtractByteOffset(ref placeholder, (nint)sizeof(IntPtr))).MethodTable->BaseSize;
            nint rank = (baseSize - (3 * sizeof(IntPtr))) / (2 * sizeof(int));
            nint flattenedIndex = 0;
            for (nint i = 0; i < rank; i++)
            {
                nint length = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, (i * sizeof(int)) + sizeof(IntPtr)));
                nint lowerBound = Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref placeholder, ((i + rank) * sizeof(int)) + sizeof(IntPtr)));
                flattenedIndex = (flattenedIndex * length) + ((nint)Unsafe.Add(ref index, i) - lowerBound);
            }
            flattenedIndex = rank == 0 ? (nint)index : flattenedIndex;
            nint offset = flattenedIndex * Unsafe.SizeOf<T>();
            return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref placeholder, baseSize + offset - (sizeof(IntPtr) * 2)));
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
        public ref T this[nuint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>((nint)index, _offset);
        }
        public ref T this[nint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(index, _offset);
        }
        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>((nint)index, _offset);
        }
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(index, _offset);
        }
        public ref T this[ulong index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>((nint)index, _offset);
        }
        public ref T this[long index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>((nint)index, _offset);
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
        public ref T this[params long[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(indices);
        }
        public ref T this[params ulong[] indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray>(_array).Get<T>(indices);
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
        public Enumerator GetEnumerator() => new(Unsafe.As<RawArray>(_array), _offset);
        public unsafe ref struct Enumerator
        {
#if NETFRAMEWORK
            private readonly RawArray _array;
            private readonly nint _offset;
#else
            private readonly ref T _baseRef;
#endif
            private readonly nint _length;
            private nint _index;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(RawArray array, nint offset)
            {
#if NETFRAMEWORK
                _array = array;
                _offset = offset;
#else
                _baseRef = ref array.GetReference<T>(offset);
#endif
                _length = (nint)array.Length;
                _index = -1;
            }
            public readonly ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK
                get => ref _array.GetElement<T>(_index, _offset);
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