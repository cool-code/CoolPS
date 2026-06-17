using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class UncheckedCopyTests
    {
        [Fact]
        public void Copy_Bytes_NonOverlapping_CopiesAll()
        {
            int len = 32;
            byte[] left = new byte[len];
            byte[] right = new byte[len];
            for (int i = 0; i < len; i++) right[i] = (byte)(i * 7 + 3);

            Unchecked.Copy(ref left.GetReference(), ref right.GetReference(), len);

            Assert.Equal(right, left);
        }

        [Fact]
        public void Copy_Bytes_Overlap_DestBeforeSource()
        {
            byte[] arr = new byte[20];
            for (int i = 0; i < arr.Length; i++) arr[i] = (byte)i;
            var original = (byte[])arr.Clone();

            int dest = 0, src = 2, count = 8;
            // expected: copy original[src..src+count) into positions starting at dest
            var expected = (byte[])original.Clone();
            for (int i = 0; i < count; i++) expected[dest + i] = original[src + i];

            Unchecked.Copy(ref Unsafe.Add(ref arr.GetReference(), dest), ref Unsafe.Add(ref arr.GetReference(), src), count);

            Assert.Equal(expected, arr);
        }

        [Fact]
        public void Copy_Bytes_Overlap_DestAfterSource()
        {
            byte[] arr = new byte[20];
            for (int i = 0; i < arr.Length; i++) arr[i] = (byte)i;
            var original = (byte[])arr.Clone();

            int dest = 2, src = 0, count = 8;
            var expected = (byte[])original.Clone();
            for (int i = 0; i < count; i++) expected[dest + i] = original[src + i];

            Unchecked.Copy(ref Unsafe.Add(ref arr.GetReference(), dest), ref Unsafe.Add(ref arr.GetReference(), src), count);

            Assert.Equal(expected, arr);
        }

        [Fact]
        public void Copy_Ulong_Overlap_CopiesElements()
        {
            ulong[] arr = new ulong[] { 0UL, 1UL, 2UL, 3UL, 4UL, 5UL };
            var original = (ulong[])arr.Clone();

            // copy 2 elements from index 0 -> index 2 (dest after source)
            Unchecked.Copy(ref Unsafe.Add(ref arr.GetReference(), 2), ref Unsafe.Add(ref arr.GetReference(), 0), 2);

            var expected = (ulong[])original.Clone();
            expected[2] = original[0];
            expected[3] = original[1];

            Assert.Equal(expected, arr);

            // now copy 3 elements from index 2 -> index 0 (dest before source)
            arr = new ulong[] { 0UL, 1UL, 2UL, 3UL, 4UL, 5UL };
            original = (ulong[])arr.Clone();
            Unchecked.Copy(ref Unsafe.Add(ref arr.GetReference(), 0), ref Unsafe.Add(ref arr.GetReference(), 2), 3);
            expected = (ulong[])original.Clone();
            for (int i = 0; i < 3; i++) expected[0 + i] = original[2 + i];

            Assert.Equal(expected, arr);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Sized3 { public byte A; public byte B; public byte C; }

        [Fact]
        public void Copy_Generic_Struct_Overlap_Works()
        {
            int n = 6;
            Sized3[] arr = new Sized3[n];
            for (int i = 0; i < n; i++) { arr[i].A = (byte)(i + 1); arr[i].B = (byte)(i + 2); arr[i].C = (byte)(i + 3); }
            var original = (Sized3[])arr.Clone();

            // copy 2 elements from index 0 -> index 2 (overlap dest after source)
            Unchecked.Copy(ref Unsafe.Add(ref arr.GetReference(), 2), ref Unsafe.Add(ref arr.GetReference(), 0), 2);

            var expected = (Sized3[])original.Clone();
            expected[2] = original[0];
            expected[3] = original[1];

            for (int i = 0; i < n; i++)
            {
                Assert.Equal(expected[i].A, arr[i].A);
                Assert.Equal(expected[i].B, arr[i].B);
                Assert.Equal(expected[i].C, arr[i].C);
            }
        }

        [Fact]
        public void Copy_Bytes_Overlap_ProblematicLengths_DestBeforeSource()
        {
            int[] lengths = new int[] { 17, 18, 31, 32, 63, 64, 127, 128, 255, 256, 1023, 1024, 2048 };
            foreach (int count in lengths)
            {
                int src = 1, dest = 0;
                byte[] arr = new byte[count + src + 2];
                for (int i = 0; i < arr.Length; i++) arr[i] = (byte)((i * 31 + 17) & 0xFF);
                var original = (byte[])arr.Clone();

                var expected = (byte[])original.Clone();
                for (int i = 0; i < count; i++) expected[dest + i] = original[src + i];

                Unchecked.Copy(ref Unsafe.Add(ref arr.GetReference(), dest), ref Unsafe.Add(ref arr.GetReference(), src), count);

                Assert.Equal(expected, arr);
            }
        }

        [Fact]
        public void Copy_Bytes_Overlap_ProblematicLengths_DestAfterSource()
        {
            int[] lengths = new int[] { 17, 18, 31, 32, 63, 64, 127, 128, 255, 256, 1023, 1024, 2048 };
            foreach (int count in lengths)
            {
                int src = 0, dest = 1;
                byte[] arr = new byte[count + dest + 2];
                for (int i = 0; i < arr.Length; i++) arr[i] = (byte)((i * 31 + 17) & 0xFF);
                var original = (byte[])arr.Clone();

                var expected = (byte[])original.Clone();
                for (int i = 0; i < count; i++) expected[dest + i] = original[src + i];

                Unchecked.Copy(ref Unsafe.Add(ref arr.GetReference(), dest), ref Unsafe.Add(ref arr.GetReference(), src), count);

                Assert.Equal(expected, arr);
            }
        }
    }
}
