using System.Runtime.CompilerServices;

namespace Cool;

public static class CharExtensions
{
    extension(char c)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsBetween(char start, char end) => (c - start) <= (end - start);

        public bool IsAscii
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => c <= 127;
        }

        public bool IsControl
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((c + 1) & ~0x80u) <= 0x20u;
        }

        public bool IsC1Control
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (c - 0x80u) <= (0x9Fu - 0x80u);
        }

        public bool IsAsciiDigit
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => c - '0' <= 9;
        }

        public bool IsAsciiHexDigit
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (c - '0' <= 9) | ((c | 0x20u) - 'a' <= 5);
        }

        public bool IsAsciiHexDigitUpper
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (c - '0' <= 9) | (c - 'A' <= ('F' - 'A'));
        }

        public bool IsAsciiHexDigitLower
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (c - '0' <= 9) | (c - 'a' <= ('f' - 'a'));
        }

        public bool IsAsciiLetter
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ((c | 0x20u) - 'a') <= ('z' - 'a');
        }

        public bool IsAsciiLetterUpper
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => c - 'A' <= 'Z' - 'A';
        }

        public bool IsAsciiLetterLower
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => c - 'a' <= 'z' - 'a';
        }
    }
}