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
        public readonly int Length;
        [FieldOffset(0)]
        internal readonly UIntPtr length_and_padding;
    }

    #region Unchecked Array
    [StructLayout(LayoutKind.Sequential)]
    public sealed class Array<T>
    {
        #region Fields and Constructor
        private readonly LengthAndPadding _length_and_padding = default;
        private T _firstElement = default!;

        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing arrays.
        private Array() { }
        #endregion

        #region Properties and Indexer
        public int Rank => 1;
        public int Length => _length_and_padding.Length;
        public long LongLength => _length_and_padding.Length;
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
            get => ref Unsafe.Add(ref _firstElement, index);
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref _firstElement;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray() => Unsafe.As<T[]>(this);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => dimension switch
        {
            0 => _length_and_padding.Length,
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK
        public Span<T> AsSpan() => new(Unsafe.As<Pinnable<T>>(this), (IntPtr)UIntPtr.Size, Length);
#else
        public Span<T> AsSpan() => new(ref _firstElement, Length);
#endif
        #endregion

        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array<T>(T[] value) => Unsafe.As<Array<T>>(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[](Array<T> value) => Unsafe.As<T[]>(value);
        #endregion

        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Iterator GetEnumerator() => new(this);
        public ref struct Iterator(Array<T> array)
        {
            private int _index = -1;
            public readonly ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref array[_index];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++_index < array.Length;
        }
        #endregion
    }
    #endregion
}