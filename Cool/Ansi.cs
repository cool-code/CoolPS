using System;
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

    private static unsafe readonly byte* _hpCache = InitNativeCache((i) => (byte)('0' + (i / 100 % 10)));
    private static unsafe readonly byte* _tpCache = InitNativeCache((i) => (byte)('0' + (i / 10 % 10)));
    private static unsafe readonly byte* _opCache = InitNativeCache((i) => (byte)('0' + (i % 10)));

    private static unsafe byte* InitNativeCache(Func<int, byte> f)
    {
        IntPtr nativeMem = Marshal.AllocHGlobal(256);
        byte* cache = (byte*)nativeMem.ToPointer();

        AppDomain.CurrentDomain.DomainUnload += (s, e) =>
        {
            if (nativeMem != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeMem);
                nativeMem = IntPtr.Zero;
            }
        };

        for (int i = 0; i < 256; i++) cache[i] = f(i);

        return cache;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void WriteInt(ref char buffer, int pos, int value)
    {
        byte b = (byte)(value & 0xFF);
        NoBoundCheck.Write(ref buffer, pos++, (char)_hpCache[b]);
        NoBoundCheck.Write(ref buffer, pos++, (char)_tpCache[b]);
        NoBoundCheck.Write(ref buffer, pos++, (char)_opCache[b]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void FillForegroundRGB(ref char buffer, int r, int g, int b)
    {
        NoBoundCheck.Write(ref buffer, 0, '\x1b');
        NoBoundCheck.Write(ref buffer, 1, '[');
        NoBoundCheck.Write(ref buffer, 2, '3');
        NoBoundCheck.Write(ref buffer, 3, '8');
        NoBoundCheck.Write(ref buffer, 4, ';');
        NoBoundCheck.Write(ref buffer, 5, '2');
        NoBoundCheck.Write(ref buffer, 6, ';');
        WriteInt(ref buffer, 7, r);
        NoBoundCheck.Write(ref buffer, 10, ';');
        WriteInt(ref buffer, 11, g);
        NoBoundCheck.Write(ref buffer, 14, ';');
        WriteInt(ref buffer, 15, b);
        NoBoundCheck.Write(ref buffer, 18, 'm');
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Foreground(int r, int g, int b)
    {
        string result = new('\0', 19);
        FillForegroundRGB(ref NoBoundCheck.GetReference(result), r, g, b);
        return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void FillBackgroundRGB(ref char buffer, int r, int g, int b)
    {
        NoBoundCheck.Write(ref buffer, 0, '\x1b');
        NoBoundCheck.Write(ref buffer, 1, '[');
        NoBoundCheck.Write(ref buffer, 2, '4');
        NoBoundCheck.Write(ref buffer, 3, '8');
        NoBoundCheck.Write(ref buffer, 4, ';');
        NoBoundCheck.Write(ref buffer, 5, '2');
        NoBoundCheck.Write(ref buffer, 6, ';');
        WriteInt(ref buffer, 7, r);
        NoBoundCheck.Write(ref buffer, 10, ';');
        WriteInt(ref buffer, 11, g);
        NoBoundCheck.Write(ref buffer, 14, ';');
        WriteInt(ref buffer, 15, b);
        NoBoundCheck.Write(ref buffer, 18, 'm');
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Background(int r, int g, int b)
    {
        string result = new('\0', 19);
        FillBackgroundRGB(ref NoBoundCheck.GetReference(result), r, g, b);
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
}
public static class AnsiSGR
{
    #region string extensions
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Escape(this string input) => Ansi.C0_CSI + input + Ansi.SGR;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Bold(this string text) => Ansi.Bold + text + Ansi.NoBold;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Dim(this string text) => Ansi.Dim + text + Ansi.NoDim;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Italic(this string text) => Ansi.Italic + text + Ansi.NoItalic;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Underline(this string text) => Ansi.Underline + text + Ansi.NoUnderline;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Inverse(this string text) => Ansi.Inverse + text + Ansi.NoInverse;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Hidden(this string text) => Ansi.Hidden + text + Ansi.NoHidden;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Strikethrough(this string text) => Ansi.Strikethrough + text + Ansi.NoStrikethrough;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Color(this string text, int r, int g, int b) => Ansi.Foreground(r, g, b) + text + Ansi.DefaultForeground;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Color(this string text, Xterm256 color) => Ansi.Foreground(color) + text + Ansi.DefaultForeground;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Color(this string text, Xterm16 color) => Ansi.Foreground(color) + text + Ansi.DefaultForeground;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string BackgroundColor(this string text, int r, int g, int b) => Ansi.Background(r, g, b) + text + Ansi.DefaultBackground;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string BackgroundColor(this string text, Xterm256 color) => Ansi.Background(color) + text + Ansi.DefaultBackground;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string BackgroundColor(this string text, Xterm16 color) => Ansi.Background(color) + text + Ansi.DefaultBackground;
    #endregion
    #region StringBuilder extensions
    public static unsafe StringBuilder Foreground(this StringBuilder sb, int r, int g, int b)
    {
        char* buffer = stackalloc char[19];
        Ansi.FillForegroundRGB(ref NoBoundCheck.GetReference(buffer), r, g, b);
        return sb.Append(buffer, 19);
    }
    public static unsafe StringBuilder Background(this StringBuilder sb, int r, int g, int b)
    {
        char* buffer = stackalloc char[19];
        Ansi.FillBackgroundRGB(ref NoBoundCheck.GetReference(buffer), r, g, b);
        return sb.Append(buffer, 19);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Foreground(this StringBuilder sb, Xterm256 color) => sb.Append(Ansi.Foreground(color));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Background(this StringBuilder sb, Xterm256 color) => sb.Append(Ansi.Background(color));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Foreground(this StringBuilder sb, Xterm16 color) => sb.Append(Ansi.Foreground(color));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Background(this StringBuilder sb, Xterm16 color) => sb.Append(Ansi.Background(color));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Escape(this StringBuilder sb, string input) => sb.Append(Ansi.C0_CSI).Append(input).Append('m');
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Bold(this StringBuilder sb, string text) => sb.Append(Ansi.Bold).Append(text).Append(Ansi.NoBold);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Dim(this StringBuilder sb, string text) => sb.Append(Ansi.Dim).Append(text).Append(Ansi.NoDim);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Italic(this StringBuilder sb, string text) => sb.Append(Ansi.Italic).Append(text).Append(Ansi.NoItalic);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Underline(this StringBuilder sb, string text) => sb.Append(Ansi.Underline).Append(text).Append(Ansi.NoUnderline);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Inverse(this StringBuilder sb, string text) => sb.Append(Ansi.Inverse).Append(text).Append(Ansi.NoInverse);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Hidden(this StringBuilder sb, string text) => sb.Append(Ansi.Hidden).Append(text).Append(Ansi.NoHidden);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Strikethrough(this StringBuilder sb, string text) => sb.Append(Ansi.Strikethrough).Append(text).Append(Ansi.NoStrikethrough);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Color(this StringBuilder sb, string text, int r, int g, int b) => sb.Foreground(r, g, b).Append(text).Append(Ansi.DefaultForeground);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Color(this StringBuilder sb, string text, Xterm256 color) => sb.Foreground(color).Append(text).Append(Ansi.DefaultForeground);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Color(this StringBuilder sb, string text, Xterm16 color) => sb.Foreground(color).Append(text).Append(Ansi.DefaultForeground);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder BackgroundColor(this StringBuilder sb, string text, int r, int g, int b) => sb.Background(r, g, b).Append(text).Append(Ansi.DefaultBackground);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder BackgroundColor(this StringBuilder sb, string text, Xterm256 color) => sb.Background(color).Append(text).Append(Ansi.DefaultBackground);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder BackgroundColor(this StringBuilder sb, string text, Xterm16 color) => sb.Background(color).Append(text).Append(Ansi.DefaultBackground);
    #endregion
}