using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.PowerShell.Commands;
namespace Cool;

public static class Console
{
    private const char ESC = '\u001B'; // Escape character
    private const string C0_CSI = "\u001B["; // Control Sequence Introducer (CSI) in C0 control codes
    private const char C1_CSI = '\u009B'; // Control Sequence Introducer (CSI) in C1 control codes
    private const char ZWJ = '\u200D'; // Zero Width Joiner
    private const char HIGH_SURROGATE_START = '\uD800';
    private const char HIGH_SURROGATE_END = '\uDBFF';
    private const char LOW_SURROGATE_START = '\uDC00';
    private const char LOW_SURROGATE_END = '\uDFFF';
    private static volatile bool _zwjSupported = true;
    private static volatile int _zeroWidth = 0;
    private static volatile int _ambiguousWidth = 1;
    private static volatile int _tabWidth = 8;
    private static volatile int _emftWidth = 0; // 0 = treat as zero-width, 1 = treat as half-width, 2 = treat as full-width
    public static void SetZWJSupport(bool supported)
    {
        _zwjSupported = supported;
    }
    public static void SetZeroWidthSupport(bool supported)
    {
        _zeroWidth = supported ? 0 : 1;
    }
    public static void SetAmbiguousAsWide(bool wide)
    {
        _ambiguousWidth = wide ? 2 : 1;
    }

    public static void SetTabWidth(int width)
    {
        if (width < 1) throw new ArgumentOutOfRangeException(nameof(width), "Tab width must be at least 1.");
        _tabWidth = width;
    }

    public static void SetEMFTWidth(int width)
    {
        if (width < 0 || width > 2) throw new ArgumentOutOfRangeException(nameof(width), "EMFT width must be 0 (zero-width), 1 (half-width), or 2 (full-width).");
        _emftWidth = width;
    }

    private const int MaxCodePoint = 0x3FFFF; // Covers all currently defined Unicode characters
    private static readonly ulong[] _wideBitmap = new ulong[(MaxCodePoint + 1) >> 6]; // 32KB
    private static readonly ulong[] _ambigBitmap = new ulong[(MaxCodePoint + 1) >> 6]; // 32KB

    private const string _wideRange = "1100-115F,231A-231B,2329-232A,23E9-23EC,23F0,23F3,25FD-25FE,2614-2615,2630-2637,2648-2653,267F,268A-268F,2693,26A1,26AA-26AB,26BD-26BE,26C4-26C5,26CE,26D4,26EA,26F2-26F3,26F5,26FA,26FD,2705,270A-270B,2728,274C,274E,2753-2755,2757,2795-2797,27B0,27BF,2B1B-2B1C,2B50,2B55,2E80-2E99,2E9B-2EF3,2F00-2FD5,2FF0-303E,3041-3096,3099-30FF,3105-312F,3131-318E,3190-31E5,31EF-321E,3220-3247,3250-A48C,A490-A4C6,A960-A97C,AC00-D7A3,F900-FAFF,FE10-FE19,FE30-FE52,FE54-FE66,FE68-FE6B,FF01-FF60,FFE0-FFE6,16FE0-16FE4,16FF0-16FF6,17000-18CD5,18CFF-18D1E,18D80-18DF2,1AFF0-1AFF3,1AFF5-1AFFB,1AFFD-1AFFE,1B000-1B122,1B132,1B150-1B152,1B155,1B164-1B167,1B170-1B2FB,1D300-1D356,1D360-1D376,1F004,1F0CF,1F18E,1F191-1F19A,1F200-1F202,1F210-1F23B,1F240-1F248,1F250-1F251,1F260-1F265,1F300-1F320,1F32D-1F335,1F337-1F37C,1F37E-1F393,1F3A0-1F3CA,1F3CF-1F3D3,1F3E0-1F3F0,1F3F4,1F3F8-1F43E,1F440,1F442-1F4FC,1F4FF-1F53D,1F54B-1F54E,1F550-1F567,1F57A,1F595-1F596,1F5A4,1F5FB-1F64F,1F680-1F6C5,1F6CC,1F6D0-1F6D2,1F6D5-1F6D8,1F6DC-1F6DF,1F6EB-1F6EC,1F6F4-1F6FC,1F7E0-1F7EB,1F7F0,1F90C-1F93A,1F93C-1F945,1F947-1F9FF,1FA70-1FA7C,1FA80-1FA8A,1FA8E-1FAC6,1FAC8,1FACD-1FADC,1FADF-1FAEA,1FAEF-1FAF8,20000-2FFFD,30000-3FFFD";
    private const string _ambigRange = "A1,A4,A7-A8,AA,AD-AE,B0-B4,B6-BA,BC-BF,C6,D0,D7-D8,DE-E1,E6,E8-EA,EC-ED,F0,F2-F3,F7-FA,FC,FE,101,111,113,11B,126-127,12B,131-133,138,13F-142,144,148-14B,14D,152-153,166-167,16B,1CE,1D0,1D2,1D4,1D6,1D8,1DA,1DC,251,261,2C4,2C7,2C9-2CB,2CD,2D0,2D8-2DB,2DD,2DF,300-36F,391-3A1,3A3-3A9,3B1-3C1,3C3-3C9,401,410-44F,451,2010,2013-2016,2018-2019,201C-201D,2020-2022,2024-2027,2030,2032-2033,2035,203B,203E,2074,207F,2081-2084,20AC,2103,2105,2109,2113,2116,2121-2122,2126,212B,2153-2154,215B-215E,2160-216B,2170-2179,2189,2190-2199,21B8-21B9,21D2,21D4,21E7,2200,2202-2203,2207-2208,220B,220F,2211,2215,221A,221D-2220,2223,2225,2227-222C,222E,2234-2237,223C-223D,2248,224C,2252,2260-2261,2264-2267,226A-226B,226E-226F,2282-2283,2286-2287,2295,2299,22A5,22BF,2312,2460-24E9,24EB-254B,2550-2573,2580-258F,2592-2595,25A0-25A1,25A3-25A9,25B2-25B3,25B6-25B7,25BC-25BD,25C0-25C1,25C6-25C8,25CB,25CE-25D1,25E2-25E5,25EF,2605-2606,2609,260E-260F,261C,261E,2640,2642,2660-2661,2663-2665,2667-266A,266C-266D,266F,269E-269F,26BF,26C6-26CD,26CF-26D3,26D5-26E1,26E3,26E8-26E9,26EB-26F1,26F4,26F6-26F9,26FB-26FC,26FE-26FF,273D,2776-277F,2B56-2B59,3248-324F,E000-F8FF,FE00-FE0F,FFFD,1F100-1F10A,1F110-1F12D,1F130-1F169,1F170-1F18D,1F18F-1F190,1F19B-1F1AC";

    unsafe static Console()
    {
        fixed (ulong* pWide = _wideBitmap, pAmbig = _ambigBitmap)
        {
            FillBitmap(_wideRange, pWide);
            FillBitmap(_ambigRange, pAmbig);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SetBit(ulong* pBitmap, int codePoint) => pBitmap[codePoint >> 6] |= 1UL << (codePoint & 63);
    private static unsafe void FillBitmap(string data, ulong* pBitmap)
    {
        if (string.IsNullOrEmpty(data)) return;
        string[] parts = data.Split(',');
        foreach (var part in parts)
        {
            int dashIndex = part.IndexOf('-');
            if (dashIndex > 0)
            {
                int start = int.Parse(part.Substring(0, dashIndex), NumberStyles.HexNumber);
                int end = Math.Min(MaxCodePoint, int.Parse(part.Substring(dashIndex + 1), NumberStyles.HexNumber));
                for (int i = start; i <= end; i++) SetBit(pBitmap, i);
            }
            else
            {
                int cp = int.Parse(part, NumberStyles.HexNumber);
                if (cp <= MaxCodePoint) SetBit(pBitmap, cp);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe bool GetBit(ulong* pBitmap, int codePoint) => (pBitmap[codePoint >> 6] & (1UL << (codePoint & 63))) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBetween(this char c, char start, char end) => (uint)(c - start) <= (uint)(end - start);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBetween(this char c, int start, int end) => (uint)(c - start) <= (uint)(end - start);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBetween(this int codePoint, int start, int end) => (uint)(codePoint - start) <= (uint)(end - start);
    // IsAsciiDigit is commonly used and simple enough to be inlined directly,
    // and it also avoids the overhead of a method call for such a small check,
    // especially in tight loops or performance-critical code paths
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiDigit(this char c) => (uint)(c - '0') <= 9;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiDigit(this int codePoint) => (uint)(codePoint - '0') <= 9;
    // .NET Core and later support inlining nested methods, but .NET Framework does not,
    // so IsC1Control does not call InRange but instead directly implements the logic
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsC1Control(this char c) => (uint)(c - 0x80) <= (0x9F - 0x80);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsC1Control(this int codePoint) => (uint)(codePoint - 0x80) <= (0x9F - 0x80);
    // This works because 'c' can never be -1.
    // 00..1F (+1) => 01..20 (&~80) => 01..20
    // 7F..9F (+1) => 80..A0 (&~80) => 00..20
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsControl(this int codePoint) => (((uint)codePoint + 1) & ~0x80u) <= 0x20u;
    // Emoji Modifier Fitzpatrick Type: U+1F3FB..U+1F3FF, used for skin tone modifiers in emojis,
    // should be treated as zero-width if ZWJ is not supported,
    // otherwise they are typically rendered as half-width or full-width depending on the terminal's emoji support,
    // so we allow configuring it as well
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEMFT(this int codePoint) => (uint)(codePoint - 0x1F3FB) <= (0x1F3FF - 0x1F3FB);
    // Variation Selectors: U+FE00..U+FE0F, used for emoji/text presentation variants and some other purposes
    // should be treated as zero-width since they do not have a visual representation on their own, but modify the preceding character
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsVariationSelector(this char c) => (uint)(c - 0xFE00) <= (0xFE0F - 0xFE00);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsVariationSelector(this int codePoint) => (uint)(codePoint - 0xFE00) <= (0xFE0F - 0xFE00);
    // Variation Selectors Supplement: U+E0100..U+E01EF, used for CJK Ideographic Variation Sequences (IVS)
    // should also be treated as zero-width since they modify the preceding character without adding visual width
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSupplementVariationSelector(this int codePoint) => (uint)(codePoint - 0xE0100) <= (0xE01EF - 0xE0100);
    // Zero-width characters: U+200B..U+200F (ZWSP, ZWNBSP, WJ, LRM, RLM) and U+FEFF (BOM) are treated as zero-width
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZeroWidth(this char c) => ((uint)(c - 0x200B) <= (0x200F - 0x200B)) || (c == '\uFEFF');
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZeroWidth(this int codePoint) => ((uint)(codePoint - 0x200B) <= (0x200F - 0x200B)) || (codePoint == 0xFEFF);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHighSurrogate(this char c) => (uint)(c - HIGH_SURROGATE_START) <= (HIGH_SURROGATE_END - HIGH_SURROGATE_START);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLowSurrogate(this char c) => (uint)(c - LOW_SURROGATE_START) <= (LOW_SURROGATE_END - LOW_SURROGATE_START);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHighSurrogate(this int codePoint) => (uint)(codePoint - HIGH_SURROGATE_START) <= (HIGH_SURROGATE_END - HIGH_SURROGATE_START);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLowSurrogate(this int codePoint) => (uint)(codePoint - LOW_SURROGATE_START) <= (LOW_SURROGATE_END - LOW_SURROGATE_START);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ToCodePoint(char highSurrogate, char lowSurrogate) => ((highSurrogate - HIGH_SURROGATE_START) << 10) + (lowSurrogate - LOW_SURROGATE_START) + 0x10000;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ToCodePoint(int highSurrogate, int lowSurrogate) => ((highSurrogate - HIGH_SURROGATE_START) << 10) + (lowSurrogate - LOW_SURROGATE_START) + 0x10000;



    // Remove C1 control characters (0x80-0x9F) and convert C1 CSI (0x9B) to ESC + [ (0x1B 0x5B) for better compatibility with terminals that may not support C1 controls
    public static unsafe string ToSafeString(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;

        int len = str.Length;
        fixed (char* pStr = str)
        {
            int i = 0;

            // --- first pass: fast scan ---
            // Find the first "breakpoint" character. If none found, return original string directly.
            while (i < len)
            {
                if (pStr[i].IsC1Control()) break;
                i++;
            }

            // if control characters are not found, return original string directly without renting StringBuilder
            if (i == len) return str;

            // --- second pass: allocate and clean ---
            // Only reach here if special characters are found, need to borrow StringBuilder from pool
            var sb = StringBuilderPool.Shared.Rent(len + 4);
            try
            {
                // first append the clean part before the first breakpoint, using char* overload to avoid intermediate string allocation
                if (i > 0) sb.Append(pStr, i);

                int start = i;
                while (i < len)
                {
                    char c = pStr[i];

                    if (c.IsC1Control())
                    {
                        // 1. Append the continuous valid segment from the last breakpoint to the current breakpoint
                        int count = i - start;
                        if (count > 0) sb.Append(pStr + start, count);

                        // 2. Handle the control character at the current breakpoint
                        // C1 CSI conversion: Convert C1 CSI (0x9B) to ESC + [ (0x1B 0x5B) for better compatibility with terminals that may not support C1 controls
                        if (c == C1_CSI) sb.Append(C0_CSI);
                        // Filter other C1 Control characters (0x80-0x9F) by default not appended

                        start = i + 1; // update start to the next character after the current breakpoint
                    }
                    i++;
                }

                // 3. Append the remaining valid segment at the end
                int remaining = len - start;
                if (remaining > 0) sb.Append(pStr + start, remaining);

                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Shared.Return(sb);
            }
        }
    }

    // Calculate the display width of the string, considering full-width characters, half-width characters, zero-width characters, variation selectors, and ANSI escape sequences
    public static unsafe int DisplayWidth(this string str)
    {
        if (string.IsNullOrEmpty(str)) return 0;

        int totalWidth = 0;
        int len = str.Length;

        // Get fixed pointers for the bitmaps and input string to allow for fast access without bounds checking.
        fixed (ulong* pWide = _wideBitmap, pAmbig = _ambigBitmap)
        fixed (char* pStr = str)
        {
            int i = 0;
            int cp = 0;
            while (i < len)
            {
                int prevcp = cp; // Store the previous code point for handling variation selectors that modify the preceding character
                cp = pStr[i++]; // Read the current character and move to the next index immediately for better performance in the common case

                if (cp.IsBetween(' ', '~')) // Fast path for common ASCII characters (printable)
                {
                    totalWidth += 1;
                    continue;
                }

                // --- 1. Core control sequence interception logic ---
                if (cp.IsControl())
                {
                    int start = i;
                    switch (cp)
                    {
                        case ESC:
                            // If it's ESC [, we skip both characters, otherwise we just skip the ESC character
                            if ((i < len) && (pStr[i] == '[')) { i += 1; } else { continue; }
                            break;
                        case C1_CSI:
                            //If it's C1 CSI, we treat it as ESC [ for better compatibility
                            break;
                        case '\t':
                            // For tabs, we can calculate the width based on the current totalWidth and the configured tab width
                            int spacesToNextTabStop = _tabWidth - (totalWidth % _tabWidth);
                            totalWidth += spacesToNextTabStop;
                            continue;
                        case '\b':
                            // For backspace, we can reduce the totalWidth by one character width, but not below zero
                            totalWidth = Math.Max(0, totalWidth - 1);
                            continue;
                        default:
                            // For other control characters, we simply skip them and do not count towards width
                            continue;
                    }

                    if (i >= len) break; // If we reached the end after skipping control character(s), break out of the loop

                    // Parse parameter part (digits, semicolons, colons)
                    char cmd;
                    do
                    {
                        cmd = pStr[i++];
                    } while ((i < len) && (cmd.IsAsciiDigit() || (cmd == ';') || (cmd == ':')));

                    // If we reached the end after parsing parameters, break out of the loop
                    // Note:
                    //   In well-formed ANSI sequences, we should encounter a command character (like 'm' for SGR) after the parameters,
                    //   but if the input is malformed and ends prematurely, we should just break to avoid out-of-bounds access
                    if (i >= len) break;

                    // if cmd is '#', we need to skip one more character for the push/pop modifier
                    // (like '{' 'p' for "push current style" and '}' 'q' for "pop style"),
                    // which is used in some extended ANSI sequences for better terminal state management
                    if (cmd == '#') i += 1; // skip the push/pop modifier character as well

                    // we do not need to specifically check for 'm', '#p', '#q', '#{', '#}' here,
                    // because any valid ANSI sequence should end with a command character that is not a digit, semicolon, or colon,
                    // and we have already skipped all the parameter characters,
                    // so we can just continue to the next iteration to process the next character after the ANSI sequence.
                    continue;
                }

                // --- 2. Handle zero-width joiner (ZWJ) ---
                if (_zwjSupported && cp == ZWJ)
                {
                    if (i < len)
                    {
                        int nextcp = pStr[i++];
                        if (nextcp.IsHighSurrogate())
                        {
                            int lowSurrogate = (i < len) ? pStr[i] : 0;
                            if (lowSurrogate.IsLowSurrogate())
                            {
                                nextcp = ToCodePoint(nextcp, lowSurrogate);
                                i += 1; // Move to the next character after the surrogate pair
                            }
                            else
                            {
                                totalWidth += 1; // If it's a malformed surrogate pair, just count the high surrogate as a normal character
                            }
                        }
                    }
                    continue;
                }

                // --- 3. Skip zero-width characters and variation selectors ---

                if (cp == 0xFE0F)
                {
                    totalWidth += ((prevcp <= MaxCodePoint) && (GetBit(pWide, prevcp) || (_ambiguousWidth > 1) && GetBit(pAmbig, prevcp))) ? _zeroWidth : 1;
                    continue;
                }

                if (cp.IsZeroWidth() || cp.IsVariationSelector())
                {
                    totalWidth += _zeroWidth;
                    continue;
                }

                if (i >= len)
                {
                    // If we reached the end after processing the current character, we can determine its width and break out of the loop
                    if (GetBit(pWide, cp)) totalWidth += 2;
                    else if (GetBit(pAmbig, cp)) totalWidth += _ambiguousWidth;
                    else totalWidth += 1;
                    break;
                }

                // --- 4. Handle surrogate pairs and bitmap lookup ---
                if (cp.IsHighSurrogate())
                {
                    int lowSurrogate = pStr[i];
                    if (lowSurrogate.IsLowSurrogate())
                    {
                        cp = ToCodePoint(cp, lowSurrogate);
                        i += 1; // Move to the next character after the surrogate pair
                        if (cp.IsEMFT())
                        {
                            totalWidth += _emftWidth;
                            continue;
                        }
                        if (cp.IsSupplementVariationSelector())
                        {
                            // Skip the Supplementary Variation Selectors (U+E0100..U+E01EF) which are used for CJK Ideographic Variation Sequences (IVS)
                            // They do not add width on their own and should be treated as zero-width modifiers for the preceding character
                            totalWidth += _zeroWidth * 2; // Treat them as zero-width, but if zero-width is disabled, treat them as full-width
                            continue;
                        }
                    }
                }

                if (cp <= MaxCodePoint)
                {
                    if (GetBit(pWide, cp)) totalWidth += 2;
                    else if (GetBit(pAmbig, cp)) totalWidth += _ambiguousWidth;
                    else totalWidth += 1;
                }
                else
                {
                    totalWidth += (cp.IsBetween(0xF0000, 0xFFFFD) || cp.IsBetween(0x100000, 0x10FFFD)) ? _ambiguousWidth : 1;
                }
            }
        }
        return totalWidth;
    }
}
