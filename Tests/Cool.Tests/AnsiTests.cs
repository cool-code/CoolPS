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
            Assert.Equal("\u001b[31m", Ansi.EscapeSGR("31"));

            var sb = new StringBuilder();
            sb.EscapeSGR("31");
            Assert.Equal("\u001b[31m", sb.ToString());
        }

        [Fact]
        public void ForegroundRgb_Works()
        {
            Assert.Equal("\u001b[38;2;1;2;3m", Ansi.Foreground(1, 2, 3));
            var sb = new StringBuilder();
            sb.Foreground(1, 2, 3);
            Assert.Equal("\u001b[38;2;1;2;3m", sb.ToString());
        }

        [Fact]
        public void BackgroundRgb_Works()
        {
            Assert.Equal("\u001b[48;2;10;20;30m", Ansi.Background(10, 20, 30));
            var sb = new StringBuilder();
            sb.Background(10, 20, 30);
            Assert.Equal("\u001b[48;2;10;20;30m", sb.ToString());
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
            Assert.Equal(Ansi.SGR.Bold + "hi" + Ansi.SGR.Normal, Ansi.Bold("hi"));
            var sb = new StringBuilder();
            sb.Bold("hi");
            Assert.Equal(Ansi.SGR.Bold + "hi" + Ansi.SGR.Normal, sb.ToString());
        }

        [Fact]
        public void Color_Returns_Foreground_Wrap_And_Default()
        {
            string text = "hello";
            var expected = Ansi.Foreground(Xterm256.Red) + text + Ansi.SGR.DefaultForegroundColor;
            Assert.Equal(expected, Ansi.Color(text, Xterm256.Red));
            var sb = new StringBuilder();
            sb.Color(text, Xterm256.Red);
            Assert.Equal(expected, sb.ToString());
        }

        [Fact]
        public void All_Sgr_Wrappers_Are_Correct()
        {
            Assert.Equal(Ansi.SGR.Dim + "x" + Ansi.SGR.Normal, Ansi.Dim("x"));
            var sb = new StringBuilder();
            sb.Dim("x");
            Assert.Equal(Ansi.SGR.Dim + "x" + Ansi.SGR.Normal, sb.ToString());

            Assert.Equal(Ansi.SGR.Italic + "y" + Ansi.SGR.NoItalic, Ansi.Italic("y"));
            sb.Clear(); sb.Italic("y");
            Assert.Equal(Ansi.SGR.Italic + "y" + Ansi.SGR.NoItalic, sb.ToString());

            Assert.Equal(Ansi.SGR.Underline + "u" + Ansi.SGR.NoUnderline, Ansi.Underline("u"));
            sb.Clear(); sb.Underline("u");
            Assert.Equal(Ansi.SGR.Underline + "u" + Ansi.SGR.NoUnderline, sb.ToString());

            Assert.Equal(Ansi.SGR.Inverse + "i" + Ansi.SGR.NoInverse, Ansi.Inverse("i"));
            sb.Clear(); sb.Inverse("i");
            Assert.Equal(Ansi.SGR.Inverse + "i" + Ansi.SGR.NoInverse, sb.ToString());

            Assert.Equal(Ansi.SGR.Hidden + "h" + Ansi.SGR.NoHidden, Ansi.Hidden("h"));
            sb.Clear(); sb.Hidden("h");
            Assert.Equal(Ansi.SGR.Hidden + "h" + Ansi.SGR.NoHidden, sb.ToString());

            Assert.Equal(Ansi.SGR.Strikethrough + "s" + Ansi.SGR.NoStrikethrough, Ansi.Strikethrough("s"));
            sb.Clear(); sb.Strikethrough("s");
            Assert.Equal(Ansi.SGR.Strikethrough + "s" + Ansi.SGR.NoStrikethrough, sb.ToString());
        }

        [Fact]
        public void Rgb_OutOfRange_And_Negative_Are_Masked_To_Byte()
        {
            Assert.Equal("\u001b[38;2;0;1;2m", Ansi.Foreground(256, 257, 258));
            Assert.Equal("\u001b[38;2;255;254;253m", Ansi.Foreground(-1, -2, -3));

            var sb = new StringBuilder();
            var ret = sb.Foreground(256, 257, 258);
            Assert.Same(sb, ret);
            Assert.Equal("\u001b[38;2;0;1;2m", sb.ToString());
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
            Assert.Equal(Ansi.Background(1, 2, 3) + "t" + Ansi.SGR.DefaultBackgroundColor, Ansi.BackgroundColor("t", 1, 2, 3));
            var sb = new StringBuilder(); sb.BackgroundColor("t", 1, 2, 3);
            Assert.Equal(Ansi.Background(1, 2, 3) + "t" + Ansi.SGR.DefaultBackgroundColor, sb.ToString());

            Assert.Equal(Ansi.Background(Xterm256.Red) + "t" + Ansi.SGR.DefaultBackgroundColor, Ansi.BackgroundColor("t", Xterm256.Red));
            Assert.Equal(Ansi.Background(Xterm16.Red) + "t" + Ansi.SGR.DefaultBackgroundColor, Ansi.BackgroundColor("t", Xterm16.Red));
        }

        [Fact]
        public void Color_Rgb_And_Xterm16_Wraps()
        {
            Assert.Equal(Ansi.Foreground(1, 2, 3) + "hi" + Ansi.SGR.DefaultForegroundColor, Ansi.Color("hi", 1, 2, 3));
            Assert.Equal(Ansi.Foreground(Xterm16.Red) + "x" + Ansi.SGR.DefaultForegroundColor, Ansi.Color("x", Xterm16.Red));
        }

        [Fact]
#pragma warning disable CS8625
        public void EscapeSGR_Null_And_Empty_Work()
        {
            Assert.Equal("\u001b[m", Ansi.EscapeSGR(null));
            Assert.Equal("\u001b[m", Ansi.EscapeSGR(string.Empty));
            var sb = new StringBuilder(); sb.EscapeSGR(null);
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
