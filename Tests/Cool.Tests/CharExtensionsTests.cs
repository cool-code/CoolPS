using System;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class CharExtensionsTests
    {
        [Fact]
        public void AllChars_IsBetween_BasicRanges()
        {
            var ranges = new (char start, char end)[] { ('0', '9'), ('A', 'Z'), ('a', 'z') };
            foreach (var (start, end) in ranges)
            {
                int mismatches = 0;
                for (int i = 0; i <= 0xFFFF; i++)
                {
                    char c = (char)i;
                    bool actual = c.IsBetween(start, end);
                    bool expected = c >= start && c <= end;
                    if (actual != expected)
                    {
                        mismatches++;
                        if (mismatches > 10) break;
                    }
                }
                Assert.Equal(0, mismatches);
            }
        }

        [Fact]
        public void AllChars_IsAscii_Exhaustive()
        {
            int mismatches = 0;
            for (int i = 0; i <= 0xFFFF; i++)
            {
                char c = (char)i;
                bool actual = c.IsAscii();
                bool expected = i <= 127;
                if (actual != expected)
                {
                    mismatches++;
                    if (mismatches > 10) break;
                }
            }
            Assert.Equal(0, mismatches);
        }

        [Fact]
        public void AllChars_IsControl_Exhaustive()
        {
            int mismatches = 0;
            for (int i = 0; i <= 0xFFFF; i++)
            {
                char c = (char)i;
                bool actual = c.IsControl();
                bool expected = (i <= 0x1F) || (i >= 0x7F && i <= 0x9F);
                if (actual != expected)
                {
                    mismatches++;
                    if (mismatches > 10) break;
                }
            }
            Assert.Equal(0, mismatches);
        }

        [Fact]
        public void AllChars_IsC1Control_Exhaustive()
        {
            int mismatches = 0;
            for (int i = 0; i <= 0xFFFF; i++)
            {
                char c = (char)i;
                bool actual = c.IsC1Control();
                bool expected = (i >= 0x80 && i <= 0x9F);
                if (actual != expected)
                {
                    mismatches++;
                    if (mismatches > 10) break;
                }
            }
            Assert.Equal(0, mismatches);
        }

        [Fact]
        public void AllChars_IsAsciiDigit_Exhaustive()
        {
            int mismatches = 0;
            for (int i = 0; i <= 0xFFFF; i++)
            {
                char c = (char)i;
                bool actual = c.IsAsciiDigit();
                bool expected = (i >= '0' && i <= '9');
                if (actual != expected)
                {
                    mismatches++;
                    if (mismatches > 10) break;
                }
            }
            Assert.Equal(0, mismatches);
        }

        [Fact]
        public void AllChars_IsAsciiHexDigit_Exhaustive()
        {
            int mismatches = 0;
            for (int i = 0; i <= 0xFFFF; i++)
            {
                char c = (char)i;
                bool actual = c.IsAsciiHexDigit();
                bool expected = (i >= '0' && i <= '9') || (i >= 'a' && i <= 'f') || (i >= 'A' && i <= 'F');
                if (actual != expected)
                {
                    mismatches++;
                    if (mismatches > 10) break;
                }
            }
            Assert.Equal(0, mismatches);
        }

        [Fact]
        public void AllChars_IsAsciiHexDigitUpper_Exhaustive()
        {
            int mismatches = 0;
            for (int i = 0; i <= 0xFFFF; i++)
            {
                char c = (char)i;
                bool actual = c.IsAsciiHexDigitUpper();
                bool expected = (i >= '0' && i <= '9') || (i >= 'A' && i <= 'F');
                if (actual != expected)
                {
                    mismatches++;
                    if (mismatches > 10) break;
                }
            }
            Assert.Equal(0, mismatches);
        }

        [Fact]
        public void AllChars_IsAsciiHexDigitLower_Exhaustive()
        {
            int mismatches = 0;
            for (int i = 0; i <= 0xFFFF; i++)
            {
                char c = (char)i;
                bool actual = c.IsAsciiHexDigitLower();
                bool expected = (i >= '0' && i <= '9') || (i >= 'a' && i <= 'f');
                if (actual != expected)
                {
                    mismatches++;
                    if (mismatches > 10) break;
                }
            }
            Assert.Equal(0, mismatches);
        }

        [Fact]
        public void AllChars_IsAsciiLetter_Exhaustive()
        {
            int mismatches = 0;
            for (int i = 0; i <= 0xFFFF; i++)
            {
                char c = (char)i;
                bool actual = c.IsAsciiLetter();
                bool expected = (i >= 'a' && i <= 'z') || (i >= 'A' && i <= 'Z');
                if (actual != expected)
                {
                    mismatches++;
                    if (mismatches > 10) break;
                }
            }
            Assert.Equal(0, mismatches);
        }

        [Fact]
        public void AllChars_IsAsciiLetterUpperLower_Exhaustive()
        {
            int mismatchesUpper = 0, mismatchesLower = 0;
            for (int i = 0; i <= 0xFFFF; i++)
            {
                char c = (char)i;
                bool actualU = c.IsAsciiLetterUpper();
                bool expectedU = (i >= 'A' && i <= 'Z');
                if (actualU != expectedU && mismatchesUpper++ == 0) Assert.True(false, $"IsAsciiLetterUpper mismatch at U+{i:X4}");

                bool actualL = c.IsAsciiLetterLower();
                bool expectedL = (i >= 'a' && i <= 'z');
                if (actualL != expectedL && mismatchesLower++ == 0) Assert.True(false, $"IsAsciiLetterLower mismatch at U+{i:X4}");
                if (mismatchesUpper > 10 || mismatchesLower > 10) break;
            }
            Assert.Equal(0, mismatchesUpper);
            Assert.Equal(0, mismatchesLower);
        }
    }
}
