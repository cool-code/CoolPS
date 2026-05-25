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
    /// Lightweight, unchecked wrapper over a managed two-dimensional array.
    /// </summary>
    /// <remarks>
    /// - High-performance wrapper: intentionally omits bounds checks and other safety checks.
    /// - Supports only zero-based, rectangular 2-D arrays; non-zero lower bounds are not supported.
    /// - Intended for use on .NET Framework 4.7+ and .NET 7+ (PowerShell scenarios).
    /// - The indexer mapping uses row-major order: offset = index1 * dim2Length + index2.
    /// - Callers must ensure indices are valid; out-of-range accesses are undefined behavior.
    /// </remarks>
    public sealed class Array2D<T>
    {
        #region Fields and Constructor
        private readonly LengthAndPadding _lengthAndPadding = default;
        private readonly int _dim1Length;
        private readonly int _dim2Length;
        private readonly int _dim1LowerBound;
        private readonly int _dim2LowerBound;
        private byte _placeholder; // Placeholder for the start of the array data; actual type is T

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
            get => ref Unsafe.Add(ref GetReference(), (index1 * (nint)_dim2Length) + index2);
        }
        public ref T this[uint index1, uint index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref GetReference(), (index1 * (nuint)_dim2Length) + index2);
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref Unsafe.As<byte, T>(ref _placeholder);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
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
        #endregion

        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array2D<T>(T[,] value) => Unsafe.As<Array2D<T>>(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[,](Array2D<T> value) => Unsafe.As<T[,]>(value);
        #endregion

        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new(this);
        public ref struct Enumerator
        {
            private readonly Array2D<T> _array;
            private uint _index = uint.MaxValue;
            private readonly uint _length;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(Array2D<T> array)
            {
                _array = array;
                _length = array._lengthAndPadding.Length;
                _index = uint.MaxValue;
            }
            public readonly ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref _array[_index];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++_index < _length;
        }
        #endregion
    }
    #endregion
}