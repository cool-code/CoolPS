using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Sequential)]
    private sealed class Array4D
    {
        internal readonly LengthAndPadding LengthAndPadding;
        private readonly int dim1Length;
        private readonly int dim2Length;
        private readonly int dim3Length;
        private readonly int dim4Length;
        private readonly int dim1LowerBound;
        private readonly int dim2LowerBound;
        private readonly int dim3LowerBound;
        private readonly int dim4LowerBound;
        internal byte Data;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference<T>() => ref Unsafe.As<byte, T>(ref Data);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetFlattenedIndex(uint index1, uint index2, uint index3, uint index4) => (((((index1 * (uint)dim2Length) + index2) * (uint)dim3Length) + index3) * (uint)dim4Length) + index4;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetFlattenedIndex(int index1, int index2, int index3, int index4) => (uint)((((((index1 * dim2Length) + index2) * dim3Length) + index3) * dim4Length) + index4);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>(int index1, int index2, int index3, int index4) => ref Unsafe.Add(ref GetReference<T>(), GetFlattenedIndex(index1, index2, index3, index4));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>(uint index1, uint index2, uint index3, uint index4) => ref Unsafe.Add(ref GetReference<T>(), GetFlattenedIndex(index1, index2, index3, index4));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => dimension switch
        {
            0 => dim1Length,
            1 => dim2Length,
            2 => dim3Length,
            3 => dim4Length,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => dimension switch
        {
            0 => dim1Length,
            1 => dim2Length,
            2 => dim3Length,
            3 => dim4Length,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => dimension switch
        {
            0 => dim1LowerBound,
            1 => dim2LowerBound,
            2 => dim3LowerBound,
            3 => dim4LowerBound,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => dimension switch
        {
            0 => dim1LowerBound + dim1Length - 1,
            1 => dim2LowerBound + dim2Length - 1,
            2 => dim3LowerBound + dim3Length - 1,
            3 => dim4LowerBound + dim4Length - 1,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
    }

    #region Unchecked Array
    [StructLayout(LayoutKind.Sequential)]
    /// <summary>
    /// Lightweight, unchecked wrapper over a managed four-dimensional array.
    /// </summary>
    /// <remarks>
    /// - High-performance wrapper: intentionally omits bounds checks and other safety checks.
    /// - Only supports zero-based arrays Indexer, non-zero lower bounds are not supported.
    /// - Intended for use on .NET Framework 4.7+ and .NET 7+ (PowerShell scenarios).
    /// - The indexer mapping uses row-major order: offset = (((((index1 * dim2Length) + index2) * dim3Length) + index3) * dim4Length) + index4.
    /// - Callers must ensure indices are valid; out-of-range accesses are undefined behavior.
    /// </remarks>
    public readonly struct Array4D<T>
    {
        #region Fields and Constructor
        private readonly T[,,,] _array;
        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing 4D arrays.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Array4D(T[,,,] array) => _array = array;
        #endregion
        #region Properties and Indexer
        public int Rank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 4;
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)Unsafe.As<Array4D>(_array).LengthAndPadding.Length;
        }

        public long LongLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.As<Array4D>(_array).LengthAndPadding.Length;
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
        public ref T this[int index1, int index2, int index3, int index4]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<Array4D>(_array).Get<T>(index1, index2, index3, index4);
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<Array4D>(_array).Get<T>(index1, index2, index3, index4);
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref _array.GetReference<T>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[,,,] ToArray() => _array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => Unsafe.As<Array4D>(_array).GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => Unsafe.As<Array4D>(_array).GetLongLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => Unsafe.As<Array4D>(_array).GetLowerBound(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => Unsafe.As<Array4D>(_array).GetUpperBound(dimension);
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
        public T GetValue(int index1, int index2, int index3, int index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4) => this[(uint)index1, (uint)index2, (uint)index3, (uint)index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4) => this[(uint)index1, (uint)index2, (uint)index3, (uint)index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4) => this[(uint)index1, (uint)index2, (uint)index3, (uint)index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4) => this[(uint)index1, (uint)index2, (uint)index3, (uint)index4] = value;
        #endregion

        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array4D<T>(T[,,,] value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[,,,](Array4D<T> value) => value._array;
        #endregion

        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new(_array);
        public ref struct Enumerator
        {
            private readonly T[,,,] _array;
            private readonly uint _length;
            private uint _index;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(T[,,,] array)
            {
                _array = array;
                _length = Unsafe.As<Array4D>(_array).LengthAndPadding.Length;
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