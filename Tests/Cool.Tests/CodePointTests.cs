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
            const string wideRange = "1100-115F,231A-231B,2329-232A,23E9-23EC,23F0,23F3,25FD-25FE,2614-2615,2630-2637,2648-2653,267F,268A-268F,2693,26A1,26AA-26AB,26BD-26BE,26C4-26C5,26CE,26D4,26EA,26F2-26F3,26F5,26FA,26FD,2705,270A-270B,2728,274C,274E,2753-2755,2757,2795-2797,27B0,27BF,2B1B-2B1C,2B50,2B55,2E80-2E99,2E9B-2EF3,2F00-2FD5,2FF0-303E,3041-3096,3099-30FF,3105-312F,3131-318E,3190-31E5,31EF-321E,3220-3247,3250-A48C,A490-A4C6,A960-A97C,AC00-D7A3,F900-FAFF,FE10-FE19,FE30-FE52,FE54-FE66,FE68-FE6B,FF01-FF60,FFE0-FFE6,16FE0-16FE4,16FF0-16FF6,17000-18CD5,18CFF-18D1E,18D80-18DF2,1AFF0-1AFF3,1AFF5-1AFFB,1AFFD-1AFFE,1B000-1B122,1B132,1B150-1B152,1B155,1B164-1B167,1B170-1B2FB,1D300-1D356,1D360-1D376,1F004,1F0CF,1F18E,1F191-1F19A,1F200-1F202,1F210-1F23B,1F240-1F248,1F250-1F251,1F260-1F265,1F300-1F320,1F32D-1F335,1F337-1F37C,1F37E-1F393,1F3A0-1F3CA,1F3CF-1F3D3,1F3E0-1F3F0,1F3F4,1F3F8-1F43E,1F440,1F442-1F4FC,1F4FF-1F53D,1F54B-1F54E,1F550-1F567,1F57A,1F595-1F596,1F5A4,1F5FB-1F64F,1F680-1F6C5,1F6CC,1F6D0-1F6D2,1F6D5-1F6D8,1F6DC-1F6DF,1F6EB-1F6EC,1F6F4-1F6FC,1F7E0-1F7EB,1F7F0,1F90C-1F93A,1F93C-1F945,1F947-1F9FF,1FA70-1FA7C,1FA80-1FA8A,1FA8E-1FAC6,1FAC8,1FACD-1FADC,1FADF-1FAEA,1FAEF-1FAF8";
            const string ambigRange = "A1,A4,A7-A8,AA,AD-AE,B0-B4,B6-BA,BC-BF,C6,D0,D7-D8,DE-E1,E6,E8-EA,EC-ED,F0,F2-F3,F7-FA,FC,FE,101,111,113,11B,126-127,12B,131-133,138,13F-142,144,148-14B,14D,152-153,166-167,16B,1CE,1D0,1D2,1D4,1D6,1D8,1DA,1DC,251,261,2C4,2C7,2C9-2CB,2CD,2D0,2D8-2DB,2DD,2DF,300-36F,391-3A1,3A3-3A9,3B1-3C1,3C3-3C9,401,410-44F,451,2010,2013-2016,2018-2019,201C-201D,2020-2022,2024-2027,2030,2032-2033,2035,203B,203E,2074,207F,2081-2084,20AC,2103,2105,2109,2113,2116,2121-2122,2126,212B,2153-2154,215B-215E,2160-216B,2170-2179,2189,2190-2199,21B8-21B9,21D2,21D4,21E7,2200,2202-2203,2207-2208,220B,220F,2211,2215,221A,221D-2220,2223,2225,2227-222C,222E,2234-2237,223C-223D,2248,224C,2252,2260-2261,2264-2267,226A-226B,226E-226F,2282-2283,2286-2287,2295,2299,22A5,22BF,2312,2460-24E9,24EB-254B,2550-2573,2580-258F,2592-2595,25A0-25A1,25A3-25A9,25B2-25B3,25B6-25B7,25BC-25BD,25C0-25C1,25C6-25C8,25CB,25CE-25D1,25E2-25E5,25EF,2605-2606,2609,260E-260F,261C,261E,2640,2642,2660-2661,2663-2665,2667-266A,266C-266D,266F,269E-269F,26BF,26C6-26CD,26CF-26D3,26D5-26E1,26E3,26E8-26E9,26EB-26F1,26F4,26F6-26F9,26FB-26FC,26FE-26FF,273D,2776-277F,2B56-2B59,3248-324F,E000-F8FF,FE00-FE0F,FFFD,1F100-1F10A,1F110-1F12D,1F130-1F169,1F170-1F18D,1F18F-1F190,1F19B-1F1AC";
            const string zeroRange = "AD,300-36F,483-489,591-5BD,5BF,5C1-5C2,5C4-5C5,5C7,600-605,610-61A,61C,64B-65F,670,6D6-6DD,6DF-6E4,6E7-6E8,6EA-6ED,70F,711,730-74A,7A6-7B0,7EB-7F3,7FD,816-819,81B-823,825-827,829-82D,859-85B,890-891,897-89F,8CA-903,93A-93C,93E-94F,951-957,962-963,981-983,9BC,9BE-9C4,9C7-9C8,9CB-9CD,9D7,9E2-9E3,9FE,A01-A03,A3C,A3E-A42,A47-A48,A4B-A4D,A51,A70-A71,A75,A81-A83,ABC,ABE-AC5,AC7-AC9,ACB-ACD,AE2-AE3,AFA-AFF,B01-B03,B3C,B3E-B44,B47-B48,B4B-B4D,B55-B57,B62-B63,B82,BBE-BC2,BC6-BC8,BCA-BCD,BD7,C00-C04,C3C,C3E-C44,C46-C48,C4A-C4D,C55-C56,C62-C63,C81-C83,CBC,CBE-CC4,CC6-CC8,CCA-CCD,CD5-CD6,CE2-CE3,CF3,D00-D03,D3B-D3C,D3E-D44,D46-D48,D4A-D4D,D57,D62-D63,D81-D83,DCA,DCF-DD4,DD6,DD8-DDF,DF2-DF3,E31,E34-E3A,E47-E4E,EB1,EB4-EBC,EC8-ECE,F18-F19,F35,F37,F39,F3E-F3F,F71-F84,F86-F87,F8D-F97,F99-FBC,FC6,102B-103E,1056-1059,105E-1060,1062-1064,1067-106D,1071-1074,1082-108D,108F,109A-109D,135D-135F,1712-1715,1732-1734,1752-1753,1772-1773,17B4-17D3,17DD,180B-180F,1885-1886,18A9,1920-192B,1930-193B,1A17-1A1B,1A55-1A5E,1A60-1A7C,1A7F,1AB0-1ADD,1AE0-1AEB,1B00-1B04,1B34-1B44,1B6B-1B73,1B80-1B82,1BA1-1BAD,1BE6-1BF3,1C24-1C37,1CD0-1CD2,1CD4-1CE8,1CED,1CF4,1CF7-1CF9,1DC0-1DFF,200B-200F,202A-202E,2060-2064,2066-206F,20D0-20F0,2CEF-2CF1,2D7F,2DE0-2DFF,302A-302F,3099-309A,A66F-A672,A674-A67D,A69E-A69F,A6F0-A6F1,A802,A806,A80B,A823-A827,A82C,A880-A881,A8B4-A8C5,A8E0-A8F1,A8FF,A926-A92D,A947-A953,A980-A983,A9B3-A9C0,A9E5,AA29-AA36,AA43,AA4C-AA4D,AA7B-AA7D,AAB0,AAB2-AAB4,AAB7-AAB8,AABE-AABF,AAC1,AAEB-AAEF,AAF5-AAF6,ABE3-ABEA,ABEC-ABED,FB1E,FE00-FE0F,FE20-FE2F,FEFF,FFF9-FFFB,101FD,102E0,10376-1037A,10A01-10A03,10A05-10A06,10A0C-10A0F,10A38-10A3A,10A3F,10AE5-10AE6,10D24-10D27,10D69-10D6D,10EAB-10EAC,10EFA-10EFF,10F46-10F50,10F82-10F85,11000-11002,11038-11046,11070,11073-11074,1107F-11082,110B0-110BA,110BD,110C2,110CD,11100-11102,11127-11134,11145-11146,11173,11180-11182,111B3-111C0,111C9-111CC,111CE-111CF,1122C-11237,1123E,11241,112DF-112EA,11300-11303,1133B-1133C,1133E-11344,11347-11348,1134B-1134D,11357,11362-11363,11366-1136C,11370-11374,113B8-113C0,113C2,113C5,113C7-113CA,113CC-113D0,113D2,113E1-113E2,11435-11446,1145E,114B0-114C3,115AF-115B5,115B8-115C0,115DC-115DD,11630-11640,116AB-116B7,1171D-1172B,1182C-1183A,11930-11935,11937-11938,1193B-1193E,11940,11942-11943,119D1-119D7,119DA-119E0,119E4,11A01-11A0A,11A33-11A39,11A3B-11A3E,11A47,11A51-11A5B,11A8A-11A99,11B60-11B67,11C2F-11C36,11C38-11C3F,11C92-11CA7,11CA9-11CB6,11D31-11D36,11D3A,11D3C-11D3D,11D3F-11D45,11D47,11D8A-11D8E,11D90-11D91,11D93-11D97,11EF3-11EF6,11F00-11F01,11F03,11F34-11F3A,11F3E-11F42,11F5A,13430-13440,13447-13455,1611E-1612F,16AF0-16AF4,16B30-16B36,16F4F,16F51-16F87,16F8F-16F92,16FE4,16FF0-16FF1,1BC9D-1BC9E,1BCA0-1BCA3,1CF00-1CF2D,1CF30-1CF46,1D165-1D169,1D16D-1D182,1D185-1D18B,1D1AA-1D1AD,1D242-1D244,1DA00-1DA36,1DA3B-1DA6C,1DA75,1DA84,1DA9B-1DA9F,1DAA1-1DAAF,1E000-1E006,1E008-1E018,1E01B-1E021,1E023-1E024,1E026-1E02A,1E08F,1E130-1E136,1E2AE,1E2EC-1E2EF,1E4EC-1E4EF,1E5EE-1E5EF,1E6E3,1E6E6,1E6EE-1E6EF,1E6F5,1E8D0-1E8D6,1E944-1E94A,1F3FB-1F3FF";
            const string emojiRange = "A9,AE,203C,2049,2122,2139,2194-2199,21A9-21AA,231A-231B,2328,23CF,23E9-23F3,23F8-23FA,24C2,25AA-25AB,25B6,25C0,25FB-25FE,2600-2604,260E,2611,2614-2615,2618,261D,2620,2622-2623,2626,262A,262E-262F,2638-263A,2640,2642,2648-2653,265F-2660,2663,2665-2666,2668,267B,267E-267F,2692-2697,2699,269B-269C,26A0-26A1,26A7,26AA-26AB,26B0-26B1,26BD-26BE,26C4-26C5,26C8,26CE-26CF,26D1,26D3-26D4,26E9-26EA,26F0-26F5,26F7-26FA,26FD,2702,2705,2708-270D,270F,2712,2714,2716,271D,2721,2728,2733-2734,2744,2747,274C,274E,2753-2755,2757,2763-2764,2795-2797,27A1,27B0,27BF,2934-2935,2B05-2B07,2B1B-2B1C,2B50,2B55,3030,303D,3297,3299,1F004,1F02C-1F02F,1F094-1F09F,1F0AF-1F0B0,1F0C0,1F0CF-1F0D0,1F0F6-1F0FF,1F170-1F171,1F17E-1F17F,1F18E,1F191-1F19A,1F1AE-1F1FF,1F201-1F20F,1F21A,1F22F,1F232-1F23A,1F23C-1F23F,1F249-1F25F,1F266-1F321,1F324-1F393,1F396-1F397,1F399-1F39B,1F39E-1F3F0,1F3F3-1F3F5,1F3F7-1F4FD,1F4FF-1F53D,1F549-1F54E,1F550-1F567,1F56F-1F570,1F573-1F57A,1F587,1F58A-1F58D,1F590,1F595-1F596,1F5A4-1F5A5,1F5A8,1F5B1-1F5B2,1F5BC,1F5C2-1F5C4,1F5D1-1F5D3,1F5DC-1F5DE,1F5E1,1F5E3,1F5E8,1F5EF,1F5F3,1F5FA-1F64F,1F680-1F6C5,1F6CB-1F6D2,1F6D5-1F6E5,1F6E9,1F6EB-1F6F0,1F6F3-1F6FF,1F7DA-1F7FF,1F80C-1F80F,1F848-1F84F,1F85A-1F85F,1F888-1F88F,1F8AE-1F8AF,1F8BC-1F8BF,1F8C2-1F8CF,1F8D9-1F8FF,1F90C-1F93A,1F93C-1F945,1F947-1F9FF,1FA58-1FA5F,1FA6E-1FAFF,1FC00-1FFFD";

            var wideBitmap = Bitmap.CreateStatic(0x1FFFFu, wideRange);
            var ambigBitmap = Bitmap.CreateStatic(0x1FFFFu, ambigRange);
            var zeroBitmap = Bitmap.CreateStatic(0x1FFFFu, zeroRange);
            var emojiBitmap = Bitmap.CreateStatic(0x1FFFFu, emojiRange);

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
                int expectedCharCount = (v >= 0x10000u && v <= 0x10FFFFu) ? 2 : 1;
                if (cp.CharCount != expectedCharCount)
                {
                    mismatchesCharCount++;
                    if (mismatchesCharCount > 20) break;
                }

                // ASCII and ascii-related properties
                bool expectedIsAscii = v <= 127u;
                if (cp.IsAscii() != expectedIsAscii)
                {
                    mismatchesAscii++;
                    if (mismatchesAscii > 20) break;
                }

                bool expectedIsAsciiDigit = v >= '0' && v <= '9';
                bool expectedIsAsciiHex = expectedIsAsciiDigit || (v >= 'a' && v <= 'f') || (v >= 'A' && v <= 'F');
                bool expectedIsAsciiUpper = v >= 'A' && v <= 'Z';
                bool expectedIsAsciiLower = v >= 'a' && v <= 'z';
                if (cp.IsAsciiDigit() != expectedIsAsciiDigit || cp.IsAsciiHexDigit() != expectedIsAsciiHex || cp.IsAsciiLetterUpper() != expectedIsAsciiUpper || cp.IsAsciiLetterLower() != expectedIsAsciiLower || cp.IsAsciiLetter() != (expectedIsAsciiUpper || expectedIsAsciiLower))
                {
                    mismatchesAsciiProps++;
                    if (mismatchesAsciiProps > 20) break;
                }

                // control characters
                bool expectedIsControl = (v <= 0x1Fu) || (v >= 0x7Fu && v <= 0x9Fu);
                bool expectedIsC1 = v >= 0x80u && v <= 0x9Fu;
                if (cp.IsControl() != expectedIsControl || cp.IsC1Control() != expectedIsC1)
                {
                    mismatchesControl++;
                    if (mismatchesControl > 20) break;
                }

                // surrogate checks
                bool expectedHigh = v >= 0xD800u && v <= 0xDBFFu;
                bool expectedLow = v >= 0xDC00u && v <= 0xDFFFu;
                bool expectedSur = v >= 0xD800u && v <= 0xDFFFu;
                if (cp.IsHighSurrogate() != expectedHigh || cp.IsLowSurrogate() != expectedLow || cp.IsSurrogate() != expectedSur)
                {
                    mismatchesSurrogate++;
                    if (mismatchesSurrogate > 20) break;
                }

                // wide/ambiguous/zero/emoji expectations via the same bitmap+range logic used in CodePoint
                bool expectedWide = wideBitmap.GetBit(v) || ((v - 0x20000u) & ~0x10000u) <= 0xFFFDu;
                if (cp.IsWideWidth() != expectedWide)
                {
                    mismatchesWide++;
                    if (mismatchesWide > 20) break;
                }

                bool expectedAmbig = ambigBitmap.GetBit(v) || ((v - 0xF0000u) & ~0x10000u) <= 0xFFFDu;
                if (cp.IsAmbiguousWidth() != expectedAmbig)
                {
                    mismatchesAmbig++;
                    if (mismatchesAmbig > 20) break;
                }

                bool expectedZero = zeroBitmap.GetBit(v) || (v - 0xE0000u <= 0xE007Fu - 0xE0000u) || (v - 0xE0100u <= 0xE01EFu - 0xE0100u);
                if (cp.IsZeroWidth() != expectedZero)
                {
                    mismatchesZero++;
                    if (mismatchesZero > 20) break;
                }

                bool expectedEmoji = emojiBitmap.GetBit(v);
                if (cp.IsEmoji() != expectedEmoji)
                {
                    mismatchesEmoji++;
                    if (mismatchesEmoji > 20) break;
                }

                bool expectedEmojiMod = (v - 0x1F3FBu) <= (0x1F3FFu - 0x1F3FBu);
                if (cp.IsEmojiModifier() != expectedEmojiMod)
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
