using System.Text;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class AnsiTests
    {
        [Fact]
        public void EscapeSGR_Returns_EscInputM()
        {
            Assert.Equal("\u001b[31m", AnsiSGR.Escape("31"));

            var sb = new StringBuilder();
            sb.Escape("31");
            Assert.Equal("\u001b[31m", sb.ToString());
        }

        [Fact]
        public void ForegroundRgb_Works()
        {
            Assert.Equal("\u001b[38;2;001;002;003m", Ansi.Foreground(1, 2, 3));
            var sb = new StringBuilder();
            sb.Foreground(1, 2, 3);
            Assert.Equal("\u001b[38;2;001;002;003m", sb.ToString());
        }

        [Fact]
        public void BackgroundRgb_Works()
        {
            Assert.Equal("\u001b[48;2;010;020;030m", Ansi.Background(10, 20, 30));
            var sb = new StringBuilder();
            sb.Background(10, 20, 30);
            Assert.Equal("\u001b[48;2;010;020;030m", sb.ToString());
        }

        [Fact]
        public void ForegroundXterm256_And_Background_Returns_Correct_Codes()
        {
            Assert.Equal("\u001b[38;5;9m", Ansi.Foreground(Xterm256.Red));
            Assert.Equal("\u001b[48;5;9m", Ansi.Background(Xterm256.Red));
        }

        [Fact]
        public void ForegroundXterm16_And_Background_Returns_Correct_Codes()
        {
            Assert.Equal("\u001b[31m", Ansi.Foreground(Xterm16.Red));
            Assert.Equal("\u001b[41m", Ansi.Background(Xterm16.Red));
        }

        [Fact]
        public void Bold_Wraps_Text()
        {
            Assert.Equal(Ansi.Bold + "hi" + Ansi.Normal, "hi".Bold());
            var sb = new StringBuilder();
            sb.Bold("hi");
            Assert.Equal(Ansi.Bold + "hi" + Ansi.Normal, sb.ToString());
        }

        [Fact]
        public void Color_Returns_Foreground_Wrap_And_Default()
        {
            string text = "hello";
            var expected = Ansi.Foreground(Xterm256.Red) + text + Ansi.DefaultForeground;
            Assert.Equal(expected, AnsiSGR.Color(text, Xterm256.Red));
            var sb = new StringBuilder();
            sb.Color(text, Xterm256.Red);
            Assert.Equal(expected, sb.ToString());
        }

        [Fact]
        public void All_Sgr_Wrappers_Are_Correct()
        {
            Assert.Equal(Ansi.Dim + "x" + Ansi.Normal, AnsiSGR.Dim("x"));
            var sb = new StringBuilder();
            sb.Dim("x");
            Assert.Equal(Ansi.Dim + "x" + Ansi.NoDim, sb.ToString());

            Assert.Equal(Ansi.Italic + "y" + Ansi.NoItalic, AnsiSGR.Italic("y"));
            sb.Clear(); sb.Italic("y");
            Assert.Equal(Ansi.Italic + "y" + Ansi.NoItalic, sb.ToString());

            Assert.Equal(Ansi.Underline + "u" + Ansi.NoUnderline, AnsiSGR.Underline("u"));
            sb.Clear(); sb.Underline("u");
            Assert.Equal(Ansi.Underline + "u" + Ansi.NoUnderline, sb.ToString());

            Assert.Equal(Ansi.Inverse + "i" + Ansi.NoInverse, AnsiSGR.Inverse("i"));
            sb.Clear(); sb.Inverse("i");
            Assert.Equal(Ansi.Inverse + "i" + Ansi.NoInverse, sb.ToString());

            Assert.Equal(Ansi.Hidden + "h" + Ansi.NoHidden, AnsiSGR.Hidden("h"));
            sb.Clear(); sb.Hidden("h");
            Assert.Equal(Ansi.Hidden + "h" + Ansi.NoHidden, sb.ToString());

            Assert.Equal(Ansi.Strikethrough + "s" + Ansi.NoStrikethrough, AnsiSGR.Strikethrough("s"));
            sb.Clear(); sb.Strikethrough("s");
            Assert.Equal(Ansi.Strikethrough + "s" + Ansi.NoStrikethrough, sb.ToString());
        }

        [Fact]
        public void Rgb_OutOfRange_And_Negative_Are_Masked_To_Byte()
        {
            Assert.Equal("\u001b[38;2;000;001;002m", Ansi.Foreground(256, 257, 258));
            Assert.Equal("\u001b[38;2;255;254;253m", Ansi.Foreground(-1, -2, -3));

            var sb = new StringBuilder();
            var ret = sb.Foreground(256, 257, 258);
            Assert.Same(sb, ret);
            Assert.Equal("\u001b[38;2;000;001;002m", sb.ToString());
        }

        [Fact]
        public void ExtensionMethods_Return_Same_StringBuilder()
        {
            var sb = new StringBuilder();
            Assert.Same(sb, sb.Foreground(1, 2, 3));
            Assert.Same(sb, sb.Background(1, 2, 3));
            Assert.Same(sb, sb.Foreground(Xterm256.Red));
            Assert.Same(sb, sb.Background(Xterm16.Red));
            Assert.Same(sb, sb.Bold("z"));
        }

        [Fact]
        public void BackgroundColor_Int_And_Xterm_Wraps_Correctly()
        {
            Assert.Equal(Ansi.Background(1, 2, 3) + "t" + Ansi.DefaultBackground, AnsiSGR.BackgroundColor("t", 1, 2, 3));
            var sb = new StringBuilder(); sb.BackgroundColor("t", 1, 2, 3);
            Assert.Equal(Ansi.Background(1, 2, 3) + "t" + Ansi.DefaultBackground, sb.ToString());

            Assert.Equal(Ansi.Background(Xterm256.Red) + "t" + Ansi.DefaultBackground, AnsiSGR.BackgroundColor("t", Xterm256.Red));
            Assert.Equal(Ansi.Background(Xterm16.Red) + "t" + Ansi.DefaultBackground, AnsiSGR.BackgroundColor("t", Xterm16.Red));
        }

        [Fact]
        public void Color_Rgb_And_Xterm16_Wraps()
        {
            Assert.Equal(Ansi.Foreground(1, 2, 3) + "hi" + Ansi.DefaultForeground, AnsiSGR.Color("hi", 1, 2, 3));
            Assert.Equal(Ansi.Foreground(Xterm16.Red) + "x" + Ansi.DefaultForeground, AnsiSGR.Color("x", Xterm16.Red));
        }

        [Fact]
#pragma warning disable CS8625
        public void EscapeSGR_Null_And_Empty_Work()
        {
            Assert.Equal("\u001b[m", AnsiSGR.Escape(null));
            Assert.Equal("\u001b[m", AnsiSGR.Escape(string.Empty));
            var sb = new StringBuilder(); sb.Escape(null);
            Assert.Equal("\u001b[m", sb.ToString());
        }
#pragma warning restore CS8625

        [Fact]
        public void Invalid_Enum_Indexes_Throw()
        {
            Assert.Throws<System.IndexOutOfRangeException>(() => Ansi.Foreground((Xterm256)300));
            Assert.Throws<System.IndexOutOfRangeException>(() => Ansi.Background((Xterm256)300));
            Assert.Throws<System.IndexOutOfRangeException>(() => Ansi.Foreground((Xterm16)16));
            Assert.Throws<System.IndexOutOfRangeException>(() => Ansi.Background((Xterm16)16));
        }
    }
}
