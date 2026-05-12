using System;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class CharExtensionsTests
    {
        [Theory]
        [InlineData('d', 'a', 'f', true)]
        [InlineData('a', 'a', 'z', true)]
        [InlineData('z', 'a', 'y', false)]
        [InlineData('A', 'A', 'Z', true)]
        public void IsBetween_Works(char c, char start, char end, bool expected)
        {
            Assert.Equal(expected, c.IsBetween(start, end));
        }

        [Fact]
        public void IsAscii_IdentifiesAscii()
        {
            Assert.True(((char)127).IsAscii());
            Assert.False(((char)128).IsAscii());
            Assert.True('A'.IsAscii());
            Assert.True(' '.IsAscii());
        }

        [Fact]
        public void IsControl_IdentifiesC0Control()
        {
            Assert.True('\n'.IsControl());
            Assert.True(((char)0).IsControl());
            Assert.True(((char)0x1Fu).IsControl());
            Assert.False(' '.IsControl());
        }

        [Fact]
        public void IsC1Control_IdentifiesRange()
        {
            Assert.True(((char)0x80u).IsC1Control());
            Assert.True(((char)0x9Fu).IsC1Control());
            Assert.False(((char)0xA0u).IsC1Control());
        }

        [Fact]
        public void IsAsciiDigit_Works()
        {
            Assert.True('0'.IsAsciiDigit());
            Assert.True('9'.IsAsciiDigit());
            Assert.False('a'.IsAsciiDigit());
        }

        [Fact]
        public void IsAsciiHexDigit_Works()
        {
            foreach (var ch in new[] { '0', '9', 'a', 'f', 'A', 'F' })
                Assert.True(ch.IsAsciiHexDigit());

            Assert.False('g'.IsAsciiHexDigit());
            Assert.False('/'.IsAsciiHexDigit());
        }

        [Fact]
        public void IsAsciiHexDigitUpper_And_Lower_Work()
        {
            Assert.True('A'.IsAsciiHexDigitUpper());
            Assert.True('F'.IsAsciiHexDigitUpper());
            Assert.True('0'.IsAsciiHexDigitUpper());
            Assert.False('a'.IsAsciiHexDigitUpper());

            Assert.True('a'.IsAsciiHexDigitLower());
            Assert.True('f'.IsAsciiHexDigitLower());
            Assert.True('0'.IsAsciiHexDigitLower());
            Assert.False('A'.IsAsciiHexDigitLower());
        }

        [Fact]
        public void IsAsciiLetter_Varieties()
        {
            Assert.True('a'.IsAsciiLetter());
            Assert.True('Z'.IsAsciiLetter());
            Assert.False('1'.IsAsciiLetter());
            Assert.False('\u00F1'.IsAsciiLetter()); // ñ should not be ASCII letter
        }

        [Fact]
        public void IsAsciiLetterUpper_And_Lower_Work()
        {
            Assert.True('A'.IsAsciiLetterUpper());
            Assert.False('a'.IsAsciiLetterUpper());

            Assert.True('a'.IsAsciiLetterLower());
            Assert.False('A'.IsAsciiLetterLower());
        }
    }
}
