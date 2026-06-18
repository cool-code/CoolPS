using System;
using System.Runtime.CompilerServices;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class ClearTests
    {
        [Fact]
        public void Clear_Bytes_Equals_UnsafeInitBlock()
        {
            int n = 1024 * 1024; // 1MB
            byte[] a = new byte[n];
            byte[] b = new byte[n];
            for (int i = 0; i < n; i++)
            {
                byte v = (byte)(i & 0xFF);
                a[i] = v;
                b[i] = v;
            }

            Unchecked.Clear(ref a[0], n);
            Unsafe.InitBlockUnaligned(ref b[0], 0, (nuint)n);

            Assert.Equal(b, a);
        }

        [Fact]
        public void Clear_Ulongs_Equals_UnsafeInitBlock()
        {
            int n = 128 * 1024; // 128k * 8 = 1MB
            ulong[] a = new ulong[n];
            ulong[] b = new ulong[n];
            for (int i = 0; i < n; i++)
            {
                ulong v = ((ulong)(uint)i * 6364136223846793005UL) ^ 0x9E3779B97F4A7C15UL;
                a[i] = v;
                b[i] = v;
            }

            Unchecked.Clear(ref a[0], n);
            Unsafe.InitBlockUnaligned(ref Unsafe.As<ulong, byte>(ref b[0]), 0, (nuint)n * sizeof(ulong));

            Assert.Equal(b, a);
        }

        [Fact]
        public void Clear_ObjectArray_Equals_ArrayClear()
        {
            int n = 8192;
            object[] a = new object[n];
            object[] b = new object[n];
            for (int i = 0; i < n; i++)
            {
                object v = (i % 2 == 0) ? (object)("str" + i) : new object();
                a[i] = v;
                b[i] = v;
            }

            Unchecked.Clear(ref a[0], n);
            Array.Clear(b, 0, n);

            Assert.Equal(b, a);
        }

        [Fact]
        public void Clear_BlittableStructArray_Equals_UnsafeInitBlock()
        {
            int n = 16384;
            BlittableStruct[] a = new BlittableStruct[n];
            BlittableStruct[] b = new BlittableStruct[n];
            for (int i = 0; i < n; i++)
            {
                a[i].A = i;
                a[i].B = ((long)i << 32) | (uint)i;
                b[i] = a[i];
            }

            Unchecked.Clear(ref a[0], n);
            Unsafe.InitBlockUnaligned(ref Unsafe.As<BlittableStruct, byte>(ref b[0]), 0, (nuint)n * (nuint)Unsafe.SizeOf<BlittableStruct>());

            Assert.Equal(b, a);
        }

        [Fact]
        public void Clear_StructWithReference_Equals_ArrayClear()
        {
            int n = 8192;
            WithRefStruct[] a = new WithRefStruct[n];
            WithRefStruct[] b = new WithRefStruct[n];
            for (int i = 0; i < n; i++)
            {
                a[i].X = i;
                a[i].O = "s" + i;
                b[i] = a[i];
            }

            Unchecked.Clear(ref a[0], n);
            Array.Clear(b, 0, n);

            Assert.Equal(b, a);
        }
    }

    internal struct BlittableStruct
    {
        public int A;
        public long B;
    }

    internal struct WithRefStruct
    {
        public int X;
        public object? O;
    }
}
