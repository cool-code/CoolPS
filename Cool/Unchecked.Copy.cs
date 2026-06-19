using System.Numerics;
using System.Reflection;
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
    private static void CopyForward<T>(ref T dst, ref T src, nint length)
    {
        if (Unsafe.SizeOf<T>() % 32 == 0)
        {
            CopyForward(ref Unsafe.As<T, Block32>(ref dst), ref Unsafe.As<T, Block32>(ref src), length);
        }
        else if (Unsafe.SizeOf<T>() % 16 == 0)
        {
            CopyForward(ref Unsafe.As<T, Block16>(ref dst), ref Unsafe.As<T, Block16>(ref src), length);
        }
        else if (Unsafe.SizeOf<T>() % 8 == 0)
        {
            CopyForward(ref Unsafe.As<T, ulong>(ref dst), ref Unsafe.As<T, ulong>(ref src), length);
        }
        else if (Unsafe.SizeOf<T>() % 4 == 0)
        {
            CopyForward(ref Unsafe.As<T, uint>(ref dst), ref Unsafe.As<T, uint>(ref src), length);
        }
        else if (Unsafe.SizeOf<T>() % 2 == 0)
        {
            CopyForward(ref Unsafe.As<T, ushort>(ref dst), ref Unsafe.As<T, ushort>(ref src), length);
        }
        else
        {
            CopyForward(ref Unsafe.As<T, byte>(ref dst), ref Unsafe.As<T, byte>(ref src), length);
        }
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
            dst = ref Unsafe.Subtract(ref dst, 2);
            src = ref Unsafe.Subtract(ref src, 2);
            Unsafe.Add(ref dst, 1) = Unsafe.Add(ref src, 1);
            dst = src;
            length -= 64;
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
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            length -= 32;
            if (length == 0) return;
        }
        if (FastCopyBackward(ref dst, ref src, length)) return;
        do
        {
            dst = ref Unsafe.Subtract(ref dst, 2);
            src = ref Unsafe.Subtract(ref src, 2);
            Unsafe.Add(ref dst, 1) = Unsafe.Add(ref src, 1);
            dst = src;
            length -= 64;
        } while (length > 0);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref Block16 dst, ref Block16 src, nint length)
    {
        if ((length & 16) != 0)
        {
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            length -= 16;
            if (length == 0) return;
        }
        if ((nuint)Unsafe.ByteOffset(ref src, ref dst) < 32)
        {
            do
            {
                dst = ref Unsafe.Subtract(ref dst, 2);
                src = ref Unsafe.Subtract(ref src, 2);
                Unsafe.Add(ref dst, 1) = Unsafe.Add(ref src, 1);
                dst = src;
                length -= 32;
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
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            length -= 8;
            if (length == 0) return;
        }
        if (!Vector.IsHardwareAccelerated)
        {
            do
            {
                dst = ref Unsafe.Subtract(ref dst, 2);
                src = ref Unsafe.Subtract(ref src, 2);
                Unsafe.Add(ref dst, 1) = Unsafe.Add(ref src, 1);
                dst = src;
                length -= 16;
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
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            length -= 4;
            if (length == 0) return;
        }
        CopyBackward(ref Unsafe.As<uint, ulong>(ref dst), ref Unsafe.As<uint, ulong>(ref src), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref ushort dst, ref ushort src, nint length)
    {
        if ((length & 2) != 0)
        {
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            length -= 2;
            if (length == 0) return;
        }
        CopyBackward(ref Unsafe.As<ushort, uint>(ref dst), ref Unsafe.As<ushort, uint>(ref src), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref byte dst, ref byte src, nint length)
    {
        if ((length & 1) != 0)
        {
            dst = ref Unsafe.Subtract(ref dst, 1);
            src = ref Unsafe.Subtract(ref src, 1);
            dst = src;
            length -= 1;
            if (length == 0) return;
        }
        CopyBackward(ref Unsafe.As<byte, ushort>(ref dst), ref Unsafe.As<byte, ushort>(ref src), length);
    }
    private static void CopyBackward<T>(ref T dst, ref T src, nint length)
    {
        dst = ref Unsafe.AddByteOffset(ref dst, length);
        src = ref Unsafe.AddByteOffset(ref src, length);
        if (Unsafe.SizeOf<T>() % 16 == 0 && Vector.IsHardwareAccelerated)
        {
            CopyBackward(ref Unsafe.As<T, Block16>(ref dst), ref Unsafe.As<T, Block16>(ref src), length);
        }
        else if (Unsafe.SizeOf<T>() % 8 == 0)
        {
            CopyBackward(ref Unsafe.As<T, ulong>(ref dst), ref Unsafe.As<T, ulong>(ref src), length);
        }
        else if (Unsafe.SizeOf<T>() % 4 == 0)
        {
            CopyBackward(ref Unsafe.As<T, uint>(ref dst), ref Unsafe.As<T, uint>(ref src), length);
        }
        else if (Unsafe.SizeOf<T>() % 2 == 0)
        {
            CopyBackward(ref Unsafe.As<T, ushort>(ref dst), ref Unsafe.As<T, ushort>(ref src), length);
        }
        else
        {
            CopyBackward(ref Unsafe.As<T, byte>(ref dst), ref Unsafe.As<T, byte>(ref src), length);
        }
    }

    private delegate void BulkMoveWriteBarrierDelegate(ref byte dst, ref byte src, nuint byteCount);
    private static readonly BulkMoveWriteBarrierDelegate? BulkMoveWithWriteBarrier = CreateBulkMoveWithWriteBarrier();
    private static BulkMoveWriteBarrierDelegate? CreateBulkMoveWithWriteBarrier()
    {
        try
        {
            var method = typeof(System.Buffer).GetMethod("BulkMoveWithWriteBarrier", BindingFlags.Static | BindingFlags.NonPublic);
            if (method != null)
            {
                return (BulkMoveWriteBarrierDelegate)method.CreateDelegate(typeof(BulkMoveWriteBarrierDelegate));
            }
        }
        catch { }
        return null;
    }

    private static void SlowCopy<T>(ref T dst, ref T src, nint numElements, bool dstGreaterThanSrc)
    {
        nint direction = dstGreaterThanSrc ? -1 : 1;
        if (dstGreaterThanSrc)
        {
            dst = ref Unsafe.Add(ref dst, numElements - 1);
            src = ref Unsafe.Add(ref src, numElements - 1);
        }
        if ((numElements & 1) != 0)
        {
            dst = src;
            dst = ref Unsafe.Add(ref dst, direction);
            src = ref Unsafe.Add(ref src, direction);
            numElements--;
            if (numElements == 0) return;
        }
        do
        {
            dst = src;
            Unsafe.Add(ref dst, direction) = Unsafe.Add(ref src, direction);
            dst = ref Unsafe.Add(ref dst, direction * 2);
            src = ref Unsafe.Add(ref src, direction * 2);
            numElements -= 2;
        } while (numElements > 0);
    }
    private static void CopyWithReference<T>(ref T dst, ref T src, nint numElements)
    {
        nint length = numElements * Unsafe.SizeOf<T>();
        if (BulkMoveWithWriteBarrier != null)
        {
            BulkMoveWithWriteBarrier(ref Unsafe.As<T, byte>(ref dst), ref Unsafe.As<T, byte>(ref src), (nuint)length);
        }
        else
        {
            SlowCopy(ref dst, ref src, numElements, (nuint)Unsafe.ByteOffset(ref src, ref dst) < (nuint)length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(ref T dst, ref T src, nint numElements)
    {
        if (numElements == 0) return;
        if (Unsafe.AreSame(ref dst, ref src)) return;
        if (numElements == 1) { dst = src; return; }
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            CopyWithReference(ref dst, ref src, numElements);
        }
        else
        {
            nint length = numElements * Unsafe.SizeOf<T>();
            if ((nuint)Unsafe.ByteOffset(ref src, ref dst) >= (nuint)length)
            {
                CopyForward<T>(ref dst, ref src, length);
            }
            else
            {
                CopyBackward<T>(ref dst, ref src, length);
            }
        }
    }

}