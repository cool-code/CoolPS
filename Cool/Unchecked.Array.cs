using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Explicit)]
    private readonly struct LengthAndPadding
    {
        [FieldOffset(0)]
        public readonly uint Length;
        [FieldOffset(0)]
        internal readonly UIntPtr lengthAndPadding;
    }

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
    public sealed partial class Array<T>
    {
        #region Fields and Constructor
        private readonly LengthAndPadding _lengthAndPadding = default;
        private byte _placeholder; // Placeholder for the start of the array data; actual type is T

        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing arrays.
        private Array() { }
        #endregion

        #region Properties and Indexer
        public int Rank => 1;
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
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref Unsafe.As<byte, T>(ref _placeholder);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray() => Unsafe.As<T[]>(this);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => dimension switch
        {
            0 => (int)_lengthAndPadding.Length,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => dimension switch
        {
            0 => 0,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => GetLowerBound(dimension) + GetLength(dimension) - 1;
        #endregion
        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array<T>(T[] value) => Unsafe.As<Array<T>>(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[](Array<T> value) => Unsafe.As<T[]>(value);
        #endregion
        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new(this);
        public ref struct Enumerator
        {
            private readonly Array<T> _array;
            private readonly uint _length;
            private uint _index;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(Array<T> array)
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