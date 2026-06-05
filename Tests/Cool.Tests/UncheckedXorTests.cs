using System;
using Xunit;
using Cool;
using System.Runtime.InteropServices;

namespace Cool.Tests
{
    public class UncheckedXorTests
    {
        [Fact]
        public void Xor_Bytes_PerformsBitwiseXorAll()
        {
            byte[] left = new byte[] { 0xFF, 0x0F, 0xAA, 0x55, 0x12, 0x34 };
            byte[] right = new byte[] { 0x0F, 0xF0, 0x0A, 0x5A, 0xFF, 0x00 };
            var expected = new byte[left.Length];
            for (int i = 0; i < left.Length; i++) expected[i] = (byte)(left[i] ^ right[i]);

            Unchecked.Xor(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length);

            Assert.Equal(expected, left);
        }

        [Theory]
        [InlineData(33)]
        [InlineData(47)]
        [InlineData(63)]
        public void Xor_Bytes_SlightlyLargerThanSingleVector_PerformsBitwiseXor(int len)
        {
            byte[] left = new byte[len];
            byte[] right = new byte[len];
            for (int i = 0; i < len; i++) { left[i] = (byte)(i ^ 0x55); right[i] = (byte)(i * 3 + 7); }

            var expected = new byte[len];
            for (int i = 0; i < len; i++) expected[i] = (byte)(left[i] ^ right[i]);

            Unchecked.Xor(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length);

            Assert.Equal(expected, left);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(9)]
        [InlineData(15)]
        [InlineData(31)]
        [InlineData(65)]
        public void Xor_Bytes_SoftwareFallbackEdgeCases_PerformsBitwiseXor(int len)
        {
            byte[] left = new byte[len];
            byte[] right = new byte[len];
            for (int i = 0; i < len; i++) { left[i] = (byte)(i * 3 + 7); right[i] = (byte)(i * 5 + 11); }

            var expected = new byte[len];
            for (int i = 0; i < len; i++) expected[i] = (byte)(left[i] ^ right[i]);

            Unchecked.Xor(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length);

            Assert.Equal(expected, left);
        }

        [Fact]
        public void Xor_UInt32_PerformsBitwiseXorAll()
        {
            uint[] left = new uint[] { 0u, 0x12345678u, 0xFFFFFFFFu, 0x0u };
            uint[] right = new uint[] { 0xFFFFFFFFu, 0x0000FFFFu, 0xAAAAAAAAu, 0x12345678u };
            var expected = new uint[left.Length];
            for (int i = 0; i < left.Length; i++) expected[i] = left[i] ^ right[i];

            Unchecked.Xor(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length);

            Assert.Equal(expected, left);
        }

        [Fact]
        public void Xor_Partial_XorPrefixOnly()
        {
            ulong[] left = new ulong[] { 1UL, 2UL, 3UL, 4UL };
            ulong[] right = new ulong[] { 0xFFFFFFFFFFFFFFFFUL, 0x0UL, 0xFFFFFFFFFFFFFFFFUL, 0x0UL };
            var originalLeft = (ulong[])left.Clone();

            Unchecked.Xor(ref left.GetReference(), ref right.GetReference(), (nuint)2);

            Assert.Equal(originalLeft[0] ^ right[0], left[0]);
            Assert.Equal(originalLeft[1] ^ right[1], left[1]);
            Assert.Equal(originalLeft[2], left[2]);
            Assert.Equal(originalLeft[3], left[3]);
        }

        [Fact]
        public void Xor_ZeroElements_NoOp()
        {
            int[] left = new int[] { 5, 6, 7 };
            int[] right = new int[] { 1, 2, 3 };
            var original = (int[])left.Clone();

            Unchecked.Xor(ref left.GetReference(), ref right.GetReference(), (nuint)0);

            Assert.Equal(original, left);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MyBigPixel
        {
            public ulong R;
            public ulong G;
            public ulong B;
        }

        [Fact]
        public void Xor_CustomUnmanagedStruct_PerformsFieldwiseXor()
        {
            int len = 5;
            MyBigPixel[] left = new MyBigPixel[len];
            MyBigPixel[] right = new MyBigPixel[len];
            for (int i = 0; i < len; i++)
            {
                left[i].R = (ulong)i;
                left[i].G = 0xAAAAAAAAAAAAAAAA;
                left[i].B = 0x5555555555555555;

                right[i].R = (ulong)(i * 3);
                right[i].G = 0x0000FFFFFFFF0000;
                right[i].B = 0x00FF00FF00FF00FF;
            }

            var expected = new MyBigPixel[len];
            for (int i = 0; i < len; i++)
            {
                expected[i].R = left[i].R ^ right[i].R;
                expected[i].G = left[i].G ^ right[i].G;
                expected[i].B = left[i].B ^ right[i].B;
            }

            Unchecked.Xor(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length);

            for (int i = 0; i < len; i++)
            {
                Assert.Equal(expected[i].R, left[i].R);
                Assert.Equal(expected[i].G, left[i].G);
                Assert.Equal(expected[i].B, left[i].B);
            }
        }
    }
}
