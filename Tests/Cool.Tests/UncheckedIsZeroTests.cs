using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class UncheckedIsZeroTests
    {
        [Fact]
        public void IsZero_Bytes_ZeroLength_ReturnsTrue()
        {
            byte[] arr = new byte[] { 1, 2, 3 };
            Assert.True(Unchecked.IsZero(ref arr.GetReference(), (nuint)0));
        }

        [Fact]
        public void IsZero_Byte_SingleElement()
        {
            byte[] a = new byte[] { 0 };
            Assert.True(Unchecked.IsZero(ref a.GetReference(), (nuint)1));
            a[0] = 5;
            Assert.False(Unchecked.IsZero(ref a.GetReference(), (nuint)1));
        }

        [Fact]
        public void IsZero_Bytes_SlowPath_TrailingNonZero()
        {
            int len = Math.Max(1, Vector<byte>.Count - 1);
            byte[] left = new byte[len];
            // all zeros -> true
            Assert.True(Unchecked.IsZero(ref left.GetReference(), (nuint)left.Length));

            // trailing non-zero should make IsZero false
            left[len - 1] = 0x1;
            Assert.False(Unchecked.IsZero(ref left.GetReference(), (nuint)left.Length));
        }

        [Fact]
        public void IsZero_Bytes_VectorPath_MiddleNonZero()
        {
            int len = Vector<byte>.Count + 5;
            byte[] arr = new byte[len];
            Assert.True(Unchecked.IsZero(ref arr.GetReference(), (nuint)arr.Length));

            arr[len / 2] = 0x7;
            Assert.False(Unchecked.IsZero(ref arr.GetReference(), (nuint)arr.Length));
        }

        [Fact]
        public void IsZero_Ulong_BasicPaths()
        {
            // all-zero multi-element
            ulong[] a = new ulong[3];
            Assert.True(Unchecked.IsZero(ref a.GetReference(), (nuint)a.Length));

            // single-element zero
            ulong[] s0 = new ulong[] { 0UL };
            Assert.True(Unchecked.IsZero(ref s0.GetReference(), (nuint)1));

            // single-element non-zero
            ulong[] s1 = new ulong[] { 123UL };
            Assert.False(Unchecked.IsZero(ref s1.GetReference(), (nuint)1));

            // middle element non-zero -> false
            a[1] = 0x1UL;
            Assert.False(Unchecked.IsZero(ref a.GetReference(), (nuint)a.Length));
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Sized3 { public byte A; public byte B; public byte C; }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Sized2 { public ushort A; }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Sized4 { public uint A; }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Sized16 { public ulong A; public ulong B; }

        [Fact]
        public void IsZero_Generic_SizeBranches()
        {
            int n = 4;

            // 3-byte struct -> byte path
            Sized3[] s3 = new Sized3[n];
            Assert.True(Unchecked.IsZero(ref s3.GetReference(), (nuint)s3.Length));
            s3[2].B = 1;
            Assert.False(Unchecked.IsZero(ref s3.GetReference(), (nuint)s3.Length));

            // 2-byte struct -> ushort path
            Sized2[] s2 = new Sized2[n];
            Assert.True(Unchecked.IsZero(ref s2.GetReference(), (nuint)s2.Length));
            s2[1].A = 0x1;
            Assert.False(Unchecked.IsZero(ref s2.GetReference(), (nuint)s2.Length));

            // 4-byte struct -> uint path
            Sized4[] s4 = new Sized4[n];
            Assert.True(Unchecked.IsZero(ref s4.GetReference(), (nuint)s4.Length));
            s4[0].A = 0xDEADBEEFu;
            Assert.False(Unchecked.IsZero(ref s4.GetReference(), (nuint)s4.Length));

            // 16-byte struct -> ulong path
            Sized16[] s16 = new Sized16[n];
            Assert.True(Unchecked.IsZero(ref s16.GetReference(), (nuint)s16.Length));
            s16[3].B = 1UL;
            Assert.False(Unchecked.IsZero(ref s16.GetReference(), (nuint)s16.Length));
        }
    }
}
