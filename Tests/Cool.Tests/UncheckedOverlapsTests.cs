using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class UncheckedOverlapsTests
    {
        [Fact]
        public void Overlaps_ZeroElements_ReturnsFalse()
        {
            byte[] left = new byte[] { 0xFF, 0x01 };
            byte[] right = new byte[] { 0xFF, 0x01 };

            bool result = Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)0);

            Assert.False(result);
        }

        [Fact]
        public void Overlaps_Single_Byte_TrueAndFalse()
        {
            byte[] a = new byte[] { 0b0011 };
            byte[] b = new byte[] { 0b0101 };
            Assert.True(Unchecked.Overlaps(ref a.GetReference(), ref b.GetReference(), (nuint)1));

            a[0] = 0b1000; b[0] = 0b0111;
            Assert.False(Unchecked.Overlaps(ref a.GetReference(), ref b.GetReference(), (nuint)1));
        }

        [Fact]
        public void Overlaps_Bytes_FindsNonPrefixOverlap()
        {
            int len = Vector<byte>.Count + 20; // ensure mixed vector + scalar code paths
            byte[] left = new byte[len];
            byte[] right = new byte[len];

            // default zeros -> no overlap at start
            int mid = len / 2;
            left[mid] = 0x80;
            right[mid] = 0x80;

            // whole-range should detect the overlap
            Assert.True(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length));

            // but single-element (prefix only) should be false
            Assert.False(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)1));
        }

        [Fact]
        public void Overlaps_Ulong_PrefixAndLaterOverlap()
        {
            // first element has no overlapping bits, second element has
            ulong[] left = new ulong[] { 0UL, 1UL << 63 };
            ulong[] right = new ulong[] { 0UL, 1UL << 63 };

            // checking two elements should find the overlap in the second element
            Assert.True(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)2));

            // single-element compare (only first) should be false
            Assert.False(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)1));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MyBigPixel
        {
            public ulong R;
            public ulong G;
            public ulong B;
        }

        [Fact]
        public void Overlaps_GenericStruct_Elementwise()
        {
            int len = 3;
            MyBigPixel[] left = new MyBigPixel[len];
            MyBigPixel[] right = new MyBigPixel[len];

            for (int i = 0; i < len; i++)
            {
                left[i].R = 0UL;
                left[i].G = 0UL;
                left[i].B = 0UL;

                right[i].R = 0UL;
                right[i].G = 0UL;
                right[i].B = 0UL;
            }

            // inject overlap into the second element's G field
            left[1].G = 0x1000UL;
            right[1].G = 0x1000UL;

            Assert.True(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length));
            // only first element -> no overlap
            Assert.False(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)1));
        }

        [Fact]
        public void Overlaps_FirstElement_ImmediateReturn()
        {
            byte[] left = new byte[] { 0x10, 0x00, 0x00 };
            byte[] right = new byte[] { 0x10, 0xFF, 0xFF };

            // first element overlaps -> immediate true regardless of length
            Assert.True(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length));
            Assert.True(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)1));
        }

        [Fact]
        public void Overlaps_Vector_TwoVectors_Middle()
        {
            int v = Vector<byte>.Count;
            int len = v * 2;
            byte[] left = new byte[len];
            byte[] right = new byte[len];

            // put overlap at the start of the second vector block
            left[v] = 0x01;
            right[v] = 0x01;

            Assert.True(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)len));
            // first element still zero -> single-element check false
            Assert.False(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)1));
        }

        [Fact]
        public void Overlaps_Bytes_SlowPath_TrailingByte()
        {
            // ensure length is smaller than vector count to exercise slow path
            int len = Math.Max(1, Vector<byte>.Count - 1);
            byte[] left = new byte[len];
            byte[] right = new byte[len];

            left[len - 1] = 0x80;
            right[len - 1] = 0x80;

            Assert.True(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)len));
            Assert.False(Unchecked.Overlaps(ref left.GetReference(), ref right.GetReference(), (nuint)1));
        }

        [Fact]
        public void Overlaps_UInt_UShort_Branches()
        {
            uint[] ua = new uint[] { 0u, 1u << 31 };
            uint[] ub = new uint[] { 0u, 1u << 31 };
            Assert.True(Unchecked.Overlaps(ref ua.GetReference(), ref ub.GetReference(), (nuint)2));
            Assert.False(Unchecked.Overlaps(ref ua.GetReference(), ref ub.GetReference(), (nuint)1));

            ushort[] sa = new ushort[] { 0, 0x8000 };
            ushort[] sb = new ushort[] { 0, 0x8000 };
            Assert.True(Unchecked.Overlaps(ref sa.GetReference(), ref sb.GetReference(), (nuint)2));
            Assert.False(Unchecked.Overlaps(ref sa.GetReference(), ref sb.GetReference(), (nuint)1));
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Sized3 { public byte A; public byte B; public byte C; }

        [Fact]
        public void Overlaps_Generic_3ByteStruct_Branch()
        {
            int n = 4;
            Sized3[] a = new Sized3[n];
            Sized3[] b = new Sized3[n];
            a[2].B = 0x10; b[2].B = 0x10;

            Assert.True(Unchecked.Overlaps(ref a.GetReference(), ref b.GetReference(), (nuint)n));
            Assert.False(Unchecked.Overlaps(ref a.GetReference(), ref b.GetReference(), (nuint)1));
        }
    }
}
