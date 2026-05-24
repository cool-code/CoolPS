using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{

    #region Unchecked Array
    [StructLayout(LayoutKind.Sequential)]
    public sealed class Array2D<T>
    {
        #region Fields and Constructor
        private readonly LengthAndPadding _lengthAndPadding = default;
        public readonly int _dim1Length;
        public readonly int _dim2Length;
        public readonly int _dim1LowerBound;
        public readonly int _dim2LowerBound;
        private T _firstElement = default!;

        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing 2D arrays.
        private Array2D() { }
        #endregion
        #region Properties and Indexer
        public int Rank => 2;
        public int Length => (int)_lengthAndPadding.Length;
        public long LongLength => _lengthAndPadding.Length;
        public bool IsFixedSize => true;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;
        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref _firstElement, index);
        }
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref _firstElement, (nint)index);
        }
        public ref T this[int index1, int index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref _firstElement, (index1 * (nint)_dim2Length) + index2);
        }
        public ref T this[uint index1, uint index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref _firstElement, (index1 * (nuint)_dim2Length) + index2);
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref _firstElement;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[,] ToArray() => Unsafe.As<T[,]>(this);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => dimension switch
        {
            0 => _dim1Length,
            1 => _dim2Length,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => dimension switch
        {
            0 => _dim1LowerBound,
            1 => _dim2LowerBound,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => GetLowerBound(dimension) + GetLength(dimension) - 1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK
        // Offset for 2D array is LengthAndPadding (4/8 bytes) + 2 lengths (2*4 bytes) + 2 lower bounds (2*4 bytes) = 20 bytes on 32-bit, 24 bytes on 64-bit
        public Span<T> AsSpan() => new(Unsafe.As<Pinnable<T>>(this), (IntPtr)UIntPtr.Size + (2 * 2 * sizeof(int)), Length & int.MaxValue);
#else
        public Span<T> AsSpan() => new(ref _firstElement, Length & int.MaxValue);
#endif
        #endregion

        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array2D<T>(T[,] value) => Unsafe.As<Array2D<T>>(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[,](Array2D<T> value) => Unsafe.As<T[,]>(value);
        #endregion

        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Iterator GetEnumerator() => new(this);
        public ref struct Iterator(Array2D<T> array)
        {
            private uint _index = uint.MaxValue;
            public readonly ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref array[_index];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++_index < array.LongLength;
        }
        #endregion
    }
    #endregion
}