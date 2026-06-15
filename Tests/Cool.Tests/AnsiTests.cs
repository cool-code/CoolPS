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
            Assert.Equal("\u001b[31m", "31".ToSGR());

            var sb = new StringBuilder();
            sb.AppendSGR("31");
            Assert.Equal("\u001b[31m", sb.ToString());
        }

        [Fact]
        public void ForegroundRgb_Works()
        {
            Assert.Equal("\u001b[38;2;001;002;003m", Ansi.Foreground(1, 2, 3));
            var sb = new StringBuilder();
            sb.AppendForeground(1, 2, 3);
            Assert.Equal("\u001b[38;2;001;002;003m", sb.ToString());
        }

        [Fact]
        public void BackgroundRgb_Works()
        {
            Assert.Equal("\u001b[48;2;010;020;030m", Ansi.Background(10, 20, 30));
            var sb = new StringBuilder();
            sb.AppendBackground(10, 20, 30);
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
            Assert.Equal(Ansi.Bold + "hi" + Ansi.Normal, "hi".ToBold());
            var sb = new StringBuilder();
            sb.AppendBold("hi");
            Assert.Equal(Ansi.Bold + "hi" + Ansi.Normal, sb.ToString());
        }

        [Fact]
        public void Color_Returns_Foreground_Wrap_And_Default()
        {
            string text = "hello";
            var expected = Ansi.Foreground(Xterm256.Red) + text + Ansi.DefaultForeground;
            Assert.Equal(expected, text.ToColor(Xterm256.Red));
            var sb = new StringBuilder();
            sb.AppendColor(text, Xterm256.Red);
            Assert.Equal(expected, sb.ToString());
        }

        [Fact]
        public void All_Sgr_Wrappers_Are_Correct()
        {
            Assert.Equal(Ansi.Dim + "x" + Ansi.Normal, "x".ToDim());
            var sb = new StringBuilder();
            sb.AppendDim("x");
            Assert.Equal(Ansi.Dim + "x" + Ansi.NoDim, sb.ToString());

            Assert.Equal(Ansi.Italic + "y" + Ansi.NoItalic, "y".ToItalic());
            sb.Clear(); sb.AppendItalic("y");
            Assert.Equal(Ansi.Italic + "y" + Ansi.NoItalic, sb.ToString());

            Assert.Equal(Ansi.Underline + "u" + Ansi.NoUnderline, "u".ToUnderline());
            sb.Clear(); sb.AppendUnderline("u");
            Assert.Equal(Ansi.Underline + "u" + Ansi.NoUnderline, sb.ToString());

            Assert.Equal(Ansi.Inverse + "i" + Ansi.NoInverse, "i".ToInverse());
            sb.Clear(); sb.AppendInverse("i");
            Assert.Equal(Ansi.Inverse + "i" + Ansi.NoInverse, sb.ToString());

            Assert.Equal(Ansi.Hidden + "h" + Ansi.NoHidden, "h".ToHidden());
            sb.Clear(); sb.AppendHidden("h");
            Assert.Equal(Ansi.Hidden + "h" + Ansi.NoHidden, sb.ToString());

            Assert.Equal(Ansi.Strikethrough + "s" + Ansi.NoStrikethrough, "s".ToStrikethrough());
            sb.Clear(); sb.AppendStrikethrough("s");
            Assert.Equal(Ansi.Strikethrough + "s" + Ansi.NoStrikethrough, sb.ToString());
        }

        [Fact]
        public void Rgb_OutOfRange_And_Negative_Are_Masked_To_Byte()
        {
            Assert.Equal("\u001b[38;2;000;001;002m", Ansi.Foreground(256, 257, 258));
            Assert.Equal("\u001b[38;2;255;254;253m", Ansi.Foreground(-1, -2, -3));

            var sb = new StringBuilder();
            var ret = sb.AppendForeground(256, 257, 258);
            Assert.Same(sb, ret);
            Assert.Equal("\u001b[38;2;000;001;002m", sb.ToString());
        }

        [Fact]
        public void ExtensionMethods_Return_Same_StringBuilder()
        {
            var sb = new StringBuilder();
            Assert.Same(sb, sb.AppendForeground(1, 2, 3));
            Assert.Same(sb, sb.AppendBackground(1, 2, 3));
            Assert.Same(sb, sb.AppendForeground(Xterm256.Red));
            Assert.Same(sb, sb.AppendBackground(Xterm16.Red));
            Assert.Same(sb, sb.AppendBold("z"));
        }

        [Fact]
        public void BackgroundColor_Int_And_Xterm_Wraps_Correctly()
        {
            Assert.Equal(Ansi.Background(1, 2, 3) + "t" + Ansi.DefaultBackground, "t".ToBackgroundColor(1, 2, 3));
            var sb = new StringBuilder(); sb.AppendBackgroundColor("t", 1, 2, 3);
            Assert.Equal(Ansi.Background(1, 2, 3) + "t" + Ansi.DefaultBackground, sb.ToString());

            Assert.Equal(Ansi.Background(Xterm256.Red) + "t" + Ansi.DefaultBackground, "t".ToBackgroundColor(Xterm256.Red));
            Assert.Equal(Ansi.Background(Xterm16.Red) + "t" + Ansi.DefaultBackground, "t".ToBackgroundColor(Xterm16.Red));
        }

        [Fact]
        public void Color_Rgb_And_Xterm16_Wraps()
        {
            Assert.Equal(Ansi.Foreground(1, 2, 3) + "hi" + Ansi.DefaultForeground, "hi".ToColor(1, 2, 3));
            Assert.Equal(Ansi.Foreground(Xterm16.Red) + "x" + Ansi.DefaultForeground, "x".ToColor(Xterm16.Red));
        }

        [Fact]
#pragma warning disable CS8625
        public void EscapeSGR_Null_And_Empty_Work()
        {
            Assert.Equal("\u001b[m", string.Empty.ToSGR());
            var sb = new StringBuilder(); sb.AppendSGR("");
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
