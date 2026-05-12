using System;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class BitmapTests
    {
        private const uint MaxCodePoint = 0x1FFFFu;

        // Copied from CodePoint._wideRange (kept private in CodePoint)
        private const string WideRange = "1100-115F,231A-231B,2329-232A,23E9-23EC,23F0,23F3,25FD-25FE,2614-2615,2630-2637,2648-2653,267F,268A-268F,2693,26A1,26AA-26AB,26BD-26BE,26C4-26C5,26CE,26D4,26EA,26F2-26F3,26F5,26FA,26FD,2705,270A-270B,2728,274C,274E,2753-2755,2757,2795-2797,27B0,27BF,2B1B-2B1C,2B50,2B55,2E80-2E99,2E9B-2EF3,2F00-2FD5,2FF0-303E,3041-3096,3099-30FF,3105-312F,3131-318E,3190-31E5,31EF-321E,3220-3247,3250-A48C,A490-A4C6,A960-A97C,AC00-D7A3,F900-FAFF,FE10-FE19,FE30-FE52,FE54-FE66,FE68-FE6B,FF01-FF60,FFE0-FFE6,16FE0-16FE4,16FF0-16FF6,17000-18CD5,18CFF-18D1E,18D80-18DF2,1AFF0-1AFF3,1AFF5-1AFFB,1AFFD-1AFFE,1B000-1B122,1B132,1B150-1B152,1B155,1B164-1B167,1B170-1B2FB,1D300-1D356,1D360-1D376,1F004,1F0CF,1F18E,1F191-1F19A,1F200-1F202,1F210-1F23B,1F240-1F248,1F250-1F251,1F260-1F265,1F300-1F320,1F32D-1F335,1F337-1F37C,1F37E-1F393,1F3A0-1F3CA,1F3CF-1F3D3,1F3E0-1F3F0,1F3F4,1F3F8-1F43E,1F440,1F442-1F4FC,1F4FF-1F53D,1F54B-1F54E,1F550-1F567,1F57A,1F595-1F596,1F5A4,1F5FB-1F64F,1F680-1F6C5,1F6CC,1F6D0-1F6D2,1F6D5-1F6D8,1F6DC-1F6DF,1F6EB-1F6EC,1F6F4-1F6FC,1F7E0-1F7EB,1F7F0,1F90C-1F93A,1F93C-1F945,1F947-1F9FF,1FA70-1FA7C,1FA80-1FA8A,1FA8E-1FAC6,1FAC8,1FACD-1FADC,1FADF-1FAEA,1FAEF-1FAF8";

        [Fact]
        public void Bitmap_SetsExpectedBits_FromWideRange()
        {
            var bmp = new Bitmap(MaxCodePoint, WideRange);
            try
            {
                // simple boundaries from the first range 1100-115F
                Assert.True(bmp.GetBit(0x1100u));
                Assert.True(bmp.GetBit(0x115Fu));
                Assert.False(bmp.GetBit(0x10FFu));
                Assert.False(bmp.GetBit(0x1160u));

                // single code point 23F0
                Assert.True(bmp.GetBit(0x23F0u));
                Assert.False(bmp.GetBit(0x23F1u));

                // cross-plane-ish examples
                Assert.True(bmp.GetBit(0x3250u));
                Assert.True(bmp.GetBit(0xA48Cu));
                Assert.False(bmp.GetBit(0xA48Du));

                // AC00-D7A3 boundary
                Assert.True(bmp.GetBit(0xAC00u));
                Assert.True(bmp.GetBit(0xD7A3u));
                Assert.False(bmp.GetBit(0xD7A4u));

                // very large ranges
                Assert.True(bmp.GetBit(0x17000u));
                Assert.True(bmp.GetBit(0x18CD5u));
                Assert.False(bmp.GetBit(0x18CD6u));

                // last value listed in the wide range: 1FAF8
                Assert.True(bmp.GetBit(0x1FAF8u));
                Assert.False(bmp.GetBit(0x1FAF9u));

                // outside of bitmap limit
                Assert.False(bmp.GetBit(0x20000u));
            }
            finally
            {
                bmp.Dispose();
            }
        }

        [Fact]
        public void Bitmap_ClearAndSetBit_Works()
        {
            var bmp = new Bitmap(MaxCodePoint, WideRange);
            try
            {
                uint pos = 0x1100u;
                Assert.True(bmp.GetBit(pos));
                bmp.ClearBit(pos);
                Assert.False(bmp.GetBit(pos));
                bmp.SetBit(pos);
                Assert.True(bmp.GetBit(pos));
            }
            finally
            {
                bmp.Dispose();
            }
        }

        [Fact]
        public void Bitmap_OutOfRange_Positions_AreIgnored()
        {
            // small bitmap: only 0x00..0xFF
            var bmp = new Bitmap(0xFFu, "0-FF");
            try
            {
                Assert.True(bmp.GetBit(0x0u));
                Assert.True(bmp.GetBit(0xFFu));
                // exactly one past the limit should be false and ignored by Set/Clear
                Assert.False(bmp.GetBit(0x100u));
                bmp.ClearBit(0x100u); // should not throw
                bmp.SetBit(0x100u); // should not set anything
                Assert.False(bmp.GetBit(0x100u));
            }
            finally
            {
                bmp.Dispose();
            }
        }

        [Fact]
        public void Bitmap_ToString_RoundTrip_Small()
        {
            var bmp = new Bitmap(10, "1-3,5,7-9");
            try
            {
                Assert.Equal("1-3,5,7-9", bmp.ToString());
            }
            finally { bmp.Dispose(); }
        }

        [Fact]
        public void Bitmap_ToString_RoundTrip_WideRange()
        {
            var bmp = new Bitmap(MaxCodePoint, WideRange);
            try
            {
                Assert.Equal(WideRange, bmp.ToString());
            }
            finally { bmp.Dispose(); }
        }

        [Fact]
        public void Bitmap_SizeProperty_IsCalculatedFromBitHighLimit()
        {
            // helper to check expected size
            void Check(uint limit, int expected)
            {
                var bmp = new Bitmap(limit, "0");
                try
                {
                    Assert.Equal(expected, bmp.Size);
                }
                finally { bmp.Dispose(); }
            }

            Check(0u, 1);             // 0 >> 3 = 0 + 1 = 1 byte
            Check(7u, 1);             // fits in one byte
            Check(8u, 2);             // needs second byte
            Check(15u, 2);
            Check(31u, 4);            // 31 >> 3 = 3 + 1 = 4
            Check(32u, 5);            // 32 >> 3 = 4 + 1 = 5
            Check(255u, 32);          // 255 >> 3 = 31 + 1 = 32
            Check(256u, 33);          // 256 >> 3 = 32 + 1 = 33
            Check(MaxCodePoint, (int)(MaxCodePoint >> 3) + 1);
        }
    }
}
