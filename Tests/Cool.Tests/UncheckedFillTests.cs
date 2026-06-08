using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class UncheckedFillTests
    {
        [Fact]
        public void Fill_ZeroElements_NoOp()
        {
            int[] arr = new int[] { 5, 6, 7 };
            var original = (int[])arr.Clone();

            int v = 42;
            Unchecked.Fill(ref arr.GetReference(), (nuint)0, in v);

            Assert.Equal(original, arr);
        }

        [Fact]
        public void Fill_Bytes_FillsAll()
        {
            int len = 33;
            byte[] arr = new byte[len];
            byte fill = 0xAB;

            Unchecked.Fill(ref arr.GetReference(), (nuint)arr.Length, in fill);

            for (int i = 0; i < len; i++) Assert.Equal(fill, arr[i]);
        }

        [Fact]
        public void Fill_UInt_SlowAndFast_PopulatesCorrectly()
        {
            int vectorSize = Vector<byte>.Count;
            int elemSize = 4;
            int threshold = Math.Max(vectorSize / elemSize, 1);

            // slow path (less than threshold)
            int slowLen = Math.Max(threshold - 1, 1);
            uint[] slow = new uint[slowLen + 2]; // extra to ensure no overrun
            uint v1 = 0xDEADBEEFu;
            Unchecked.Fill(ref slow.GetReference(), (nuint)slowLen, in v1);
            for (int i = 0; i < slowLen; i++) Assert.Equal(v1, slow[i]);

            // fast path (>= threshold)
            int fastLen = threshold + 2;
            uint[] fast = new uint[fastLen + 2];
            uint v2 = 0x12345678u;
            Unchecked.Fill(ref fast.GetReference(), (nuint)fastLen, in v2);
            for (int i = 0; i < fastLen; i++) Assert.Equal(v2, fast[i]);
        }

        [Fact]
        public void Fill_Ulong_SlowAndFast_PopulatesCorrectly()
        {
            int vectorSize = Vector<byte>.Count;
            int elemSize = 8;
            int threshold = Math.Max(vectorSize / elemSize, 1);

            int slowLen = Math.Max(threshold - 1, 1);
            ulong[] slow = new ulong[slowLen + 1];
            ulong vs = 0xCAFEBABECAFEBABEu;
            Unchecked.Fill(ref slow.GetReference(), (nuint)slowLen, in vs);
            for (int i = 0; i < slowLen; i++) Assert.Equal(vs, slow[i]);

            int fastLen = threshold + 3;
            ulong[] fast = new ulong[fastLen + 1];
            ulong vf = 0x0F0F0F0F0F0F0F0Ful;
            Unchecked.Fill(ref fast.GetReference(), (nuint)fastLen, in vf);
            for (int i = 0; i < fastLen; i++) Assert.Equal(vf, fast[i]);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ThreeBytes
        {
            public byte A;
            public byte B;
            public byte C;
        }

        [Fact]
        public void Fill_CustomNonPow2_SlowPath_PopulatesFields()
        {
            int len = 7;
            ThreeBytes[] arr = new ThreeBytes[len];
            ThreeBytes value = new ThreeBytes { A = 1, B = 2, C = 3 };

            Unchecked.Fill(ref arr.GetReference(), (nuint)arr.Length, in value);

            for (int i = 0; i < len; i++)
            {
                Assert.Equal(value.A, arr[i].A);
                Assert.Equal(value.B, arr[i].B);
                Assert.Equal(value.C, arr[i].C);
            }
        }
    }
}
