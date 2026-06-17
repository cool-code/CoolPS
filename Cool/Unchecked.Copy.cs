#if NETFRAMEWORK
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    private struct Block16 { }
    [StructLayout(LayoutKind.Sequential, Size = 32)]
    private struct Block32 { }
    [StructLayout(LayoutKind.Sequential, Size = 64)]
    private struct Block64 { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Copy(ref Block64 dst, ref Block64 src, nuint length, nuint offset)
    {
        if (length < 64) return;
        if ((length & 64) != 0)
        {
            Unsafe.AddByteOffset(ref dst, offset) = Unsafe.AddByteOffset(ref src, offset);
            offset += 64;
        }
        for (nuint stopLoopAtOffset = length; offset < stopLoopAtOffset; offset += 128)
        {
            Unsafe.AddByteOffset(ref dst, offset) = Unsafe.AddByteOffset(ref src, offset);
            Unsafe.AddByteOffset(ref dst, offset + 64) = Unsafe.AddByteOffset(ref src, offset + 64);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Copy(ref Block16 dst, ref Block16 src, nuint length, nuint offset)
    {
        if (length < 16) return;
        if ((length & 16) != 0)
        {
            Unsafe.AddByteOffset(ref dst, offset) = Unsafe.AddByteOffset(ref src, offset);
            offset += 16;
        }
        if ((length & 32) != 0)
        {
            Unsafe.AddByteOffset(ref Unsafe.As<Block16, Block32>(ref dst), offset) = Unsafe.AddByteOffset(ref Unsafe.As<Block16, Block32>(ref src), offset);
            offset += 32;
        }
        Copy(ref Unsafe.As<Block16, Block64>(ref dst), ref Unsafe.As<Block16, Block64>(ref src), length, offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Copy(ref ulong dst, ref ulong src, nuint length, nuint offset)
    {
        if (length < 8) return;
        if ((length & 8) != 0)
        {
            Unsafe.AddByteOffset(ref dst, offset) = Unsafe.AddByteOffset(ref src, offset);
            offset += 8;
        }
        Copy(ref Unsafe.As<ulong, Block16>(ref dst), ref Unsafe.As<ulong, Block16>(ref src), length, offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Copy(ref uint dst, ref uint src, nuint length, nuint offset)
    {
        if (length < 4) return;
        if ((length & 4) != 0)
        {
            Unsafe.AddByteOffset(ref dst, offset) = Unsafe.AddByteOffset(ref src, offset);
            offset += 4;
        }
        Copy(ref Unsafe.As<uint, ulong>(ref dst), ref Unsafe.As<uint, ulong>(ref src), length, offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Copy(ref ushort dst, ref ushort src, nuint length, nuint offset)
    {
        if (length < 2) return;
        if ((length & 2) != 0)
        {
            Unsafe.AddByteOffset(ref dst, offset) = Unsafe.AddByteOffset(ref src, offset);
            offset += 2;
        }
        Copy(ref Unsafe.As<ushort, uint>(ref dst), ref Unsafe.As<ushort, uint>(ref src), length, offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Copy(ref byte dst, ref byte src, nuint length)
    {
        nuint offset = 0;
        if ((length & 1) != 0)
        {
            Unsafe.AddByteOffset(ref dst, offset) = Unsafe.AddByteOffset(ref src, offset);
            offset += 1;
        }
        Copy(ref Unsafe.As<byte, ushort>(ref dst), ref Unsafe.As<byte, ushort>(ref src), length, offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(ref T dst, ref T src, int numElements)
    {
        if (numElements == 0) return;
        if (Unsafe.AreSame(ref dst, ref src)) return;
        if (numElements == 1) { dst = src; return; }

        nuint byteCount = (nuint)Unsafe.ByteOffset(ref src, ref Unsafe.Add(ref src, numElements));
        nuint diff = (nuint)Unsafe.ByteOffset(ref src, ref dst);
        bool isOverlapped = diff < byteCount;

        if (!isOverlapped && !RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            ref byte dstBytes = ref Unsafe.As<T, byte>(ref dst);
            ref byte srcBytes = ref Unsafe.As<T, byte>(ref src);
            if (Unsafe.SizeOf<T>() % 64 == 0)
            {
                Copy(ref Unsafe.As<byte, Block64>(ref dstBytes), ref Unsafe.As<byte, Block64>(ref srcBytes), byteCount, 0);
            }
            else if (Unsafe.SizeOf<T>() % 16 == 0)
            {
                Copy(ref Unsafe.As<byte, Block16>(ref dstBytes), ref Unsafe.As<byte, Block16>(ref srcBytes), byteCount, 0);
            }
            else if (Unsafe.SizeOf<T>() % 8 == 0)
            {
                Copy(ref Unsafe.As<byte, ulong>(ref dstBytes), ref Unsafe.As<byte, ulong>(ref srcBytes), byteCount, 0);
            }
            else if (Unsafe.SizeOf<T>() % 4 == 0)
            {
                Copy(ref Unsafe.As<byte, uint>(ref dstBytes), ref Unsafe.As<byte, uint>(ref srcBytes), byteCount, 0);
            }
            else if (Unsafe.SizeOf<T>() % 2 == 0)
            {
                Copy(ref Unsafe.As<byte, ushort>(ref dstBytes), ref Unsafe.As<byte, ushort>(ref srcBytes), byteCount, 0);
            }
            else
            {
                Copy(ref dstBytes, ref srcBytes, byteCount);
            }
        }
        else
        {
            SlowCopy(ref dst, ref src, numElements);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowCopy<T>(ref T dst, ref T src, int numElements)
    {
        bool srcGreaterThanDst = Unsafe.IsAddressGreaterThan(ref src, ref dst);
        int direction = srcGreaterThanDst ? 1 : -1;
        int runCount = srcGreaterThanDst ? 0 : numElements - 1;
        int loopCount = 0;
        for (; loopCount < (numElements & ~1); loopCount += 2)
        {
            Unsafe.Add(ref dst, runCount) = Unsafe.Add(ref src, runCount);
            Unsafe.Add(ref dst, runCount + direction * 1) = Unsafe.Add(ref src, runCount + direction * 1);
            runCount += direction * 2;
        }
        if (loopCount < numElements)
        {
            Unsafe.Add(ref dst, runCount) = Unsafe.Add(ref src, runCount);
        }
    }
}
#endif