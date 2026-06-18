using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe bool FastCopyForward(ref Block32 dst, ref Block32 src, nint length)
    {
        nint misaligned = (nint)Unsafe.AsPointer(ref src) & (32 - 1);
        if (misaligned == 0 || length < 256 || (nuint)Unsafe.ByteOffset(ref dst, ref src) < 32) return false;
        nint alignmentOffset = 32 - misaligned;
        dst = src;
        dst = ref Unsafe.AddByteOffset(ref dst, alignmentOffset);
        src = ref Unsafe.AddByteOffset(ref src, alignmentOffset);
        length -= alignmentOffset;
        do
        {
            dst = src;
            Unsafe.Add(ref dst, 1) = Unsafe.Add(ref src, 1);
            dst = ref Unsafe.Add(ref dst, 2);
            src = ref Unsafe.Add(ref src, 2);
            length -= 64;
        } while (length > 64);
        dst = src;
        Unsafe.AddByteOffset(ref dst, length - 32) = Unsafe.AddByteOffset(ref src, length - 32);
        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref Block32 dst, ref Block32 src, nint length)
    {
        if ((length & 32) != 0)
        {
            dst = src;
            dst = ref Unsafe.Add(ref dst, 1);
            src = ref Unsafe.Add(ref src, 1);
            length -= 32;
            if (length == 0) return;
        }
        if (FastCopyForward(ref dst, ref src, length)) return;
        do
        {
            dst = src;
            Unsafe.Add(ref dst, 1) = Unsafe.Add(ref src, 1);
            dst = ref Unsafe.Add(ref dst, 2);
            src = ref Unsafe.Add(ref src, 2);
            length -= 64;
        } while (length > 0);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref Block16 dst, ref Block16 src, nint length)
    {
        if ((length & 16) != 0)
        {
            dst = src;
            dst = ref Unsafe.Add(ref dst, 1);
            src = ref Unsafe.Add(ref src, 1);
            length -= 16;
            if (length == 0) return;
        }
        CopyForward(ref Unsafe.As<Block16, Block32>(ref dst), ref Unsafe.As<Block16, Block32>(ref src), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref ulong dst, ref ulong src, nint length)
    {
        if ((length & 8) != 0)
        {
            dst = src;
            dst = ref Unsafe.Add(ref dst, 1);
            src = ref Unsafe.Add(ref src, 1);
            length -= 8;
            if (length == 0) return;
        }
        CopyForward(ref Unsafe.As<ulong, Block16>(ref dst), ref Unsafe.As<ulong, Block16>(ref src), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref uint dst, ref uint src, nint length)
    {
        if ((length & 4) != 0)
        {
            dst = src;
            dst = ref Unsafe.Add(ref dst, 1);
            src = ref Unsafe.Add(ref src, 1);
            length -= 4;
            if (length == 0) return;
        }
        CopyForward(ref Unsafe.As<uint, ulong>(ref dst), ref Unsafe.As<uint, ulong>(ref src), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref ushort dst, ref ushort src, nint length)
    {
        if ((length & 2) != 0)
        {
            dst = src;
            dst = ref Unsafe.Add(ref dst, 1);
            src = ref Unsafe.Add(ref src, 1);
            length -= 2;
            if (length == 0) return;
        }
        CopyForward(ref Unsafe.As<ushort, uint>(ref dst), ref Unsafe.As<ushort, uint>(ref src), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref byte dst, ref byte src, nint length)
    {
        if ((length & 1) != 0)
        {
            dst = src;
            dst = ref Unsafe.Add(ref dst, 1);
            src = ref Unsafe.Add(ref src, 1);
            length -= 1;
            if (length == 0) return;
        }
        CopyForward(ref Unsafe.As<byte, ushort>(ref dst), ref Unsafe.As<byte, ushort>(ref src), length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe bool FastCopyBackward(ref Block32 dst, ref Block32 src, nint length)
    {
        nint misaligned = (nint)Unsafe.AsPointer(ref src) & (32 - 1);
        if (misaligned == 0 || length < 256) return false;
        Unsafe.Subtract(ref dst, 1) = Unsafe.Subtract(ref src, 1);
        dst = ref Unsafe.SubtractByteOffset(ref dst, misaligned);
        src = ref Unsafe.SubtractByteOffset(ref src, misaligned);
        length -= misaligned;
        do
        {
            length -= 64;
            dst = ref Unsafe.Subtract(ref dst, 2);
            src = ref Unsafe.Subtract(ref src, 2);
            Unsafe.Add(ref dst, 1) = Unsafe.Add(ref src, 1);
            dst = src;
        } while (length > 64);
        if ((length & 32) != 0)
        {
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            length -= 32;
        }
        Unsafe.SubtractByteOffset(ref dst, length) = Unsafe.SubtractByteOffset(ref src, length);
        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref Block32 dst, ref Block32 src, nint length)
    {
        if ((length & 32) != 0)
        {
            length -= 32;
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            if (length == 0) return;
        }
        if (FastCopyBackward(ref dst, ref src, length)) return;
        do
        {
            length -= 64;
            dst = ref Unsafe.Subtract(ref dst, 2);
            src = ref Unsafe.Subtract(ref src, 2);
            Unsafe.Add(ref dst, 1) = Unsafe.Add(ref src, 1);
            dst = src;
        } while (length > 0);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref Block16 dst, ref Block16 src, nint length)
    {
        if ((length & 16) != 0)
        {
            length -= 16;
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            if (length == 0) return;
        }
        if ((nuint)Unsafe.ByteOffset(ref src, ref dst) < 32)
        {
            do
            {
                length -= 32;
                dst = ref Unsafe.Subtract(ref dst, 2);
                src = ref Unsafe.Subtract(ref src, 2);
                Unsafe.Add(ref dst, 1) = Unsafe.Add(ref src, 1);
                dst = src;
            } while (length > 0);
        }
        else
        {
            CopyBackward(ref Unsafe.As<Block16, Block32>(ref dst), ref Unsafe.As<Block16, Block32>(ref src), length);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref ulong dst, ref ulong src, nint length)
    {
        if ((length & 8) != 0)
        {
            length -= 8;
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            if (length == 0) return;
        }
        if (!Vector.IsHardwareAccelerated)
        {
            do
            {
                length -= 16;
                dst = ref Unsafe.Subtract(ref dst, 2);
                src = ref Unsafe.Subtract(ref src, 2);
                Unsafe.Add(ref dst, 1) = Unsafe.Add(ref src, 1);
                dst = src;
            } while (length > 0);
        }
        else
        {
            CopyBackward(ref Unsafe.As<ulong, Block16>(ref dst), ref Unsafe.As<ulong, Block16>(ref src), length);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref uint dst, ref uint src, nint length)
    {
        if ((length & 4) != 0)
        {
            length -= 4;
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            if (length == 0) return;
        }
        CopyBackward(ref Unsafe.As<uint, ulong>(ref dst), ref Unsafe.As<uint, ulong>(ref src), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref ushort dst, ref ushort src, nint length)
    {
        if ((length & 2) != 0)
        {
            length -= 2;
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            if (length == 0) return;
        }
        CopyBackward(ref Unsafe.As<ushort, uint>(ref dst), ref Unsafe.As<ushort, uint>(ref src), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref byte dst, ref byte src, nint length)
    {
        if ((length & 1) != 0)
        {
            length -= 1;
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            if (length == 0) return;
        }
        CopyBackward(ref Unsafe.As<byte, ushort>(ref dst), ref Unsafe.As<byte, ushort>(ref src), length);
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(ref T dst, ref T src, int numElements)
    {
        if (numElements == 0) return;
        if (Unsafe.AreSame(ref dst, ref src)) return;
        if (numElements == 1) { dst = src; return; }
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            SlowCopy(ref dst, ref src, numElements);
            return;
        }
        nint byteCount = Unsafe.ByteOffset(ref src, ref Unsafe.Add(ref src, numElements));
        ref byte dstBytes = ref Unsafe.As<T, byte>(ref dst);
        ref byte srcBytes = ref Unsafe.As<T, byte>(ref src);
        if ((nuint)Unsafe.ByteOffset(ref src, ref dst) < (nuint)byteCount)
        {
            dstBytes = ref Unsafe.AddByteOffset(ref dstBytes, byteCount);
            srcBytes = ref Unsafe.AddByteOffset(ref srcBytes, byteCount);
            if (Unsafe.SizeOf<T>() % 16 == 0 && Vector.IsHardwareAccelerated)
            {
                CopyBackward(ref Unsafe.As<byte, Block16>(ref dstBytes), ref Unsafe.As<byte, Block16>(ref srcBytes), byteCount);
            }
            else if (Unsafe.SizeOf<T>() % 8 == 0)
            {
                CopyBackward(ref Unsafe.As<byte, ulong>(ref dstBytes), ref Unsafe.As<byte, ulong>(ref srcBytes), byteCount);
            }
            else if (Unsafe.SizeOf<T>() % 4 == 0)
            {
                CopyBackward(ref Unsafe.As<byte, uint>(ref dstBytes), ref Unsafe.As<byte, uint>(ref srcBytes), byteCount);
            }
            else if (Unsafe.SizeOf<T>() % 2 == 0)
            {
                CopyBackward(ref Unsafe.As<byte, ushort>(ref dstBytes), ref Unsafe.As<byte, ushort>(ref srcBytes), byteCount);
            }
            else
            {
                CopyBackward(ref dstBytes, ref srcBytes, byteCount);
            }
            return;
        }
        if (Unsafe.SizeOf<T>() % 32 == 0)
        {
            CopyForward(ref Unsafe.As<byte, Block32>(ref dstBytes), ref Unsafe.As<byte, Block32>(ref srcBytes), byteCount);
        }
        else if (Unsafe.SizeOf<T>() % 16 == 0)
        {
            CopyForward(ref Unsafe.As<byte, Block16>(ref dstBytes), ref Unsafe.As<byte, Block16>(ref srcBytes), byteCount);
        }
        else if (Unsafe.SizeOf<T>() % 8 == 0)
        {
            CopyForward(ref Unsafe.As<byte, ulong>(ref dstBytes), ref Unsafe.As<byte, ulong>(ref srcBytes), byteCount);
        }
        else if (Unsafe.SizeOf<T>() % 4 == 0)
        {
            CopyForward(ref Unsafe.As<byte, uint>(ref dstBytes), ref Unsafe.As<byte, uint>(ref srcBytes), byteCount);
        }
        else if (Unsafe.SizeOf<T>() % 2 == 0)
        {
            CopyForward(ref Unsafe.As<byte, ushort>(ref dstBytes), ref Unsafe.As<byte, ushort>(ref srcBytes), byteCount);
        }
        else
        {
            CopyForward(ref dstBytes, ref srcBytes, byteCount);
        }
    }
}