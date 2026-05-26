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
    /// Lightweight, unchecked wrapper over a managed one-dimensional array.
    /// </summary>
    /// <remarks>
    /// - High-performance wrapper: intentionally omits bounds checks and other safety checks.
    /// - Supports only zero-based arrays; non-zero lower bounds are not supported.
    /// - Intended for use on .NET Framework 4.7+ and .NET 7+ (PowerShell scenarios).
    /// - Callers must ensure indices are valid; out-of-range accesses are undefined behavior.
    /// </remarks>
    public readonly struct Array<T>
    {
        #region Fields and Constructor
        private readonly T[] _array;
        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing arrays.
        private Array(T[] array) => _array = array;
        #endregion

        #region Properties and Indexer
        public int Rank => 1;
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
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref _array.GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray() => _array;
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
        public static implicit operator Array<T>(T[] value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[](Array<T> value) => value._array;
        #endregion
        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new(_array);
        public ref struct Enumerator
        {
            private readonly T[] _array;
            private readonly uint _length;
            private uint _index;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(T[] array)
            {
                _array = array;
                _length = (uint)array.Length;
                _index = uint.MaxValue;
            }
            public readonly ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref Unsafe.Add(ref _array.GetReference(), _index);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++_index < _length;
        }
        #endregion
    }
    #endregion
}