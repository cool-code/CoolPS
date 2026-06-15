using System;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class ValueStringBuilderAppendNumberTests
    {
        [Fact]
        public void Append_Ulong_AllCases()
        {
            (ulong value, string expected)[] cases = new (ulong, string)[] {
                (0UL, "0"),
                (1UL, "1"),
                (9UL, "9"),
                (10UL, "10"),
                (99UL, "99"),
                (100UL, "100"),
                (12345678901234567890UL, "12345678901234567890"),
                (ulong.MaxValue, ulong.MaxValue.ToString())
            };

            foreach (var c in cases)
            {
                var sb = new ValueStringBuilder();
                sb.Append(c.value);
                Assert.Equal(c.expected, sb.ToString());
            }

            for (ulong i = 1; i < ulong.MaxValue / 3; i *= 3)
            {
                var sb = new ValueStringBuilder();
                sb.Append(i);
                Assert.Equal(i.ToString(), sb.ToString());
            }
        }

        [Fact]
        public void Append_Long_AllCases()
        {
            (long value, string expected)[] cases = new (long, string)[] {
                (0L, "0"),
                (1L, "1"),
                (-1L, "-1"),
                (10L, "10"),
                (100L, "100"),
                (long.MaxValue, long.MaxValue.ToString()),
                (long.MinValue, long.MinValue.ToString())
            };

            foreach (var c in cases)
            {
                var sb = new ValueStringBuilder();
                sb.Append(c.value);
                Assert.Equal(c.expected, sb.ToString());
            }
        }

        [Fact]
        public void Append_Int_And_UInt()
        {
            var sb = new ValueStringBuilder();
            sb.Append(0);
            Assert.Equal("0", sb.ToString());

            sb = new ValueStringBuilder();
            sb.Append(-1);
            Assert.Equal("-1", sb.ToString());

            sb = new ValueStringBuilder();
            sb.Append(int.MaxValue);
            Assert.Equal(int.MaxValue.ToString(), sb.ToString());

            sb = new ValueStringBuilder();
            sb.Append(int.MinValue);
            Assert.Equal(int.MinValue.ToString(), sb.ToString());

            sb = new ValueStringBuilder();
            sb.Append(uint.MaxValue);
            Assert.Equal(uint.MaxValue.ToString(), sb.ToString());
        }

        [Fact]
        public void Append_SmallerIntegerTypes()
        {
            // ushort
            var sb = new ValueStringBuilder();
            sb.Append((ushort)0);
            Assert.Equal("0", sb.ToString());

            sb = new ValueStringBuilder();
            sb.Append((ushort)65535);
            Assert.Equal(((ushort)65535).ToString(), sb.ToString());

            // short
            sb = new ValueStringBuilder();
            sb.Append((short)-32768);
            Assert.Equal(((short)-32768).ToString(), sb.ToString());

            sb = new ValueStringBuilder();
            sb.Append((short)32767);
            Assert.Equal(((short)32767).ToString(), sb.ToString());

            // byte
            sb = new ValueStringBuilder();
            sb.Append((byte)0);
            Assert.Equal("0", sb.ToString());

            sb = new ValueStringBuilder();
            sb.Append((byte)255);
            Assert.Equal(((byte)255).ToString(), sb.ToString());

            // sbyte
            sb = new ValueStringBuilder();
            sb.Append((sbyte)-128);
            Assert.Equal(((sbyte)-128).ToString(), sb.ToString());

            sb = new ValueStringBuilder();
            sb.Append((sbyte)127);
            Assert.Equal(((sbyte)127).ToString(), sb.ToString());
        }

        [Fact]
        public void Append_NInt_NUInt_Basic()
        {
            var sb = new ValueStringBuilder();
            sb.Append((nint)42);
            Assert.Equal("42", sb.ToString());

            sb = new ValueStringBuilder();
            sb.Append((nuint)42);
            Assert.Equal("42", sb.ToString());
        }
    }
}
