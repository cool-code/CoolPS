using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint FastGetHashCode(ref byte src, nuint length, ref uint p0, ref uint p1)
    {
        nuint offset = 0;
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            ref Vector<uint> vSrc = ref Unsafe.As<byte, Vector<uint>>(ref src);
            nuint stride = (nuint)Vector<uint>.Count * sizeof(uint);

            var vHash0 = new Vector<uint>(p0);
            var vHash1 = new Vector<uint>(p1);

            var vDivisor = new Vector<uint>(524288u);

            nuint stopLoopAtOffset = length & ~(stride * 2 - 1);
            for (; offset < stopLoopAtOffset; offset += stride * 2)
            {
                var vData1 = Unsafe.AddByteOffset(ref vSrc, offset);
                vHash0 += vData1; vHash1 ^= vHash0;
                vHash0 = Vector.BitwiseOr(vHash0 * 8192u, vHash0 / vDivisor);

                var vData2 = Unsafe.AddByteOffset(ref vSrc, offset + stride);
                vHash0 += vData2; vHash1 ^= vHash0;
                vHash0 = Vector.BitwiseOr(vHash0 * 8192u, vHash0 / vDivisor);
            }

            if ((length & stride) != 0)
            {
                var vData = Unsafe.AddByteOffset(ref vSrc, offset);
                vHash0 += vData; vHash1 ^= vHash0;
                vHash0 = Vector.BitwiseOr(vHash0 * 8192u, vHash0 / vDivisor);
                offset += stride;
            }

            uint f0 = 0; uint f1 = 0;
            for (int i = 0; i < Vector<uint>.Count; i++)
            {
                f0 += vHash0[i]; f1 ^= vHash1[i];
            }
            p0 = f0; p1 = f1;
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe nuint SlowGetHashCode(ref ulong src, nuint length, nuint offset, ref uint p0, ref uint p1, out nuint remaining)
    {
        ref nuint nsrc = ref Unsafe.As<ulong, nuint>(ref src);
        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            nuint data1 = Unsafe.AddByteOffset(ref nsrc, offset);
            p0 += (uint)data1; p1 ^= p0; p0 = (p0 << 13) | (p0 >> 19);
            if (sizeof(nuint) == 8) { p0 += (uint)(data1 >> 32); p1 ^= p0; p0 = (p0 << 13) | (p0 >> 19); }

            nuint data2 = Unsafe.AddByteOffset(ref nsrc, offset + (nuint)sizeof(nuint));
            p0 += (uint)data2; p1 ^= p0; p0 = (p0 << 13) | (p0 >> 19);
            if (sizeof(nuint) == 8) { p0 += (uint)(data2 >> 32); p1 ^= p0; p0 = (p0 << 13) | (p0 >> 19); }
        }

        remaining = length - offset;
        if ((remaining & sizeof(ulong)) != 0)
        {
            ulong data = Unsafe.AddByteOffset(ref src, offset);
            p0 += (uint)data; p1 ^= p0; p0 = (p0 << 13) | (p0 >> 19);
            p0 += (uint)(data >> 32); p1 ^= p0; p0 = (p0 << 13) | (p0 >> 19);
            offset += sizeof(ulong);
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint SlowGetHashCode(ref uint src, nuint length, nuint offset, ref uint p0, ref uint p1, out nuint remaining)
    {
        offset = SlowGetHashCode(ref Unsafe.As<uint, ulong>(ref src), length, offset, ref p0, ref p1, out remaining);
        if ((remaining & sizeof(uint)) != 0)
        {
            uint data = Unsafe.AddByteOffset(ref src, offset);
            p0 += data; p1 ^= p0; p0 = (p0 << 13) | (p0 >> 19);
            offset += sizeof(uint);
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint SlowGetHashCode(ref ushort src, nuint length, nuint offset, ref uint p0, ref uint p1, out nuint remaining)
    {
        offset = SlowGetHashCode(ref Unsafe.As<ushort, uint>(ref src), length, offset, ref p0, ref p1, out remaining);
        if ((remaining & sizeof(ushort)) != 0)
        {
            ushort data = Unsafe.AddByteOffset(ref src, offset);
            p0 += data; p1 ^= p0; p0 = (p0 << 13) | (p0 >> 19);
            offset += sizeof(ushort);
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowGetHashCode(ref byte src, nuint length, nuint offset, ref uint p0, ref uint p1)
    {
        offset = SlowGetHashCode(ref Unsafe.As<byte, ushort>(ref src), length, offset, ref p0, ref p1, out nuint remaining);
        if ((remaining & sizeof(byte)) != 0)
        {
            byte data = Unsafe.AddByteOffset(ref src, offset);
            p0 += data; p1 ^= p0; p0 = (p0 << 13) | (p0 >> 19);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode(ref ulong src, nuint numElements)
    {
        if (numElements == 0) return 0;
        uint p0 = 0x73616d70; uint p1 = 0x656c696e;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ulong>();
        nuint offset = FastGetHashCode(ref Unsafe.As<ulong, byte>(ref src), length, ref p0, ref p1);
        if (offset < length) SlowGetHashCode(ref src, length, offset, ref p0, ref p1, out _);
        return (int)(p0 ^ p1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode(ref uint src, nuint numElements)
    {
        if (numElements == 0) return 0;
        uint p0 = 0x73616d70; uint p1 = 0x656c696e;
        nuint length = numElements * (nuint)Unsafe.SizeOf<uint>();
        nuint offset = FastGetHashCode(ref Unsafe.As<uint, byte>(ref src), length, ref p0, ref p1);
        if (offset < length) SlowGetHashCode(ref src, length, offset, ref p0, ref p1, out _);
        return (int)(p0 ^ p1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode(ref ushort src, nuint numElements)
    {
        if (numElements == 0) return 0;
        uint p0 = 0x73616d70; uint p1 = 0x656c696e;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ushort>();
        nuint offset = FastGetHashCode(ref Unsafe.As<ushort, byte>(ref src), length, ref p0, ref p1);
        if (offset < length) SlowGetHashCode(ref src, length, offset, ref p0, ref p1, out _);
        return (int)(p0 ^ p1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode(ref byte src, nuint length)
    {
        if (length == 0) return 0;
        uint p0 = 0x73616d70; uint p1 = 0x656c696e;
        nuint offset = FastGetHashCode(ref src, length, ref p0, ref p1);
        if (offset < length) SlowGetHashCode(ref src, length, offset, ref p0, ref p1);
        return (int)(p0 ^ p1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(ref T src, nuint numElements) where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() % sizeof(ulong) == 0)
        {
            return GetHashCode(ref Unsafe.As<T, ulong>(ref src), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ulong)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(uint) == 0)
        {
            return GetHashCode(ref Unsafe.As<T, uint>(ref src), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(uint)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(ushort) == 0)
        {
            return GetHashCode(ref Unsafe.As<T, ushort>(ref src), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ushort)));
        }
        else
        {
            return GetHashCode(ref Unsafe.As<T, byte>(ref src), numElements * (nuint)Unsafe.SizeOf<T>());
        }
    }
}
