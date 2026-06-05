#if !NETFRAMEWORK
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
                    Unsafe.AddByteOffset(ref v, offset) = ~Unsafe.AddByteOffset(ref v, offset);
                    Unsafe.AddByteOffset(ref v, offset + (nuint)Vector<byte>.Count) = ~Unsafe.AddByteOffset(ref v, offset + (nuint)Vector<byte>.Count);
                }
                if ((totalByteLength & (nuint)Vector<byte>.Count) != 0)
                {
                    Unsafe.AddByteOffset(ref v, offset) = ~Unsafe.AddByteOffset(ref v, offset);
                    offset += (nuint)Vector<byte>.Count;
                }
            }

            ref nuint n = ref Unsafe.As<byte, nuint>(ref baseByteRef);
            for (nuint stopLoopAtOffset = totalByteLength & ~((nuint)sizeof(nuint) * 8 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 8)
            {
                Unsafe.AddByteOffset(ref n, offset) = ~Unsafe.AddByteOffset(ref n, offset);
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2);
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3);
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 4) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 4);
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 5) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 5);
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 6) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 6);
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 7) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 7);
            }
            remainingBytes = totalByteLength - offset;
            if ((remainingBytes & (nuint)sizeof(nuint) * 4) != 0)
            {
                Unsafe.AddByteOffset(ref n, offset) = ~Unsafe.AddByteOffset(ref n, offset);
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2);
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3);
                offset += (nuint)sizeof(nuint) * 4;
            }
            if ((remainingBytes & (nuint)sizeof(nuint) * 2) != 0)
            {
                Unsafe.AddByteOffset(ref n, offset) = ~Unsafe.AddByteOffset(ref n, offset);
                Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
                offset += (nuint)sizeof(nuint) * 2;
            }
            if ((remainingBytes & (nuint)sizeof(nuint)) != 0)
            {
                Unsafe.AddByteOffset(ref n, offset) = ~Unsafe.AddByteOffset(ref n, offset);
                offset += (nuint)sizeof(nuint);
            }
            remainingBytes = totalByteLength - offset;
        }
        else
        {
            if ((remainingBytes & sizeof(ulong)) != 0)
            {
                Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref baseByteRef, offset)) = ~Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref baseByteRef, offset));
                offset += sizeof(ulong);
            }
        }
        if ((remainingBytes & sizeof(uint)) != 0)
        {
            Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref baseByteRef, offset)) = ~Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref baseByteRef, offset));
            offset += sizeof(uint);
        }
        if ((remainingBytes & sizeof(ushort)) != 0)
        {
            Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref baseByteRef, offset)) = (ushort)~Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref baseByteRef, offset));
            offset += sizeof(ushort);
        }
        if ((remainingBytes & sizeof(byte)) != 0)
        {
            Unsafe.AddByteOffset(ref baseByteRef, offset) = (byte)~Unsafe.AddByteOffset(ref baseByteRef, offset);
        }
    }
}
#endif