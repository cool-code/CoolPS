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
    private static unsafe nuint SlowNot(ref ulong source, nuint length, nuint offset, out nuint remaining)
    {
        ref nuint n = ref Unsafe.As<ulong, nuint>(ref source);
        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            Unsafe.AddByteOffset(ref n, offset) = ~Unsafe.AddByteOffset(ref n, offset);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
        }
        remaining = length - offset;
        if ((remaining & sizeof(ulong)) != 0)
        {
            Unsafe.AddByteOffset(ref source, offset) = ~Unsafe.AddByteOffset(ref source, offset);
            offset += sizeof(ulong);
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint SlowNot(ref uint source, nuint length, nuint offset, out nuint remaining)
    {
        offset = SlowNot(ref Unsafe.As<uint, ulong>(ref source), length, offset, out remaining);
        if ((remaining & sizeof(uint)) != 0)
        {
            Unsafe.AddByteOffset(ref source, offset) = ~Unsafe.AddByteOffset(ref source, offset);
            offset += sizeof(uint);
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint SlowNot(ref ushort source, nuint length, nuint offset, out nuint remaining)
    {
        offset = SlowNot(ref Unsafe.As<ushort, uint>(ref source), length, offset, out remaining);
        if ((remaining & sizeof(ushort)) != 0)
        {
            Unsafe.AddByteOffset(ref source, offset) = (ushort)~Unsafe.AddByteOffset(ref source, offset);
            offset += sizeof(ushort);
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowNot(ref byte source, nuint length, nuint offset)
    {
        offset = SlowNot(ref Unsafe.As<byte, ushort>(ref source), length, offset, out nuint remaining);
        if ((remaining & sizeof(byte)) != 0)
        {
            Unsafe.AddByteOffset(ref source, offset) = (byte)~Unsafe.AddByteOffset(ref source, offset);
        }
    }
}
#endif