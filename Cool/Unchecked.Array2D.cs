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
    public readonly struct Array2D<T>
    {
        #region Fields and Constructor
        private readonly T[,] _array;
        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing 2D arrays.
        private Array2D(T[,] array) => _array = array;
        #endregion
        #region Properties and Indexer
        public int Rank => 2;
        public int Length => _array.Length;
        public long LongLength => _array.LongLength;
        public bool IsFixedSize => true;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => _array;
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
            get => ref Unsafe.Add(ref GetReference(), (index1 * (nint)_array.GetLength(1)) + index2);
        }
        public ref T this[uint index1, uint index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref GetReference(), (index1 * (nuint)_array.GetLength(1)) + index2);
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
        public int GetLength(int dimension) => _array.GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => _array.GetLongLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => _array.GetLowerBound(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => _array.GetUpperBound(dimension);
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
                _length = (uint)array.LongLength;
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