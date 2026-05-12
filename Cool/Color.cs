namespace Cool;

public static class Color
{
    public static readonly string Default = Ansi.DefaultForeground;
    public static readonly string Red = Ansi.Foreground(Xterm256.Red1);
    public static readonly string Orange = Ansi.Foreground(Xterm256.Orange1);
    public static readonly string Yellow = Ansi.Foreground(Xterm256.OrangeYellow);
    public static readonly string Green = Ansi.Foreground(Xterm256.Green7);
    public static readonly string Cyan = Ansi.Foreground(Xterm256.PaleGreenBlue4);
    public static readonly string Blue = Ansi.Foreground(Xterm256.GreenBlue5);
    public static readonly string Purple = Ansi.Foreground(Xterm256.PaleBluePurple2);
    public static readonly string Magenta = Ansi.Foreground(Xterm256.Purple4);
    public static readonly string Turquoise = Ansi.Foreground(Xterm256.DarkGreenBlue2);
    public static readonly string Black = Ansi.Foreground(Xterm256.Black);
    public static readonly string Maroon = Ansi.Foreground(Xterm256.Maroon);
    public static readonly string Olive = Ansi.Foreground(Xterm256.Olive);
    public static readonly string Navy = Ansi.Foreground(Xterm256.Navy);
    public static readonly string Teal = Ansi.Foreground(Xterm256.Teal);
    public static readonly string Silver = Ansi.Foreground(Xterm256.Silver);
    public static readonly string Gray = Ansi.Foreground(Xterm256.Gray);
    public static readonly string Lime = Ansi.Foreground(Xterm256.Lime);
    public static readonly string Fuchsia = Ansi.Foreground(Xterm256.Fuchsia);
    public static readonly string Aqua = Ansi.Foreground(Xterm256.Aqua);
    public static readonly string White = Ansi.Foreground(Xterm256.White);
}