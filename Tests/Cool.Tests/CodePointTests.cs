using System;
using System.Reflection;
using System.Text;
using System.Linq;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class CodePointTests
    {
        [Fact]
        public void AllCodePoints_BasicProperties_Exhaustive()
        {
            // reflect private bitmaps used by CodePoint to build expected values
            var wideBitmap = (Bitmap)typeof(CodePoint).GetField("wideBitmap", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;
            var ambigBitmap = (Bitmap)typeof(CodePoint).GetField("ambigBitmap", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;
            var zeroBitmap = (Bitmap)typeof(CodePoint).GetField("zeroBitmap", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;
            var emojiBitmap = (Bitmap)typeof(CodePoint).GetField("emojiBitmap", BindingFlags.NonPublic | BindingFlags.Static)!.GetValue(null)!;

            int mismatchesConv = 0;
            int mismatchesCharCount = 0;
            int mismatchesAscii = 0;
            int mismatchesSurrogate = 0;
            int mismatchesAsciiProps = 0;
            int mismatchesControl = 0;
            int mismatchesWide = 0;
            int mismatchesAmbig = 0;
            int mismatchesZero = 0;
            int mismatchesEmoji = 0;
            int mismatchesEmojiMod = 0;

            const uint MaxUnicode = 0x10FFFFu;

            for (uint v = 0; v <= MaxUnicode; v++)
            {
                CodePoint cp = (CodePoint)v;

                // conversions
                if ((uint)cp != v)
                {
                    mismatchesConv++;
                    if (mismatchesConv > 20) break;
                }

                // CharCount: match the same arithmetic expression used in CodePoint
                int expectedCharCount = ((v - 0xFFFFu) <= (0x10FFFFu - 0xFFFFu)) ? 2 : 1;
                if (cp.CharCount != expectedCharCount)
                {
                    mismatchesCharCount++;
                    if (mismatchesCharCount > 20) break;
                }

                // ASCII and ascii-related properties
                bool expectedIsAscii = v <= 127u;
                if (cp.IsAscii != expectedIsAscii)
                {
                    mismatchesAscii++;
                    if (mismatchesAscii > 20) break;
                }

                bool expectedIsAsciiDigit = v >= '0' && v <= '9';
                bool expectedIsAsciiHex = expectedIsAsciiDigit || (v >= 'a' && v <= 'f') || (v >= 'A' && v <= 'F');
                bool expectedIsAsciiUpper = v >= 'A' && v <= 'Z';
                bool expectedIsAsciiLower = v >= 'a' && v <= 'z';
                if (cp.IsAsciiDigit != expectedIsAsciiDigit || cp.IsAsciiHexDigit != expectedIsAsciiHex || cp.IsAsciiLetterUpper != expectedIsAsciiUpper || cp.IsAsciiLetterLower != expectedIsAsciiLower || cp.IsAsciiLetter != (expectedIsAsciiUpper || expectedIsAsciiLower))
                {
                    mismatchesAsciiProps++;
                    if (mismatchesAsciiProps > 20) break;
                }

                // control characters
                bool expectedIsControl = (v <= 0x1Fu) || (v >= 0x7Fu && v <= 0x9Fu);
                bool expectedIsC1 = v >= 0x80u && v <= 0x9Fu;
                if (cp.IsControl != expectedIsControl || cp.IsC1Control != expectedIsC1)
                {
                    mismatchesControl++;
                    if (mismatchesControl > 20) break;
                }

                // surrogate checks
                bool expectedHigh = v >= 0xD800u && v <= 0xDBFFu;
                bool expectedLow = v >= 0xDC00u && v <= 0xDFFFu;
                bool expectedSur = v >= 0xD800u && v <= 0xDFFFu;
                if (cp.IsHighSurrogate != expectedHigh || cp.IsLowSurrogate != expectedLow || cp.IsSurrogate != expectedSur)
                {
                    mismatchesSurrogate++;
                    if (mismatchesSurrogate > 20) break;
                }

                // wide/ambiguous/zero/emoji expectations via the same bitmap+range logic used in CodePoint
                bool expectedWide = wideBitmap.GetBit(v) || ((v - 0x20000u) & ~0x10000u) <= 0xFFFDu;
                if (cp.IsWideWidth != expectedWide)
                {
                    mismatchesWide++;
                    if (mismatchesWide > 20) break;
                }

                bool expectedAmbig = ambigBitmap.GetBit(v) || ((v - 0xF0000u) & ~0x10000u) <= 0xFFFDu;
                if (cp.IsAmbiguousWidth != expectedAmbig)
                {
                    mismatchesAmbig++;
                    if (mismatchesAmbig > 20) break;
                }

                bool expectedZero = zeroBitmap.GetBit(v) || (v - 0xE0000u <= 0xE007Fu - 0xE0000u) || (v - 0xE0100u <= 0xE01EFu - 0xE0100u);
                if (cp.IsZeroWidth != expectedZero)
                {
                    mismatchesZero++;
                    if (mismatchesZero > 20) break;
                }

                bool expectedEmoji = emojiBitmap.GetBit(v);
                if (cp.IsEmoji != expectedEmoji)
                {
                    mismatchesEmoji++;
                    if (mismatchesEmoji > 20) break;
                }

                bool expectedEmojiMod = (v - 0x1F3FBu) <= (0x1F3FFu - 0x1F3FBu);
                if (cp.IsEmojiModifier != expectedEmojiMod)
                {
                    mismatchesEmojiMod++;
                    if (mismatchesEmojiMod > 20) break;
                }
            }

            // Assert no mismatches
            Assert.Equal(0, mismatchesConv);
            Assert.Equal(0, mismatchesCharCount);
            Assert.Equal(0, mismatchesAscii);
            Assert.Equal(0, mismatchesAsciiProps);
            Assert.Equal(0, mismatchesControl);
            Assert.Equal(0, mismatchesSurrogate);
            Assert.Equal(0, mismatchesWide);
            Assert.Equal(0, mismatchesAmbig);
            Assert.Equal(0, mismatchesZero);
            Assert.Equal(0, mismatchesEmoji);
            Assert.Equal(0, mismatchesEmojiMod);
        }

        [Fact]
        public void CodePoint_Operators_And_Methods_Samples()
        {
            uint[] samples = [0u, 'A', '0', 0xD7FFu, 0xD800u, 0xDBFFu, 0xDC00u, 0xDFFFu, 0xFFFFu, 0x10000u, 0x10FFFFu, 0x110000u, uint.MaxValue];

            foreach (var v in samples)
            {
                CodePoint cp = (CodePoint)v;

                // arithmetic with ints - use a separate temporary so we don't modify `cp`
                var tmp = cp;
                var plus5 = tmp + 5;
                Assert.Equal(v + 5, (uint)plus5);
                var minus3 = tmp - 3;
                Assert.Equal(v - 3, (uint)minus3);

                // difference between code points
                var cp2 = (CodePoint)(v + 7);
                Assert.Equal((int)((uint)cp2 - (uint)cp), cp2 - cp);

                // increment / decrement on a copy
                var arith = cp;
                var inc = ++arith;
                Assert.Equal(v + 1, (uint)inc);
                var dec = --inc;
                Assert.Equal(v, (uint)dec);

                // ToString and concatenation and repetition (use original cp)
                string expected = ExpectedToString(v);
                Assert.Equal(expected, cp.ToString());
                Assert.Equal("X" + expected, "X" + cp);
                Assert.Equal(expected + "Y", cp + "Y");

                string rep = cp * 3;
                Assert.Equal(string.Concat(expected, expected, expected), rep);

                string rep2 = cp * 4096;
                Assert.Equal(string.Concat(Enumerable.Repeat(expected, 4096)), rep2);

                // Append and AppendUnicode helpers
                var sb = new StringBuilder();
                sb.Append(cp);
                Assert.Equal(expected, sb.ToString());

                var sb2 = new StringBuilder();
                sb2.AppendUnicode(cp);
                Assert.Equal(ExpectedToUnicode(v), sb2.ToString());
            }
        }

        [Fact]
        public void FromSurrogatePair_ValidAndInvalid()
        {
            char high = (char)0xD800;
            char low = (char)0xDC00;
            var cp = CodePoint.FromSurrogatePair(high, low);
            Assert.Equal(0x10000u, (uint)cp);

            // invalid pairs throw
            Assert.Throws<ArgumentException>(() => CodePoint.FromSurrogatePair((char)0, (char)0));
            Assert.Throws<ArgumentException>(() => CodePoint.FromSurrogatePair(0u, 0u));
            Assert.Throws<ArgumentException>(() => CodePoint.FromSurrogatePair(0, 0));
        }

        [Fact]
        public void ImplicitConversions_And_Equals_HashCode()
        {
            CodePoint a = (CodePoint)0x41u; // 'A'
            uint au = (uint)a;
            int ai = (int)a;
            Assert.Equal(0x41u, au);
            Assert.Equal(0x41, ai);

            CodePoint fromChar = (CodePoint)'Z';
            char asChar = (char)fromChar;
            Assert.Equal('Z', asChar);

            // equals with different boxed types
            Assert.True(a.Equals((object)0x41u));
            Assert.True(a.Equals((object)0x41));
            Assert.True(a.Equals((object)'A'));

            Assert.Equal((int)0x41u, a.GetHashCode());
        }

        private static string ExpectedToString(uint v)
        {
            if (v <= 0xFFFFu) return new string((char)v, 1);
            if (v > 0x10FFFFu) return "\uFFFD";
            uint t = v - 0x10000u;
            char high = (char)((t >> 10) + 0xD800);
            char low = (char)((t & 0x3FFu) + 0xDC00);
            return new string([high, low]);
        }

        private static string ExpectedToUnicode(uint v)
        {
            if (v > 0x10FFFFu) return "U+FFFD";
            int digits = (v <= 0xFFFFu) ? 4 : (v <= 0xFFFFFu) ? 5 : 6;
            return "U+" + v.ToString("X" + digits);
        }
    }
}
