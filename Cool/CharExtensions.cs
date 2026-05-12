using System.Runtime.CompilerServices;

namespace Cool;

public static class CharExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBetween(this char c, char start, char end) => (uint)(c - start) <= (uint)(end - start);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this char c) => (uint)c <= 127;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsControl(this char c) => (uint)((c + 1) & ~0x80u) <= 0x20u;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsC1Control(this char c) => (c - 0x80u) <= (0x9Fu - 0x80u);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiDigit(this char c) => (uint)(c - '0') <= 9;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiHexDigit(this char c) => ((uint)(c - '0') <= 9) | (((c | 0x20u) - 'a') <= 5);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiHexDigitUpper(this char c) => ((uint)(c - '0') <= 9) | ((uint)(c - 'A') <= ('F' - 'A'));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiHexDigitLower(this char c) => ((uint)(c - '0') <= 9) | ((uint)(c - 'a') <= ('f' - 'a'));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiLetter(this char c) => ((c | 0x20u) - 'a') <= ('z' - 'a');

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiLetterUpper(this char c) => (uint)(c - 'A') <= ('Z' - 'A');

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiLetterLower(this char c) => (uint)(c - 'a') <= ('z' - 'a');
}