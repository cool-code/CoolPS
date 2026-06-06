using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class UncheckedPopCountTests
    {
        [Fact]
        public void PopCount_ZeroElements_ReturnsZero()
        {
            byte[] arr = new byte[] { 1, 2, 3 };

            nuint result = Unchecked.PopCount(ref arr.GetReference(), (nuint)0);

            Assert.Equal((nuint)0, result);
        }

        [Fact]
        public void PopCount_Single_Ulong_ReturnsBitCount()
        {
            ulong[] arr = new ulong[] { 0xF0F0F0F0_F0F0F0F0UL };
            nuint expected = (nuint)BitOperations.PopCount(arr[0]);

            Assert.Equal(expected, Unchecked.PopCount(ref arr.GetReference(), (nuint)1));
        }

        [Fact]
        public void PopCount_Single_UInt_ReturnsBitCount()
        {
            uint[] arr = new uint[] { 0x12345678u };
            nuint expected = (nuint)BitOperations.PopCount(arr[0]);

            Assert.Equal(expected, Unchecked.PopCount(ref arr.GetReference(), (nuint)1));
        }

        [Fact]
        public void PopCount_Single_UShort_ReturnsBitCount()
        {
            ushort[] arr = new ushort[] { 0xF0F0 };
            nuint expected = (nuint)BitOperations.PopCount(arr[0]);

            Assert.Equal(expected, Unchecked.PopCount(ref arr.GetReference(), (nuint)1));
        }

        [Fact]
        public void PopCount_Single_Byte_ReturnsBitCount()
        {
            byte[] arr = new byte[] { 0xAA };
            nuint expected = (nuint)BitOperations.PopCount(arr[0]);

            Assert.Equal(expected, Unchecked.PopCount(ref arr.GetReference(), (nuint)1));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MyBigPixelForPopCount
        {
            public ulong R;
            public ulong G;
            public ulong B;
        }

        [Fact]
        public void PopCount_Single_CustomStruct_UnsupportedSize_ReturnsZero()
        {
            MyBigPixelForPopCount[] arr = new MyBigPixelForPopCount[1];
            arr[0].R = 0xFFFFFFFFFFFFFFFF;
            arr[0].G = 0xAAAAAAAAAAAAAAAA;
            arr[0].B = 0x5555555555555555;

            nuint result = Unchecked.PopCount(ref arr.GetReference(), (nuint)1);

            Assert.Equal((nuint)(BitOperations.PopCount(arr[0].R) + BitOperations.PopCount(arr[0].G) + BitOperations.PopCount(arr[0].B)), result);
        }

        [Theory]
        [InlineData(7)]
        [InlineData(9)]
        [InlineData(15)]
        [InlineData(31)]
        [InlineData(65)]
        public void PopCount_Bytes_SoftwareFallback_CalculatesCorrectly(int len)
        {
            byte[] arr = new byte[len];
            for (int i = 0; i < len; i++) arr[i] = (byte)(i * 3 + 7);

            nuint expected = 0;
            for (int i = 0; i < len; i++) expected += (nuint)BitOperations.PopCount(arr[i]);

            Assert.Equal(expected, Unchecked.PopCount(ref arr.GetReference(), (nuint)arr.Length));
        }

        [Fact]
        public void PopCount_Bytes_Vectorized_CalculatesCorrectly()
        {
            int len = 200;
            byte[] arr = new byte[len];
            for (int i = 0; i < len; i++) arr[i] = (byte)(i * 7 + 13);

            nuint expected = 0;
            for (int i = 0; i < len; i++) expected += (nuint)BitOperations.PopCount(arr[i]);

            Assert.Equal(expected, Unchecked.PopCount(ref arr.GetReference(), (nuint)arr.Length));
        }
    }
}
