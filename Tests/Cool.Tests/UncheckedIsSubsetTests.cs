using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class UncheckedIsSubsetTests
    {
        [Fact]
        public void IsSubset_ZeroElements_ReturnsTrue()
        {
            int[] left = new int[] { 5, 6, 7 };
            int[] right = new int[] { 1, 2, 3 };

            bool result = Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)0);

            Assert.True(result);
        }

        [Fact]
        public void IsSubset_Single_Ulong_TrueAndFalse()
        {
            ulong[] left = new ulong[] { 0b11UL };
            ulong[] right = new ulong[] { 0b111UL };
            Assert.True(Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)1));

            left[0] = 0b1000UL; right[0] = 0b0111UL;
            Assert.False(Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)1));
        }

        [Fact]
        public void IsSubset_Single_UInt_TrueAndFalse()
        {
            uint[] left = new uint[] { 0b101u };
            uint[] right = new uint[] { 0b111u };
            Assert.True(Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)1));

            left[0] = 0b1000u; right[0] = 0b0111u;
            Assert.False(Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)1));
        }

        [Fact]
        public void IsSubset_Single_UShort_TrueAndFalse()
        {
            ushort[] left = new ushort[] { 0b1010 };
            ushort[] right = new ushort[] { 0b1111 };
            Assert.True(Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)1));

            left[0] = 0b10000; right[0] = 0b01111;
            Assert.False(Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)1));
        }

        [Fact]
        public void IsSubset_Single_Byte_TrueAndFalse()
        {
            byte[] left = new byte[] { 0b0011 };
            byte[] right = new byte[] { 0b0111 };
            Assert.True(Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)1));

            left[0] = 0b1000; right[0] = 0b0111;
            Assert.False(Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)1));
        }

        [Fact]
        public void IsSubset_Bytes_MixedPaths_ReturnsExpected()
        {
            int len = Vector<byte>.Count + 20; // ensure mixed vector + scalar work
            byte[] left = new byte[len];
            byte[] right = new byte[len];
            for (int i = 0; i < len; i++)
            {
                right[i] = 0xFF;
                left[i] = (byte)(i * 3 + 7);
            }

            // all bits in left are present in right
            Assert.True(Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length));

            // introduce a bit in left not present in right
            for (int i = 0; i < len; i++)
            {
                var l = left[i];
                var r = right[i];
                left[i] = 0x80;
                right[i] = 0x7F;
                Assert.False(Unchecked.IsSubset(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length));
                left[i] = l;
                right[i] = r;
            }
        }
    }
}
