#if NETFRAMEWORK
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Not(ref byte baseByteRef, nuint totalByteLength)
    {
        if (totalByteLength == 0) return;
        nuint offset = 0;
        nuint remainingBytes = totalByteLength;
        if (remainingBytes > sizeof(ulong))
        {
            if (Vector.IsHardwareAccelerated && totalByteLength >= (nuint)Vector<byte>.Count)
            {
                ref Vector<byte> v = ref Unsafe.As<byte, Vector<byte>>(ref baseByteRef);
                for (nuint stopLoopAtOffset = totalByteLength & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
                {
                    ref Vector<byte> v1 = ref Unsafe.AddByteOffset(ref v, offset); v1 = ~v1;
                    ref Vector<byte> v2 = ref Unsafe.AddByteOffset(ref v, offset + (nuint)Vector<byte>.Count); v2 = ~v2;
                }
                if ((totalByteLength & (nuint)Vector<byte>.Count) != 0)
                {
                    ref Vector<byte> vRemaining = ref Unsafe.AddByteOffset(ref v, offset);
                    vRemaining = ~vRemaining;
                    offset += (nuint)Vector<byte>.Count;
                }
            }

            ref nuint n = ref Unsafe.As<byte, nuint>(ref baseByteRef);
            for (nuint stopLoopAtOffset = totalByteLength & ~((nuint)sizeof(nuint) * 8 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 8)
            {
                ref nuint n1 = ref Unsafe.AddByteOffset(ref n, offset); n1 = ~n1;
                ref nuint n2 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)); n2 = ~n2;
                ref nuint n3 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2); n3 = ~n3;
                ref nuint n4 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3); n4 = ~n4;
                ref nuint n5 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 4); n5 = ~n5;
                ref nuint n6 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 5); n6 = ~n6;
                ref nuint n7 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 6); n7 = ~n7;
                ref nuint n8 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 7); n8 = ~n8;
            }
            remainingBytes = totalByteLength - offset;
            if ((remainingBytes & (nuint)sizeof(nuint) * 4) != 0)
            {
                ref nuint n1 = ref Unsafe.AddByteOffset(ref n, offset); n1 = ~n1;
                ref nuint n2 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)); n2 = ~n2;
                ref nuint n3 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2); n3 = ~n3;
                ref nuint n4 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3); n4 = ~n4;
                offset += (nuint)sizeof(nuint) * 4;
            }
            if ((remainingBytes & (nuint)sizeof(nuint) * 2) != 0)
            {
                ref nuint n1 = ref Unsafe.AddByteOffset(ref n, offset); n1 = ~n1;
                ref nuint n2 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)); n2 = ~n2;
                offset += (nuint)sizeof(nuint) * 2;
            }
            if ((remainingBytes & (nuint)sizeof(nuint)) != 0)
            {
                ref nuint n1 = ref Unsafe.AddByteOffset(ref n, offset); n1 = ~n1;
                offset += (nuint)sizeof(nuint);
            }
            remainingBytes = totalByteLength - offset;
        }
        else
        {
            if ((remainingBytes & sizeof(ulong)) != 0)
            {
                ref ulong n1 = ref Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref baseByteRef, offset)); n1 = ~n1;
                offset += sizeof(ulong);
            }
        }
        if ((remainingBytes & sizeof(uint)) != 0)
        {
            ref uint n1 = ref Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref baseByteRef, offset)); n1 = ~n1;
            offset += sizeof(uint);
        }
        if ((remainingBytes & sizeof(ushort)) != 0)
        {
            ref ushort n1 = ref Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref baseByteRef, offset)); n1 = (ushort)~n1;
            offset += sizeof(ushort);
        }
        if ((remainingBytes & sizeof(byte)) != 0)
        {
            ref byte n1 = ref Unsafe.AddByteOffset(ref baseByteRef, offset); n1 = (byte)~n1;
        }
    }
}
#endif