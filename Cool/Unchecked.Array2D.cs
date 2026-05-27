using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Sequential)]
    private sealed class RawArray2D
    {
        internal readonly LengthAndPadding LengthAndPadding;
        private readonly int dim1Length;
        private readonly int dim2Length;
        private readonly int dim1LowerBound;
        private readonly int dim2LowerBound;
        internal byte Data;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference<T>() => ref Unsafe.As<byte, T>(ref Data);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetFlattenedIndex(uint index1, uint index2) => ((index1 - (uint)dim1LowerBound) * (uint)dim2Length) + (index2 - (uint)dim2LowerBound);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetFlattenedIndex(int index1, int index2) => ((uint)(index1 - dim1LowerBound) * (uint)dim2Length) + (uint)(index2 - dim2LowerBound);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>(int index1, int index2) => ref Unsafe.Add(ref GetReference<T>(), GetFlattenedIndex(index1, index2));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>(uint index1, uint index2) => ref Unsafe.Add(ref GetReference<T>(), GetFlattenedIndex(index1, index2));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => dimension switch
        {
            0 => dim1Length,
            1 => dim2Length,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => dimension switch
        {
            0 => dim1Length,
            1 => dim2Length,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => dimension switch
        {
            0 => dim1LowerBound,
            1 => dim2LowerBound,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => dimension switch
        {
            0 => dim1LowerBound + dim1Length - 1,
            1 => dim2LowerBound + dim2Length - 1,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
    }

    #region Unchecked Array
    [StructLayout(LayoutKind.Sequential)]
    /// <summary>
    /// Lightweight, unchecked wrapper over a managed two-dimensional array.
    /// </summary>
    /// <remarks>
    /// - High-performance wrapper: intentionally omits bounds checks and other safety checks.
    /// - Intended for use on .NET Framework 4.7+ and .NET 7+ (PowerShell scenarios).
    /// - The indexer mapping uses row-major order: offset = index1 * dim2Length + index2.
    /// - Callers must ensure indices are valid; out-of-range accesses are undefined behavior.
    /// </remarks>
    public readonly struct Array2D<T>
    {
        #region Fields and Constructor
        private readonly T[,] _array;
        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing 2D arrays.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Array2D(T[,] array) => _array = array;
        #endregion
        #region Properties and Indexer
        public int Rank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 2;
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)Unsafe.As<RawArray2D>(_array).LengthAndPadding.Length;
        }

        public long LongLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.As<RawArray2D>(_array).LengthAndPadding.Length;
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
        public ref T this[int index1, int index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray2D>(_array).Get<T>(index1, index2);
        }
        public ref T this[uint index1, uint index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<RawArray2D>(_array).Get<T>(index1, index2);
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref _array.GetReference<T>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[,] ToArray() => _array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => Unsafe.As<RawArray2D>(_array).GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => Unsafe.As<RawArray2D>(_array).GetLongLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => Unsafe.As<RawArray2D>(_array).GetLowerBound(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => Unsafe.As<RawArray2D>(_array).GetUpperBound(dimension);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2) => this[(uint)index1, (uint)index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2) => this[(uint)index1, (uint)index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2) => this[(uint)index1, (uint)index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2) => this[(uint)index1, (uint)index2] = value;
        #endregion

        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array2D<T>(T[,] value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[,](Array2D<T> value) => value._array;
        #endregion

        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new(_array);
        public ref struct Enumerator
        {
            private readonly T[,] _array;
            private readonly uint _length;
            private uint _index;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(T[,] array)
            {
                _array = array;
                _length = Unsafe.As<RawArray2D>(_array).LengthAndPadding.Length;
                _index = uint.MaxValue;
            }
            public readonly ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref Unsafe.Add(ref _array.GetReference<T>(), _index);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++_index < _length;
        }
        #endregion
    }
    #endregion
}