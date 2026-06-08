using System;
using System.Numerics;
using Xunit;
using Cool;
using System.Runtime.InteropServices;

namespace Cool.Tests
{
    public class UncheckedEqualsTests
    {
        [Fact]
        public void Equals_Bytes_BasicTrueAndFalse()
        {
            byte[] a = new byte[] { 1, 2, 3, 4, 5, 6 };
            byte[] b = new byte[] { 1, 2, 3, 4, 5, 6 };
            byte[] c = new byte[] { 1, 2, 9, 4, 5, 6 };

            Assert.True(Unchecked.Equals(ref a.GetReference(), ref b.GetReference(), (nuint)a.Length));
            Assert.False(Unchecked.Equals(ref a.GetReference(), ref c.GetReference(), (nuint)a.Length));
        }

        [Fact]
        public void Equals_Bytes_ZeroElements_ReturnsTrue()
        {
            byte[] a = new byte[] { 10, 20, 30 };
            byte[] b = new byte[] { 99, 98, 97 };

            Assert.True(Unchecked.Equals(ref a.GetReference(), ref b.GetReference(), (nuint)0));
        }

        [Fact]
        public void Equals_UInt64_PrefixAndSingleElementBehavior()
        {
            ulong[] left = new ulong[] { 1UL, 2UL, 3UL, 4UL };
            ulong[] right = new ulong[] { 1UL, 2UL, 9UL, 9UL };

            // first two elements equal -> prefix compare should be true
            Assert.True(Unchecked.Equals(ref left.GetReference(), ref right.GetReference(), (nuint)2));

            // full compare should be false
            Assert.False(Unchecked.Equals(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length));

            // single-element compare compares directly
            ulong[] s1 = new ulong[] { 5UL };
            ulong[] s2 = new ulong[] { 9UL };
            Assert.False(Unchecked.Equals(ref s1.GetReference(), ref s2.GetReference(), (nuint)1));
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MyBigPixel
        {
            public ulong R;
            public ulong G;
            public ulong B;
        }

        [Fact]
        public void Equals_GenericStruct_Elementwise()
        {
            int len = 4;
            MyBigPixel[] left = new MyBigPixel[len];
            MyBigPixel[] right = new MyBigPixel[len];

            for (int i = 0; i < len; i++)
            {
                left[i].R = (ulong)(i + 1);
                left[i].G = 0xAAAAAAAAAAAAAAAA;
                left[i].B = 0x5555555555555555;

                right[i] = left[i];
            }

            // equal arrays -> equality
            Assert.True(Unchecked.Equals(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length));

            // change one field -> inequality
            right[2].R = 0xCAFEBABECAFEBABE;
            Assert.False(Unchecked.Equals(ref left.GetReference(), ref right.GetReference(), (nuint)left.Length));
        }

        [Fact]
        public void Equals_Bytes_SlowAndFastPaths()
        {
            int vCount = Vector<byte>.Count;

            // small length (likely software fallback when vectorized)
            int smallLen = Math.Max(1, vCount - 1);
            byte[] a = new byte[smallLen];
            byte[] b = new byte[smallLen];
            for (int i = 0; i < smallLen; i++) a[i] = b[i] = (byte)(i * 7 + 3);

            Assert.True(Unchecked.Equals(ref a.GetReference(), ref b.GetReference(), (nuint)smallLen));
            b[smallLen - 1] ^= 0xFF;
            Assert.False(Unchecked.Equals(ref a.GetReference(), ref b.GetReference(), (nuint)smallLen));

            // larger length to exercise vectorized loop when available
            int bigLen = vCount * 2 + 1;
            byte[] a2 = new byte[bigLen];
            byte[] b2 = new byte[bigLen];
            for (int i = 0; i < bigLen; i++) a2[i] = b2[i] = (byte)(i * 13 + 17);

            Assert.True(Unchecked.Equals(ref a2.GetReference(), ref b2.GetReference(), (nuint)bigLen));

            // mismatches at different offsets
            b2[0] ^= 0x80; // beginning
            Assert.False(Unchecked.Equals(ref a2.GetReference(), ref b2.GetReference(), (nuint)bigLen));
            b2[0] ^= 0x80; // restore

            b2[vCount] ^= 0x40; // inside second vector compared in loop
            Assert.False(Unchecked.Equals(ref a2.GetReference(), ref b2.GetReference(), (nuint)bigLen));
        }

        [Fact]
        public void Equals_UInt32_And_UInt16_BasicCases()
        {
            // uint
            uint[] ua = new uint[10];
            uint[] ub = new uint[10];
            for (int i = 0; i < ua.Length; i++) ua[i] = ub[i] = (uint)(i * 0x1234567);
            Assert.True(Unchecked.Equals(ref ua.GetReference(), ref ub.GetReference(), (nuint)ua.Length));
            ub[3] = ub[3] ^ 0xFFFFFFFFu;
            Assert.False(Unchecked.Equals(ref ua.GetReference(), ref ub.GetReference(), (nuint)ua.Length));

            // single-element behavior
            uint[] s1 = new uint[] { 7u };
            uint[] s2 = new uint[] { 8u };
            Assert.False(Unchecked.Equals(ref s1.GetReference(), ref s2.GetReference(), (nuint)1));

            // ushort
            ushort[] sa = new ushort[7];
            ushort[] sb = new ushort[7];
            for (int i = 0; i < sa.Length; i++) sa[i] = sb[i] = (ushort)(i * 11 + 1);
            Assert.True(Unchecked.Equals(ref sa.GetReference(), ref sb.GetReference(), (nuint)sa.Length));
            sb[6]++;
            Assert.False(Unchecked.Equals(ref sa.GetReference(), ref sb.GetReference(), (nuint)sa.Length));
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
        public void Equals_Generic_TakesCorrectBranch_PerStructSize()
        {
            // Size 3 -> falls back to byte comparison
            int n = 6;
            Sized3[] a3 = new Sized3[n];
            Sized3[] b3 = new Sized3[n];
            for (int i = 0; i < n; i++) { a3[i].A = b3[i].A = (byte)i; a3[i].B = b3[i].B = (byte)(i + 1); a3[i].C = b3[i].C = (byte)(i + 2); }
            Assert.True(Unchecked.Equals(ref a3.GetReference(), ref b3.GetReference(), (nuint)n));
            b3[2].B ^= 0xFF;
            Assert.False(Unchecked.Equals(ref a3.GetReference(), ref b3.GetReference(), (nuint)n));

            // Size 2 -> ushort path
            Sized2[] a2 = new Sized2[n];
            Sized2[] b2 = new Sized2[n];
            for (int i = 0; i < n; i++) a2[i].A = b2[i].A = (ushort)(i * 7 + 3);
            Assert.True(Unchecked.Equals(ref a2.GetReference(), ref b2.GetReference(), (nuint)n));
            b2[1].A++;
            Assert.False(Unchecked.Equals(ref a2.GetReference(), ref b2.GetReference(), (nuint)n));

            // Size 4 -> uint path
            Sized4[] a4 = new Sized4[n];
            Sized4[] b4 = new Sized4[n];
            for (int i = 0; i < n; i++) a4[i].A = b4[i].A = (uint)(i * 0xABCDEF);
            Assert.True(Unchecked.Equals(ref a4.GetReference(), ref b4.GetReference(), (nuint)n));
            b4[3].A = ~b4[3].A;
            Assert.False(Unchecked.Equals(ref a4.GetReference(), ref b4.GetReference(), (nuint)n));

            // Size 16 -> ulong path
            Sized16[] a16 = new Sized16[n];
            Sized16[] b16 = new Sized16[n];
            for (int i = 0; i < n; i++) { a16[i].A = b16[i].A = (ulong)i; a16[i].B = b16[i].B = (ulong)(i * 3); }
            Assert.True(Unchecked.Equals(ref a16.GetReference(), ref b16.GetReference(), (nuint)n));
            b16[4].B = 0xDEADBEEFDEADBEEF;
            Assert.False(Unchecked.Equals(ref a16.GetReference(), ref b16.GetReference(), (nuint)n));
        }
    }
}
