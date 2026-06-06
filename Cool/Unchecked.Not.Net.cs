#if !NETFRAMEWORK
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint FastNot(ref byte source, nuint length)
    {
        nuint offset = 0;
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            ref Vector<byte> v = ref Unsafe.As<byte, Vector<byte>>(ref source);
            for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
            {
                Unsafe.AddByteOffset(ref v, offset) = ~Unsafe.AddByteOffset(ref v, offset);
                Unsafe.AddByteOffset(ref v, offset + (nuint)Vector<byte>.Count) = ~Unsafe.AddByteOffset(ref v, offset + (nuint)Vector<byte>.Count);
            }
            if ((length & (nuint)Vector<byte>.Count) != 0)
            {
                Unsafe.AddByteOffset(ref v, offset) = ~Unsafe.AddByteOffset(ref v, offset);
                offset += (nuint)Vector<byte>.Count;
            }
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SlowNot(ref byte source, nuint length, nuint offset)
    {
        ref nuint n = ref Unsafe.As<byte, nuint>(ref source);
        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            Unsafe.AddByteOffset(ref n, offset) = ~Unsafe.AddByteOffset(ref n, offset);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
        }
        nuint remainingBytes = length - offset;
        if ((remainingBytes & sizeof(ulong)) != 0)
        {
            Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref source, offset)) = ~Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref source, offset));
            offset += sizeof(ulong);
        }
        if ((remainingBytes & sizeof(uint)) != 0)
        {
            Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref source, offset)) = ~Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref source, offset));
            offset += sizeof(uint);
        }
        if ((remainingBytes & sizeof(ushort)) != 0)
        {
            Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref source, offset)) = (ushort)~Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref source, offset));
            offset += sizeof(ushort);
        }
        if ((remainingBytes & sizeof(byte)) != 0)
        {
            Unsafe.AddByteOffset(ref source, offset) = (byte)~Unsafe.AddByteOffset(ref source, offset);
        }
    }
}
#endif