using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System;

namespace Cool;

public static partial class Unchecked
{
    #region Unchecked String
    [StructLayout(LayoutKind.Sequential)]
    public sealed class String
    {
        public readonly int Length = 0;
        private char _firstChar = '\0';

        private String() { }

        public char this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.Add(ref _firstChar, index);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Unsafe.Add(ref _firstChar, index) = value;
        }

        public char this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.Add(ref _firstChar, index);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Unsafe.Add(ref _firstChar, index) = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref readonly char GetPinnableReference() => ref _firstChar;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => Unsafe.As<string>(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETFRAMEWORK
        public ReadOnlySpan<char> AsSpan() => new(Unsafe.As<Pinnable<char>>(this), (IntPtr)sizeof(int), Length);
#else
        public ReadOnlySpan<char> AsSpan() => new(ref _firstChar, Length);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator String(string value) => Unsafe.As<String>(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(String value) => Unsafe.As<string>(value);
        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Iterator GetEnumerator() => new(this);
        public ref struct Iterator(String str)
        {
            private int _index = -1;
            public readonly char Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => str[_index];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++_index < str.Length;
        }
        #endregion

    }
    #endregion
}