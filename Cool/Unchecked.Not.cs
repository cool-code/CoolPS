using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint FastNot(ref byte baseByteRef, nuint totalByteLength)
    {
        nuint offset = 0;

        if (Vector.IsHardwareAccelerated && totalByteLength >= (nuint)Vector<byte>.Count)
        {
            ref Vector<byte> v = ref Unsafe.As<byte, Vector<byte>>(ref baseByteRef);
            for (nuint stopLoopAtOffset = totalByteLength & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
            {
#if NETFRAMEWORK
                ref Vector<byte> v1 = ref Unsafe.AddByteOffset(ref v, offset);
                v1 = ~v1;
                ref Vector<byte> v2 = ref Unsafe.AddByteOffset(ref v, offset + (nuint)Vector<byte>.Count);
                v2 = ~v2;
#else
                Unsafe.AddByteOffset(ref v, offset) = ~Unsafe.AddByteOffset(ref v, offset);
                Unsafe.AddByteOffset(ref v, offset + (nuint)Vector<byte>.Count) = ~Unsafe.AddByteOffset(ref v, offset + (nuint)Vector<byte>.Count);
#endif
            }
            if ((totalByteLength & (nuint)Vector<byte>.Count) != 0)
            {
#if NETFRAMEWORK
                ref Vector<byte> vRemaining = ref Unsafe.AddByteOffset(ref v, offset);
                vRemaining = ~vRemaining;
#else
                Unsafe.AddByteOffset(ref v, offset) = ~Unsafe.AddByteOffset(ref v, offset);
#endif
                offset += (nuint)Vector<byte>.Count;
            }
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SlowNot(ref byte baseByteRef, nuint totalByteLength, nuint offset)
    {
        ref nuint n = ref Unsafe.As<byte, nuint>(ref baseByteRef);
        for (nuint stopLoopAtOffset = totalByteLength & ~((nuint)sizeof(nuint) * 8 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 8)
        {
#if NETFRAMEWORK
            ref nuint n1 = ref Unsafe.AddByteOffset(ref n, offset);
            n1 = ~n1;
            ref nuint n2 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
            n2 = ~n2;
            ref nuint n3 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2);
            n3 = ~n3;
            ref nuint n4 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3);
            n4 = ~n4;
            ref nuint n5 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 4);
            n5 = ~n5;
            ref nuint n6 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 5);
            n6 = ~n6;
            ref nuint n7 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 6);
            n7 = ~n7;
            ref nuint n8 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 7);
            n8 = ~n8;
#else
            Unsafe.AddByteOffset(ref n, offset) = ~Unsafe.AddByteOffset(ref n, offset);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 4) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 4);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 5) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 5);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 6) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 6);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 7) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 7);
#endif
        }
        nuint remainingBytes = totalByteLength - offset;
        if (remainingBytes >= (nuint)sizeof(nuint) * 4)
        {
#if NETFRAMEWORK
            ref nuint n1 = ref Unsafe.AddByteOffset(ref n, offset);
            n1 = ~n1;
            ref nuint n2 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
            n2 = ~n2;
            ref nuint n3 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2);
            n3 = ~n3;
            ref nuint n4 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3);
            n4 = ~n4;
#else
            Unsafe.AddByteOffset(ref n, offset) = ~Unsafe.AddByteOffset(ref n, offset);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3);
#endif
            offset += (nuint)sizeof(nuint) * 4;
            remainingBytes -= (nuint)sizeof(nuint) * 4;
        }
        if (remainingBytes >= (nuint)sizeof(nuint) * 2)
        {
#if NETFRAMEWORK
            ref nuint n1 = ref Unsafe.AddByteOffset(ref n, offset);
            n1 = ~n1;
            ref nuint n2 = ref Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
            n2 = ~n2;
#else
            Unsafe.AddByteOffset(ref n, offset) = ~Unsafe.AddByteOffset(ref n, offset);
            Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)) = ~Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint));
#endif
            offset += (nuint)sizeof(nuint) * 2;
            remainingBytes -= (nuint)sizeof(nuint) * 2;
        }
        if (remainingBytes >= (nuint)sizeof(nuint))
        {
#if NETFRAMEWORK
            ref nuint n1 = ref Unsafe.AddByteOffset(ref n, offset);
            n1 = ~n1;
#else
            Unsafe.AddByteOffset(ref n, offset) = ~Unsafe.AddByteOffset(ref n, offset);
#endif
            offset += (nuint)sizeof(nuint);
        }
        for (; offset < totalByteLength; offset++)
        {
#if NETFRAMEWORK
            ref byte b = ref Unsafe.AddByteOffset(ref baseByteRef, offset);
            b = (byte)~b;
#else
            Unsafe.AddByteOffset(ref baseByteRef, offset) = (byte)~Unsafe.AddByteOffset(ref baseByteRef, offset);
#endif
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Not<T>(ref T reference, nuint numElements) where T : unmanaged
    {
        nuint totalByteLength = numElements * (nuint)Unsafe.SizeOf<T>();
        if (totalByteLength == 0) return;
        ref byte baseByteRef = ref Unsafe.As<T, byte>(ref reference);
        nuint offset = FastNot(ref baseByteRef, totalByteLength);
        if (offset < totalByteLength) SlowNot(ref baseByteRef, totalByteLength, offset);
    }
}