using System.Runtime.CompilerServices;
using System.Text;

namespace Cool;

public static class Ansi
{
    public const string Esc = "\u001b[";
    public static class SGR
    {
        public const string Reset = Esc + "0m";
        public const string Bold = Esc + "1m";
        public const string Dim = Esc + "2m";
        public const string Italic = Esc + "3m";
        public const string Underline = Esc + "4m";
        public const string Inverse = Esc + "7m";
        public const string Hidden = Esc + "8m";
        public const string Strikethrough = Esc + "9m";
        public const string Normal = Esc + "22m";
        public const string NoItalic = Esc + "23m";
        public const string NoUnderline = Esc + "24m";
        public const string NoInverse = Esc + "27m";
        public const string NoHidden = Esc + "28m";
        public const string NoStrikethrough = Esc + "29m";
        public const string DefaultForegroundColor = Esc + "39m";
        public const string DefaultBackgroundColor = Esc + "49m";

    }
    private static readonly string[] _fg256Cache = Init256ColorCache(true);
    private static readonly string[] _bg256Cache = Init256ColorCache(false);
    private static readonly string[] _fg16Cache = Init16ColorCache(true);
    private static readonly string[] _bg16Cache = Init16ColorCache(false);

    private static string[] Init256ColorCache(bool isForeground)
    {
        string prefix = isForeground ? "38;5;" : "48;5;";
        string[] cache = new string[256];
        for (int i = 0; i < 256; i++) cache[i] = Esc + prefix + i + "m";
        return cache;
    }

    private static string[] Init16ColorCache(bool isForeground)
    {
        string[] cache = new string[16];
        int baseCode = isForeground ? 30 : 40;
        for (int i = 0; i < 16; i++)
        {
            // foreground color: i < 8 is standard color (30-37)，i >= 8 is bright color (90-97)
            // background color: i < 8 is standard color (40-47)，i >= 8 is bright color (100-107)
            int code = (i < 8) ? (baseCode + i) : (baseCode + 60 + (i - 8));
            cache[i] = Esc + code + "m";
        }
        return cache;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string EscapeSGR(string input) => Esc + input + "m";
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder EscapeSGR(this StringBuilder sb, string input) => sb.Append(Esc).Append(input).Append('m');
    private static readonly StringBuilderPool _sbPool = new();
    private static StringBuilder AppendFastInt(this StringBuilder sb, int value)
    {
        value &= 0xFF;
        if (value >= 100) sb.Append((char)('0' + (value / 100)));
        if (value >= 10) sb.Append((char)('0' + (value / 10 % 10)));
        return sb.Append((char)('0' + (value % 10)));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static StringBuilder AppendRGB(this StringBuilder sb, int r, int g, int b) => sb.AppendFastInt(r).Append(';').AppendFastInt(g).Append(';').AppendFastInt(b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Foreground(this StringBuilder sb, int r, int g, int b) => sb.Append(Esc).Append("38;2;").AppendRGB(r, g, b).Append('m');
    public static string Foreground(int r, int g, int b)
    {
        var sb = _sbPool.Rent(20);
        sb.Foreground(r, g, b);
        var result = sb.ToString();
        _sbPool.Return(sb);
        return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Background(this StringBuilder sb, int r, int g, int b) => sb.Append(Esc).Append("48;2;").AppendRGB(r, g, b).Append('m');
    public static string Background(int r, int g, int b)
    {
        var sb = _sbPool.Rent(20);
        sb.Background(r, g, b);
        var result = sb.ToString();
        _sbPool.Return(sb);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Foreground(Xterm256 color) => _fg256Cache[(int)color];
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Foreground(this StringBuilder sb, Xterm256 color) => sb.Append(_fg256Cache[(int)color]);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Background(Xterm256 color) => _bg256Cache[(int)color];
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Background(this StringBuilder sb, Xterm256 color) => sb.Append(_bg256Cache[(int)color]);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Foreground(Xterm16 color) => _fg16Cache[(int)color];
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Foreground(this StringBuilder sb, Xterm16 color) => sb.Append(_fg16Cache[(int)color]);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Background(Xterm16 color) => _bg16Cache[(int)color];
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Background(this StringBuilder sb, Xterm16 color) => sb.Append(_bg16Cache[(int)color]);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Bold(string text) => SGR.Bold + text + SGR.Normal;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Bold(this StringBuilder sb, string text) => sb.Append(SGR.Bold).Append(text).Append(SGR.Normal);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Dim(string text) => SGR.Dim + text + SGR.Normal;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Dim(this StringBuilder sb, string text) => sb.Append(SGR.Dim).Append(text).Append(SGR.Normal);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Italic(string text) => SGR.Italic + text + SGR.NoItalic;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Italic(this StringBuilder sb, string text) => sb.Append(SGR.Italic).Append(text).Append(SGR.NoItalic);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Underline(string text) => SGR.Underline + text + SGR.NoUnderline;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Underline(this StringBuilder sb, string text) => sb.Append(SGR.Underline).Append(text).Append(SGR.NoUnderline);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Inverse(string text) => SGR.Inverse + text + SGR.NoInverse;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Inverse(this StringBuilder sb, string text) => sb.Append(SGR.Inverse).Append(text).Append(SGR.NoInverse);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Hidden(string text) => SGR.Hidden + text + SGR.NoHidden;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Hidden(this StringBuilder sb, string text) => sb.Append(SGR.Hidden).Append(text).Append(SGR.NoHidden);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Strikethrough(string text) => SGR.Strikethrough + text + SGR.NoStrikethrough;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Strikethrough(this StringBuilder sb, string text) => sb.Append(SGR.Strikethrough).Append(text).Append(SGR.NoStrikethrough);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Color(string text, int r, int g, int b) => Foreground(r, g, b) + text + SGR.DefaultForegroundColor;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Color(this StringBuilder sb, string text, int r, int g, int b) => sb.Foreground(r, g, b).Append(text).Append(SGR.DefaultForegroundColor);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Color(string text, Xterm256 color) => Foreground(color) + text + SGR.DefaultForegroundColor;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Color(this StringBuilder sb, string text, Xterm256 color) => sb.Foreground(color).Append(text).Append(SGR.DefaultForegroundColor);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Color(string text, Xterm16 color) => Foreground(color) + text + SGR.DefaultForegroundColor;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder Color(this StringBuilder sb, string text, Xterm16 color) => sb.Foreground(color).Append(text).Append(SGR.DefaultForegroundColor);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string BackgroundColor(string text, int r, int g, int b) => Background(r, g, b) + text + SGR.DefaultBackgroundColor;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder BackgroundColor(this StringBuilder sb, string text, int r, int g, int b) => sb.Background(r, g, b).Append(text).Append(SGR.DefaultBackgroundColor);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string BackgroundColor(string text, Xterm256 color) => Background(color) + text + SGR.DefaultBackgroundColor;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder BackgroundColor(this StringBuilder sb, string text, Xterm256 color) => sb.Background(color).Append(text).Append(SGR.DefaultBackgroundColor);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string BackgroundColor(string text, Xterm16 color) => Background(color) + text + SGR.DefaultBackgroundColor;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringBuilder BackgroundColor(this StringBuilder sb, string text, Xterm16 color) => sb.Background(color).Append(text).Append(SGR.DefaultBackgroundColor);
}
