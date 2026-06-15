using System;

namespace Cool;

public static class Console
{
    private const char ESC = '\u001B'; // Escape character
    private const string C0_CSI = "\u001B["; // Control Sequence Introducer (CSI) in C0 control codes
    private const char C1_CSI = '\u009B'; // Control Sequence Introducer (CSI) in C1 control codes
    private const char TAB = '\t'; // Tab character
    private const char BACKSPACE = '\b'; // Backspace character 
    private const char ZWJ = '\u200D'; // Zero Width Joiner
    private const char VS16 = '\uFE0F'; // Variation Selector-16, used to specify emoji presentation
    private static volatile bool _zwjSupported = true; // Assume ZWJ support by default
    private static volatile int _zeroWidth = 0; // 0 = treat as zero-width, 1 = treat as non-zero-width
    private static volatile int _ambiguousWidth = 1; // 1 = treat ambiguous as narrow, 2 = treat ambiguous as wide
    private static volatile int _tabWidth = 8; // Default tab width is 8 spaces
    private static volatile int _emojiModifierWidth = 0; // 0 = treat as zero-width, 1 = treat as half-width, 2 = treat as full-width
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

    // Emoji Modifier Fitzpatrick Type characters can be treated as zero-width (like ZWJ), half-width, or full-width.
    // By default, they are treated as zero-width to allow for proper emoji composition, but some terminals may not support this well.
    // For example, Windows Terminal treats them as zero-width, Windows Console treats them as full-width,
    // and VS Code integrated terminal treats them as half-width.
    // This setting allows users to choose the behavior that works best for their terminal.
    public static void SetEmojiModifierWidth(int width)
    {
        if (width < 0 || width > 2) throw new ArgumentOutOfRangeException(nameof(width), "Emoji modifier width must be 0 (zero-width), 1 (half-width), or 2 (full-width).");
        _emojiModifierWidth = width;
    }

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

        // Get pointers for the bitmaps and input string to allow for fast access without bounds checking.
        fixed (char* pStr = str)
        {
            int i = 0;
            CodePoint cp = -1; // Initialize to an invalid code point to handle the case where the first character is a variation selector
            CodePoint prevcp = cp; // Store the previous code point for handling variation selectors that modify the preceding character
            while (i < len)
            {
                // Update prevcp only if the current code point is not zero-width, so that variation selectors can correctly modify the preceding non-zero-width character
                if (!cp.IsZeroWidth()) prevcp = cp;

                // Read the current character and move to the next index immediately for better performance in the common case
                cp = pStr[i++];

                if (cp.IsBetween(' ', '~')) // Fast path for common ASCII characters (printable)
                {
                    totalWidth += 1;
                    continue;
                }

                // --- 1. Core control sequence interception logic ---
                if (cp.IsControl())
                {
                    int start = i;
                    switch ((char)cp)
                    {
                        case ESC:
                            // If it's ESC [, we skip both characters, otherwise we just skip the ESC character
                            if ((i < len) && (pStr[i] == '[')) { i += 1; } else { continue; }
                            break;
                        case C1_CSI:
                            //If it's C1 CSI, we treat it as ESC [ for better compatibility
                            break;
                        case TAB:
                            // For tabs, we can calculate the width based on the current totalWidth and the configured tab width
                            int spacesToNextTabStop = _tabWidth - (totalWidth % _tabWidth);
                            totalWidth += spacesToNextTabStop;
                            continue;
                        case BACKSPACE:
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
                    if (!prevcp.IsValid())
                    {
                        // If ZWJ is at the start of the string,
                        // we treat it as a normal character to avoid losing width information
                        // for the subsequent characters that are supposed to be joined.
                        totalWidth += 1;
                        continue;
                    }
                    if (i < len)
                    {
                        CodePoint nextcp = pStr[i++];
                        if (nextcp.IsHighSurrogate())
                        {
                            CodePoint lowSurrogate = (i < len) ? pStr[i] : 0;
                            if (lowSurrogate.IsLowSurrogate())
                            {
                                nextcp += lowSurrogate;
                                i += 1; // Move to the next character after the surrogate pair
                            }
                            else
                            {
                                totalWidth += 1; // If it's a malformed surrogate pair, just count the high surrogate as a normal character
                            }
                        }

                        // If the next code point after ZWJ is an emoji, we treat the ZWJ and the emoji as zero-width to allow them to be joined together properly.
                        if (nextcp.IsEmoji()) continue;
                    }
                    continue;
                }

                // --- 3. Skip zero-width characters and variation selectors ---

                if (cp == VS16)
                {
                    totalWidth += (prevcp.IsWideWidth() || (_ambiguousWidth > 1) && prevcp.IsAmbiguousWidth()) ? _zeroWidth : 1;
                    continue;
                }

                if (i >= len)
                {
                    // If we reached the end after processing the current character, we can determine its width and break out of the loop
                    if (cp.IsZeroWidth()) totalWidth += _zeroWidth;
                    if (cp.IsWideWidth()) totalWidth += 2;
                    else if (cp.IsAmbiguousWidth()) totalWidth += _ambiguousWidth;
                    else totalWidth += 1;
                    break;
                }

                // --- 4. Handle surrogate pairs and bitmap lookup ---
                if (cp.IsHighSurrogate())
                {
                    CodePoint lowSurrogate = pStr[i];
                    if (lowSurrogate.IsLowSurrogate())
                    {
                        cp += lowSurrogate;
                        i += 1; // Move to the next character after the surrogate pair
                        if (cp.IsEmojiModifier())
                        {
                            totalWidth += _emojiModifierWidth;
                            continue;
                        }
                    }
                }

                // If it's zero-width, add zero-width for each character in the code point (1 for BMP, 2 for supplementary)
                if (cp.IsZeroWidth()) totalWidth += _zeroWidth * cp.CharCount;
                else if (cp.IsWideWidth()) totalWidth += 2;
                else if (cp.IsAmbiguousWidth()) totalWidth += _ambiguousWidth;
                else totalWidth += 1;
            }
        }
        return totalWidth;
    }
    /// <summary>
    /// Pads a string to a specific display width.
    /// </summary>
    /// <param name="str">string to pad</param>
    /// <param name="width">display width</param>
    /// <param name="align">
    /// <0: align left (pad right)
    ///  0: align center
    /// >0: align right (pad left) 
    /// </param>
    /// <returns></returns>
    public static string DisplayPad(this string str, int width, int align)
    {
        var currentWidth = DisplayWidth(str);
        var pad = width - currentWidth;
        if (pad <= 0) return str;
        string result = Unchecked.FastAllocateString(str.Length + pad);
        if (align < 0)
        {
            Unsafe.CopyBlockUnaligned(ref Unsafe.As<char, byte>(ref result.GetReference()), ref Unsafe.As<char, byte>(ref str.GetReference()), (uint)str.Length * sizeof(char));
            Unchecked.Fill(ref Unsafe.Add(ref result.GetReference(), str.Length), (uint)pad, ' ');
        }
        else if (align > 0)
        {
            Unchecked.Fill(ref result.GetReference(), (uint)pad, ' ');
            Unsafe.CopyBlockUnaligned(ref Unsafe.As<char, byte>(ref Unsafe.Add(ref result.GetReference(), pad)), ref Unsafe.As<char, byte>(ref str.GetReference()), (uint)str.Length * sizeof(char));
        }
        else
        {
            var leftPad = pad / 2;
            var rightPad = pad - leftPad;
            Unchecked.Fill(ref result.GetReference(), (uint)leftPad, ' ');
            Unsafe.CopyBlockUnaligned(ref Unsafe.As<char, byte>(ref Unsafe.Add(ref result.GetReference(), leftPad)), ref Unsafe.As<char, byte>(ref str.GetReference()), (uint)str.Length * sizeof(char));
            Unchecked.Fill(ref Unsafe.Add(ref result.GetReference(), str.Length + leftPad), (uint)rightPad, ' ');
        }
        return result;
    }

    public static string DisplayPadLeft(this string str, int width) => DisplayPad(str, width, 1);
    public static string DisplayPadRight(this string str, int width) => DisplayPad(str, width, -1);
    public static string DisplayPadCenter(this string str, int width) => DisplayPad(str, width, 0);
}
