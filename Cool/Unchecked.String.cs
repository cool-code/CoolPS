using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class RawString
    {
        public readonly int Length = 0;
        internal char FirstChar = '\0';
    }

    #region Unchecked String
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct String
    {
        private readonly string _string;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private String(string str) => _string = str;

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _string.Length;
        }

        public char this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.Add(ref _string.GetReference(), index);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Unsafe.Add(ref _string.GetReference(), index) = value;
        }

        public char this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.Add(ref _string.GetReference(), index);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Unsafe.Add(ref _string.GetReference(), index) = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref readonly char GetPinnableReference() => ref Unsafe.AsRef<string, RawString>(_string).FirstChar;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref char GetReference() => ref Unsafe.AsRef<string, RawString>(_string).FirstChar;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString() => _string;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator String(string value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(String value) => value._string;

        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new(_string);
        public ref struct Enumerator
        {
            private readonly string _str;
            private int _index;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(string str)
            {
                _str = str;
                _index = -1;
            }
            public readonly char Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => Unsafe.Add(ref _str.GetReference(), _index);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++_index < _str.Length;
        }
        #endregion

    }
    #endregion
}