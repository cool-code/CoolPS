using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Cool;

public static class Ansi
{
    public static readonly string C0_CSI = "\u001b[";
    public static readonly string SGR = "m";
    public static readonly string Reset = C0_CSI + "0" + SGR;
    public static readonly string Bold = C0_CSI + "1" + SGR;
    public static readonly string Dim = C0_CSI + "2" + SGR;
    public static readonly string Italic = C0_CSI + "3" + SGR;
    public static readonly string Underline = C0_CSI + "" + SGR;
    public static readonly string Inverse = C0_CSI + "7" + SGR;
    public static readonly string Hidden = C0_CSI + "8" + SGR;
    public static readonly string Strikethrough = C0_CSI + "9" + SGR;
    public static readonly string Normal = C0_CSI + "22" + SGR;
    public static readonly string NoBold = C0_CSI + "22" + SGR;
    public static readonly string NoDim = C0_CSI + "22" + SGR;
    public static readonly string NoItalic = C0_CSI + "23" + SGR;
    public static readonly string NoUnderline = C0_CSI + "24" + SGR;
    public static readonly string NoInverse = C0_CSI + "27" + SGR;
    public static readonly string NoHidden = C0_CSI + "28" + SGR;
    public static readonly string NoStrikethrough = C0_CSI + "29" + SGR;
    public static readonly string DefaultForeground = C0_CSI + "39" + SGR;
    public static readonly string DefaultBackground = C0_CSI + "49" + SGR;
    private static readonly string[] _fg256Cache = AnsiCacheBridge.Init256ColorCache(true);
    private static readonly string[] _bg256Cache = AnsiCacheBridge.Init256ColorCache(false);
    private static readonly string[] _fg16Cache = AnsiCacheBridge.Init16ColorCache(true);
    private static readonly string[] _bg16Cache = AnsiCacheBridge.Init16ColorCache(false);
    private static class AnsiCacheBridge
    {
        internal static string[] Init256ColorCache(bool isForeground)
        {
            string control = isForeground ? "38;5;" : "48;5;";
            string[] cache = new string[256];
            for (int i = 0; i < 256; i++) cache[i] = C0_CSI + control + i + "m";
            return cache;
        }

        internal static string[] Init16ColorCache(bool isForeground)
        {
            string[] cache = new string[16];
            int baseCode = isForeground ? 30 : 40;
            for (int i = 0; i < 16; i++)
            {
                // foreground color: i < 8 is standard color (30-37)，i >= 8 is bright color (90-97)
                // background color: i < 8 is standard color (40-47)，i >= 8 is bright color (100-107)
                int code = (i < 8) ? (baseCode + i) : (baseCode + 60 + (i - 8));
                cache[i] = C0_CSI + code + "m";
            }
            return cache;
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 32)]
    public struct RGBHeader;
    private static readonly RGBHeader _foregroundHeader = Unsafe.As<char, RGBHeader>(ref Unchecked.GetReference("\x1b[38;2;000;000;0"));
    private static readonly RGBHeader _backgroundHeader = Unsafe.As<char, RGBHeader>(ref Unchecked.GetReference("\x1b[48;2;000;000;0"));
    private static readonly char[] _pCache = InitNativeCache();
    private static char[] InitNativeCache()
    {
        char[] cache = new char[256 * 3];
        ref char cacheRef = ref cache.GetReference();
        for (int i = 0; i < 256; i++)
        {
            Unchecked.Write(ref cacheRef, i, (char)('0' + (i / 100 % 10)));
            Unchecked.Write(ref cacheRef, 256 + i, (char)('0' + (i / 10 % 10)));
            Unchecked.Write(ref cacheRef, 512 + i, (char)('0' + (i % 10)));
        }
        return cache;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void FillForegroundRGB(ref char buffer, byte r, byte g, byte b)
    {
        ref char hpCache = ref _pCache.GetReference();
        ref char tpCache = ref Unsafe.Add(ref hpCache, 256);
        ref char opCache = ref Unsafe.Add(ref hpCache, 512);
        Unsafe.As<char, RGBHeader>(ref buffer) = _foregroundHeader;
        Unchecked.Write(ref buffer, 7, Unchecked.Read(ref hpCache, r));
        Unchecked.Write(ref buffer, 8, Unchecked.Read(ref tpCache, r));
        Unchecked.Write(ref buffer, 9, Unchecked.Read(ref opCache, r));
        Unchecked.Write(ref buffer, 11, Unchecked.Read(ref hpCache, g));
        Unchecked.Write(ref buffer, 12, Unchecked.Read(ref tpCache, g));
        Unchecked.Write(ref buffer, 13, Unchecked.Read(ref opCache, g));
        Unchecked.Write(ref buffer, 15, Unchecked.Read(ref hpCache, b));
        Unchecked.Write(ref buffer, 16, Unchecked.Read(ref tpCache, b));
        Unchecked.Write(ref buffer, 17, Unchecked.Read(ref opCache, b));
        Unchecked.Write(ref buffer, 18, 'm');
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Foreground(int r, int g, int b)
    {
        string result = Unchecked.FastAllocateString(19);
        FillForegroundRGB(ref result.GetReference(), (byte)r, (byte)g, (byte)b);
        return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void FillBackgroundRGB(ref char buffer, byte r, byte g, byte b)
    {
        ref char hpCache = ref _pCache.GetReference();
        ref char tpCache = ref Unsafe.Add(ref hpCache, 256);
        ref char opCache = ref Unsafe.Add(ref hpCache, 512);
        Unsafe.As<char, RGBHeader>(ref buffer) = _backgroundHeader;
        Unchecked.Write(ref buffer, 7, Unchecked.Read(ref hpCache, r));
        Unchecked.Write(ref buffer, 8, Unchecked.Read(ref tpCache, r));
        Unchecked.Write(ref buffer, 9, Unchecked.Read(ref opCache, r));
        Unchecked.Write(ref buffer, 11, Unchecked.Read(ref hpCache, g));
        Unchecked.Write(ref buffer, 12, Unchecked.Read(ref tpCache, g));
        Unchecked.Write(ref buffer, 13, Unchecked.Read(ref opCache, g));
        Unchecked.Write(ref buffer, 15, Unchecked.Read(ref hpCache, b));
        Unchecked.Write(ref buffer, 16, Unchecked.Read(ref tpCache, b));
        Unchecked.Write(ref buffer, 17, Unchecked.Read(ref opCache, b));
        Unchecked.Write(ref buffer, 18, 'm');
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Background(int r, int g, int b)
    {
        string result = Unchecked.FastAllocateString(19);
        FillBackgroundRGB(ref result.GetReference(), (byte)r, (byte)g, (byte)b);
        return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Foreground(Xterm256 color) => _fg256Cache[(int)color];
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Background(Xterm256 color) => _bg256Cache[(int)color];
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Foreground(Xterm16 color) => _fg16Cache[(int)color];
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Background(Xterm16 color) => _bg16Cache[(int)color];

    #region string extensions
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToSGR(this string input) => C0_CSI + input + SGR;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToBold(this string text) => Bold + text + NoBold;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToDim(this string text) => Dim + text + NoDim;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToItalic(this string text) => Italic + text + NoItalic;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToUnderline(this string text) => Underline + text + NoUnderline;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToInverse(this string text) => Inverse + text + NoInverse;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToHidden(this string text) => Hidden + text + NoHidden;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToStrikethrough(this string text) => Strikethrough + text + NoStrikethrough;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToColor(this string text, int r, int g, int b) => Foreground(r, g, b) + text + DefaultForeground;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToColor(this string text, Xterm256 color) => Foreground(color) + text + DefaultForeground;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToColor(this string text, Xterm16 color) => Foreground(color) + text + DefaultForeground;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToBackgroundColor(this string text, int r, int g, int b) => Background(r, g, b) + text + DefaultBackground;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToBackgroundColor(this string text, Xterm256 color) => Background(color) + text + DefaultBackground;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToBackgroundColor(this string text, Xterm16 color) => Background(color) + text + DefaultBackground;
    #endregion
    #region StringBuilder extensions
    public static unsafe StringBuilder AppendForeground(this StringBuilder sb, int r, int g, int b)
    {
        Unchecked.Ptr<char> buffer = stackalloc char[19];
        FillForegroundRGB(ref buffer.GetReference(), (byte)r, (byte)g, (byte)b);
        return sb.Append(buffer, 19);
    }
    public static unsafe StringBuilder AppendBackground(this StringBuilder sb, int r, int g, int b)
    {
        Unchecked.Ptr<char> buffer = stackalloc char[19];
        FillBackgroundRGB(ref buffer.GetReference(), (byte)r, (byte)g, (byte)b);
        return sb.Append(buffer, 19);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendForeground(this StringBuilder sb, Xterm256 color) => sb.Append(Foreground(color));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendBackground(this StringBuilder sb, Xterm256 color) => sb.Append(Background(color));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendForeground(this StringBuilder sb, Xterm16 color) => sb.Append(Foreground(color));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendBackground(this StringBuilder sb, Xterm16 color) => sb.Append(Background(color));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendSGR(this StringBuilder sb, string input)
    {
        sb.EnsureCapacity(sb.Length + C0_CSI.Length + input.Length + 1);
        return sb.Append(C0_CSI).Append(input).Append('m');
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendBold(this StringBuilder sb, string text)
    {
        sb.EnsureCapacity(sb.Length + Bold.Length + text.Length + NoBold.Length);
        return sb.Append(Bold).Append(text).Append(NoBold);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendDim(this StringBuilder sb, string text)
    {
        sb.EnsureCapacity(sb.Length + Dim.Length + text.Length + NoDim.Length);
        return sb.Append(Dim).Append(text).Append(NoDim);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendItalic(this StringBuilder sb, string text)
    {
        sb.EnsureCapacity(sb.Length + Italic.Length + text.Length + NoItalic.Length);
        return sb.Append(Italic).Append(text).Append(NoItalic);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendUnderline(this StringBuilder sb, string text)
    {
        sb.EnsureCapacity(sb.Length + Underline.Length + text.Length + NoUnderline.Length);
        return sb.Append(Underline).Append(text).Append(NoUnderline);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendInverse(this StringBuilder sb, string text)
    {
        sb.EnsureCapacity(sb.Length + Inverse.Length + text.Length + NoInverse.Length);
        return sb.Append(Inverse).Append(text).Append(NoInverse);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendHidden(this StringBuilder sb, string text)
    {
        sb.EnsureCapacity(sb.Length + Hidden.Length + text.Length + NoHidden.Length);
        return sb.Append(Hidden).Append(text).Append(NoHidden);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendStrikethrough(this StringBuilder sb, string text)
    {
        sb.EnsureCapacity(sb.Length + Strikethrough.Length + text.Length + NoStrikethrough.Length);
        return sb.Append(Strikethrough).Append(text).Append(NoStrikethrough);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendColor(this StringBuilder sb, string text, int r, int g, int b)
    {
        sb.EnsureCapacity(sb.Length + 19 + text.Length + DefaultForeground.Length);
        return sb.AppendForeground(r, g, b).Append(text).Append(DefaultForeground);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendColor(this StringBuilder sb, string text, Xterm256 color)
    {
        sb.EnsureCapacity(sb.Length + 11 + text.Length + DefaultForeground.Length);
        return sb.AppendForeground(color).Append(text).Append(DefaultForeground);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendColor(this StringBuilder sb, string text, Xterm16 color)
    {
        sb.EnsureCapacity(sb.Length + 6 + text.Length + DefaultForeground.Length);
        return sb.AppendForeground(color).Append(text).Append(DefaultForeground);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendBackgroundColor(this StringBuilder sb, string text, int r, int g, int b)
    {
        sb.EnsureCapacity(sb.Length + 19 + text.Length + DefaultBackground.Length);
        return sb.AppendBackground(r, g, b).Append(text).Append(DefaultBackground);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendBackgroundColor(this StringBuilder sb, string text, Xterm256 color)
    {
        sb.EnsureCapacity(sb.Length + 11 + text.Length + DefaultBackground.Length);
        return sb.AppendBackground(color).Append(text).Append(DefaultBackground);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder AppendBackgroundColor(this StringBuilder sb, string text, Xterm16 color)
    {
        sb.EnsureCapacity(sb.Length + 6 + text.Length + DefaultBackground.Length);
        return sb.AppendBackground(color).Append(text).Append(DefaultBackground);
    }
    #endregion
    #region ValueStringBuilder extensions
    public static void AppendForeground(ref this ValueStringBuilder vsb, int r, int g, int b)
    {
        FillForegroundRGB(ref vsb.AppendRef(19), (byte)r, (byte)g, (byte)b);
    }
    public static void AppendBackground(ref this ValueStringBuilder vsb, int r, int g, int b)
    {
        FillBackgroundRGB(ref vsb.AppendRef(19), (byte)r, (byte)g, (byte)b);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendForeground(ref this ValueStringBuilder vsb, Xterm256 color) => vsb.Append(Foreground(color));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendBackground(ref this ValueStringBuilder vsb, Xterm256 color) => vsb.Append(Background(color));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendForeground(ref this ValueStringBuilder vsb, Xterm16 color) => vsb.Append(Foreground(color));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendBackground(ref this ValueStringBuilder vsb, Xterm16 color) => vsb.Append(Background(color));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendSGR(ref this ValueStringBuilder vsb, string input)
    {
        vsb.EnsureCapacity(vsb.Length + C0_CSI.Length + input.Length + 1);
        vsb.Append(C0_CSI);
        vsb.Append(input);
        vsb.Append('m');
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendBold(ref this ValueStringBuilder vsb, string text)
    {
        vsb.EnsureCapacity(vsb.Length + Bold.Length + text.Length + NoBold.Length);
        vsb.Append(Bold);
        vsb.Append(text);
        vsb.Append(NoBold);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendDim(ref this ValueStringBuilder vsb, string text)
    {
        vsb.EnsureCapacity(vsb.Length + Dim.Length + text.Length + NoDim.Length);
        vsb.Append(Dim);
        vsb.Append(text);
        vsb.Append(NoDim);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendItalic(ref this ValueStringBuilder vsb, string text)
    {
        vsb.EnsureCapacity(vsb.Length + Italic.Length + text.Length + NoItalic.Length);
        vsb.Append(Italic);
        vsb.Append(text);
        vsb.Append(NoItalic);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendUnderline(ref this ValueStringBuilder vsb, string text)
    {
        vsb.EnsureCapacity(vsb.Length + Underline.Length + text.Length + NoUnderline.Length);
        vsb.Append(Underline);
        vsb.Append(text);
        vsb.Append(NoUnderline);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendInverse(ref this ValueStringBuilder vsb, string text)
    {
        vsb.EnsureCapacity(vsb.Length + Inverse.Length + text.Length + NoInverse.Length);
        vsb.Append(Inverse);
        vsb.Append(text);
        vsb.Append(NoInverse);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendHidden(ref this ValueStringBuilder vsb, string text)
    {
        vsb.EnsureCapacity(vsb.Length + Hidden.Length + text.Length + NoHidden.Length);
        vsb.Append(Hidden);
        vsb.Append(text);
        vsb.Append(NoHidden);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendStrikethrough(ref this ValueStringBuilder vsb, string text)
    {
        vsb.EnsureCapacity(vsb.Length + Strikethrough.Length + text.Length + NoStrikethrough.Length);
        vsb.Append(Strikethrough);
        vsb.Append(text);
        vsb.Append(NoStrikethrough);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendColor(ref this ValueStringBuilder vsb, string text, int r, int g, int b)
    {
        vsb.EnsureCapacity(vsb.Length + 19 + text.Length + DefaultForeground.Length);
        vsb.AppendForeground(r, g, b);
        vsb.Append(text);
        vsb.Append(DefaultForeground);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendColor(ref this ValueStringBuilder vsb, string text, Xterm256 color)
    {
        vsb.EnsureCapacity(vsb.Length + 11 + text.Length + DefaultForeground.Length);
        vsb.AppendForeground(color);
        vsb.Append(text);
        vsb.Append(DefaultForeground);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendColor(ref this ValueStringBuilder vsb, string text, Xterm16 color)
    {
        vsb.EnsureCapacity(vsb.Length + 6 + text.Length + DefaultForeground.Length);
        vsb.AppendForeground(color);
        vsb.Append(text);
        vsb.Append(DefaultForeground);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendBackgroundColor(ref this ValueStringBuilder vsb, string text, int r, int g, int b)
    {
        vsb.EnsureCapacity(vsb.Length + 19 + text.Length + DefaultBackground.Length);
        vsb.AppendBackground(r, g, b);
        vsb.Append(text);
        vsb.Append(DefaultBackground);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendBackgroundColor(ref this ValueStringBuilder vsb, string text, Xterm256 color)
    {
        vsb.EnsureCapacity(vsb.Length + 11 + text.Length + DefaultBackground.Length);
        vsb.AppendBackground(color);
        vsb.Append(text);
        vsb.Append(DefaultBackground);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AppendBackgroundColor(ref this ValueStringBuilder vsb, string text, Xterm16 color)
    {
        vsb.EnsureCapacity(vsb.Length + 6 + text.Length + DefaultBackground.Length);
        vsb.AppendBackground(color);
        vsb.Append(text);
        vsb.Append(DefaultBackground);
    }
    #endregion
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char WriteForeground(ref char buffer, int r, int g, int b)
    {
        FillForegroundRGB(ref buffer, (byte)r, (byte)g, (byte)b);
        return ref Unsafe.Add(ref buffer, 19);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char WriteBackground(ref char buffer, int r, int g, int b)
    {
        FillBackgroundRGB(ref buffer, (byte)r, (byte)g, (byte)b);
        return ref Unsafe.Add(ref buffer, 19);
    }
}