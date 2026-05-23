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
        private readonly LengthAndPadding _length_and_padding = default;
        private T _firstElement = default!;

        private Array() { }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _length_and_padding.Length;
        }

        public T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.Add(ref _firstElement, index);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Unsafe.Add(ref _firstElement, index) = value;
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.Add(ref _firstElement, index);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Unsafe.Add(ref _firstElement, index) = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref _firstElement;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray() => Unsafe.As<T[]>(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK
        public Span<T> AsSpan() => new(Unsafe.As<Pinnable<T>>(this), (IntPtr)UIntPtr.Size, Length);
#else
        public Span<T> AsSpan() => new(ref _firstElement, Length);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array<T>(T[] value) => Unsafe.As<Array<T>>(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[](Array<T> value) => Unsafe.As<T[]>(value);
    }
    #endregion
}