using System;
using Xunit;
using Cool;
using System.Runtime.InteropServices;

namespace Cool.Tests
{
    public class UncheckedNotTests
    {
        [Fact]
        public void Not_Bytes_InvertsAll()
        {
            byte[] arr = new byte[] { 0x00, 0xFF, 0xAA, 0x55, 0x12, 0x34 };
            var expected = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++) expected[i] = (byte)~arr[i];

            Unchecked.Not(ref arr.GetReference(), (nuint)arr.Length);

            for (int i = 0; i < arr.Length; i++) Assert.Equal(expected[i], arr[i]);
        }

        [Fact]
        public void Not_UInt32_InvertsAll()
        {
            uint[] arr = new uint[] { 0u, 0x12345678u, 0xFFFFFFFFu, 0x0u };
            var expected = new uint[arr.Length];
            for (int i = 0; i < arr.Length; i++) expected[i] = ~arr[i];

            Unchecked.Not(ref arr.GetReference(), (nuint)arr.Length);

            for (int i = 0; i < arr.Length; i++) Assert.Equal(expected[i], arr[i]);
        }

        [Fact]
        public void Not_Partial_InvertsPrefixOnly()
        {
            ulong[] arr = new ulong[] { 1UL, 2UL, 3UL, 4UL };
            var original = (ulong[])arr.Clone();

            Unchecked.Not(ref arr.GetReference(), (nuint)2);

            Assert.Equal(~original[0], arr[0]);
            Assert.Equal(~original[1], arr[1]);
            Assert.Equal(original[2], arr[2]);
            Assert.Equal(original[3], arr[3]);
        }

        [Fact]
        public void Not_ZeroElements_NoOp()
        {
            int[] arr = new int[] { 5, 6, 7 };
            var original = (int[])arr.Clone();

            Unchecked.Not(ref arr.GetReference(), (nuint)0);

            Assert.Equal(original, arr);
        }

        [Fact]
        public void Not_LongByteArray_NonPowerOfTwoLength_InvertsAll()
        {
            // 200 bytes (not a power of two, > 128)
            int len = 200;
            byte[] arr = new byte[len];
            for (int i = 0; i < len; i++) arr[i] = (byte)(i * 7 + 13);

            var expected = new byte[len];
            for (int i = 0; i < len; i++) expected[i] = (byte)~arr[i];

            Unchecked.Not(ref arr.GetReference(), (nuint)arr.Length);

            Assert.Equal(expected, arr);
        }

        [Fact]
        public void Not_UlongArray_25Elements_200Bytes_InvertsAll()
        {
            // 25 * 8 = 200 bytes, non power-of-two element count
            int len = 25;
            ulong[] arr = new ulong[len];
            for (int i = 0; i < len; i++) arr[i] = (ulong)(i * 0x12345678u + 0xCAFEBABE);

            var expected = new ulong[len];
            for (int i = 0; i < len; i++) expected[i] = ~arr[i];

            Unchecked.Not(ref arr.GetReference(), (nuint)arr.Length);

            for (int i = 0; i < len; i++) Assert.Equal(expected[i], arr[i]);
        }

        [Theory]
        [InlineData(33)]
        [InlineData(47)]
        [InlineData(63)]
        public void Not_Bytes_SlightlyLargerThanSingleVector_InvertsAll(int len)
        {
            byte[] arr = new byte[len];
            for (int i = 0; i < len; i++) arr[i] = (byte)(i ^ 0x55);

            var expected = new byte[len];
            for (int i = 0; i < len; i++) expected[i] = (byte)~arr[i];

            Unchecked.Not(ref arr.GetReference(), (nuint)arr.Length);

            Assert.Equal(expected, arr);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(9)]
        [InlineData(15)]
        [InlineData(31)]
        [InlineData(65)]
        public void Not_Bytes_SoftwareFallbackEdgeCases_InvertsAll(int len)
        {
            byte[] arr = new byte[len];
            for (int i = 0; i < len; i++) arr[i] = (byte)(i * 3 + 7);

            var expected = new byte[len];
            for (int i = 0; i < len; i++) expected[i] = (byte)~arr[i];

            Unchecked.Not(ref arr.GetReference(), (nuint)arr.Length);

            Assert.Equal(expected, arr);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MyBigPixel
        {
            public ulong R;
            public ulong G;
            public ulong B;
        }

        [Fact]
        public void Not_CustomUnmanagedStruct_InvertsAll()
        {
            int len = 5;
            MyBigPixel[] arr = new MyBigPixel[len];
            for (int i = 0; i < len; i++)
            {
                arr[i].R = (ulong)i;
                arr[i].G = 0xAAAAAAAAAAAAAAAA;
                arr[i].B = 0x5555555555555555;
            }

            var expected = new MyBigPixel[len];
            for (int i = 0; i < len; i++)
            {
                expected[i].R = ~arr[i].R;
                expected[i].G = ~arr[i].G;
                expected[i].B = ~arr[i].B;
            }

            Unchecked.Not(ref arr.GetReference(), (nuint)arr.Length);

            for (int i = 0; i < len; i++)
            {
                Assert.Equal(expected[i].R, arr[i].R);
                Assert.Equal(expected[i].G, arr[i].G);
                Assert.Equal(expected[i].B, arr[i].B);
            }
        }
    }
}
