using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool CheckAndCopyForward<T>(ref T from, ref T to, ref nint length, ref nint offset)
    {
        if ((length & Unsafe.SizeOf<T>()) != 0)
        {
            Unsafe.AddByteOffset(ref to, offset) = Unsafe.AddByteOffset(ref from, offset);
            length &= ~Unsafe.SizeOf<T>();
            if (length == 0) return true;
            offset += Unsafe.SizeOf<T>();
        }
        return false;
    }
    private static unsafe void FastCopyForward(ref Block32 from, ref Block32 to, nint length)
    {
        if (length > 1024 && ((nuint)Unsafe.ByteOffset(ref to, ref from) >= 32))
        {
            nint misaligned = (nint)Unsafe.AsPointer(ref from) & (32 - 1);
            if (misaligned > 0)
            {
                nint alignmentOffset = 32 - misaligned;
                to = from;
                from = ref Unsafe.AddByteOffset(ref from, alignmentOffset);
                to = ref Unsafe.AddByteOffset(ref to, alignmentOffset);
                length -= alignmentOffset;
            }
        }
        while (length > 64)
        {
            to = from;
            Unsafe.Add(ref to, 1) = Unsafe.Add(ref from, 1);
            from = ref Unsafe.Add(ref from, 2);
            to = ref Unsafe.Add(ref to, 2);
            length -= 64;
        }
        to = from;
        Unsafe.AddByteOffset(ref to, length - 32) = Unsafe.AddByteOffset(ref from, length - 32);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref Block32 from, ref Block32 to, ref nint length, ref nint offset)
    {
        if (CheckAndCopyForward(ref from, ref to, ref length, ref offset)) return;
        FastCopyForward(
            ref Unsafe.AddByteOffset(ref from, offset),
            ref Unsafe.AddByteOffset(ref to, offset),
            length
        );
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref Block16 from, ref Block16 to, ref nint length, ref nint offset)
    {
        if (CheckAndCopyForward(ref from, ref to, ref length, ref offset)) return;
        CopyForward(ref Unsafe.As<Block16, Block32>(ref from), ref Unsafe.As<Block16, Block32>(ref to), ref length, ref offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref ulong from, ref ulong to, ref nint length, ref nint offset)
    {
        if (CheckAndCopyForward(ref from, ref to, ref length, ref offset)) return;
        CopyForward(ref Unsafe.As<ulong, Block16>(ref from), ref Unsafe.As<ulong, Block16>(ref to), ref length, ref offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref uint from, ref uint to, ref nint length, ref nint offset)
    {
        if (CheckAndCopyForward(ref from, ref to, ref length, ref offset)) return;
        CopyForward(ref Unsafe.As<uint, ulong>(ref from), ref Unsafe.As<uint, ulong>(ref to), ref length, ref offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref ushort from, ref ushort to, ref nint length, ref nint offset)
    {
        if (CheckAndCopyForward(ref from, ref to, ref length, ref offset)) return;
        CopyForward(ref Unsafe.As<ushort, uint>(ref from), ref Unsafe.As<ushort, uint>(ref to), ref length, ref offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward(ref byte from, ref byte to, ref nint length, ref nint offset)
    {
        if (CheckAndCopyForward(ref from, ref to, ref length, ref offset)) return;
        CopyForward(ref Unsafe.As<byte, ushort>(ref from), ref Unsafe.As<byte, ushort>(ref to), ref length, ref offset);
    }
    private static void CopyForward<T>(ref T from, ref T to, nint length, nint offset = 0)
    {
        if (Unsafe.SizeOf<T>() % 32 == 0)
        {
            CopyForward(ref Unsafe.As<T, Block32>(ref from), ref Unsafe.As<T, Block32>(ref to), ref length, ref offset);
        }
        else if (Unsafe.SizeOf<T>() % 16 == 0)
        {
            CopyForward(ref Unsafe.As<T, Block16>(ref from), ref Unsafe.As<T, Block16>(ref to), ref length, ref offset);
        }
        else if (Unsafe.SizeOf<T>() % 8 == 0)
        {
            CopyForward(ref Unsafe.As<T, ulong>(ref from), ref Unsafe.As<T, ulong>(ref to), ref length, ref offset);
        }
        else if (Unsafe.SizeOf<T>() % 4 == 0)
        {
            CopyForward(ref Unsafe.As<T, uint>(ref from), ref Unsafe.As<T, uint>(ref to), ref length, ref offset);
        }
        else if (Unsafe.SizeOf<T>() % 2 == 0)
        {
            CopyForward(ref Unsafe.As<T, ushort>(ref from), ref Unsafe.As<T, ushort>(ref to), ref length, ref offset);
        }
        else
        {
            CopyForward(ref Unsafe.As<T, byte>(ref from), ref Unsafe.As<T, byte>(ref to), ref length, ref offset);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool CheckAndCopyBackward<T>(ref T from, ref T to, ref nint length)
    {
        if ((length & Unsafe.SizeOf<T>()) != 0)
        {
            length &= ~Unsafe.SizeOf<T>();
            Unsafe.AddByteOffset(ref to, length) = Unsafe.AddByteOffset(ref from, length);
            if (length == 0) return true;
        }
        return false;
    }
    private static void TailsCopyBackward<T>(ref T from, ref T to, nint length)
    {
        do
        {
            length -= Unsafe.SizeOf<T>() * 2;
            Unsafe.AddByteOffset(ref to, length + Unsafe.SizeOf<T>()) = Unsafe.AddByteOffset(ref from, length + Unsafe.SizeOf<T>());
            Unsafe.AddByteOffset(ref to, length) = Unsafe.AddByteOffset(ref from, length);
        } while (length > 0);
    }
    private static unsafe void FastCopyBackward(ref Block32 from, ref Block32 to, nint length)
    {
        if (length > 1024)
        {
            nint misaligned = (nint)Unsafe.AsPointer(ref from) & (32 - 1);
            if (misaligned > 0)
            {
                Unsafe.Subtract(ref to, 1) = Unsafe.Subtract(ref from, 1);
                from = ref Unsafe.SubtractByteOffset(ref from, misaligned);
                to = ref Unsafe.SubtractByteOffset(ref to, misaligned);
                length -= misaligned;
            }
        }
        while (length > 64)
        {
            from = ref Unsafe.Subtract(ref from, 2);
            to = ref Unsafe.Subtract(ref to, 2);
            Unsafe.Add(ref to, 1) = Unsafe.Add(ref from, 1);
            to = from;
            length -= 64;
        }
        from = ref Unsafe.Subtract(ref from, 1);
        to = ref Unsafe.Subtract(ref to, 1);
        to = from;
        length -= 32;
        Unsafe.SubtractByteOffset(ref to, length) = Unsafe.SubtractByteOffset(ref from, length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref Block32 from, ref Block32 to, ref nint length)
    {
        if (CheckAndCopyBackward(ref from, ref to, ref length)) return;
        FastCopyBackward(
            ref Unsafe.AddByteOffset(ref from, length),
            ref Unsafe.AddByteOffset(ref to, length),
            length
        );
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref Block16 from, ref Block16 to, ref nint length)
    {
        if (CheckAndCopyBackward(ref from, ref to, ref length)) return;
        if ((nuint)Unsafe.ByteOffset(ref from, ref to) >= 32)
        {
            CopyBackward(ref Unsafe.As<Block16, Block32>(ref from), ref Unsafe.As<Block16, Block32>(ref to), ref length);
        }
        else
        {
            TailsCopyBackward(ref from, ref to, length);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref ulong from, ref ulong to, ref nint length)
    {
        if (CheckAndCopyBackward(ref from, ref to, ref length)) return;
        if (Vector.IsHardwareAccelerated)
        {
            CopyBackward(ref Unsafe.As<ulong, Block16>(ref from), ref Unsafe.As<ulong, Block16>(ref to), ref length);
        }
        else
        {
            TailsCopyBackward(ref from, ref to, length);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref uint from, ref uint to, ref nint length)
    {
        if (CheckAndCopyBackward(ref from, ref to, ref length)) return;
        CopyBackward(ref Unsafe.As<uint, ulong>(ref from), ref Unsafe.As<uint, ulong>(ref to), ref length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref ushort from, ref ushort to, ref nint length)
    {
        if (CheckAndCopyBackward(ref from, ref to, ref length)) return;
        CopyBackward(ref Unsafe.As<ushort, uint>(ref from), ref Unsafe.As<ushort, uint>(ref to), ref length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref byte from, ref byte to, ref nint length)
    {
        if (CheckAndCopyBackward(ref from, ref to, ref length)) return;
        CopyBackward(ref Unsafe.As<byte, ushort>(ref from), ref Unsafe.As<byte, ushort>(ref to), ref length);
    }
    private static void CopyBackward<T>(ref T from, ref T to, nint length)
    {
        if (Unsafe.SizeOf<T>() % 16 == 0 && Vector.IsHardwareAccelerated)
        {
            CopyBackward(ref Unsafe.As<T, Block16>(ref from), ref Unsafe.As<T, Block16>(ref to), length);
        }
        else if (Unsafe.SizeOf<T>() % 8 == 0)
        {
            CopyBackward(ref Unsafe.As<T, ulong>(ref from), ref Unsafe.As<T, ulong>(ref to), length);
        }
        else if (Unsafe.SizeOf<T>() % 4 == 0)
        {
            CopyBackward(ref Unsafe.As<T, uint>(ref from), ref Unsafe.As<T, uint>(ref to), ref length);
        }
        else if (Unsafe.SizeOf<T>() % 2 == 0)
        {
            CopyBackward(ref Unsafe.As<T, ushort>(ref from), ref Unsafe.As<T, ushort>(ref to), ref length);
        }
        else
        {
            CopyBackward(ref Unsafe.As<T, byte>(ref from), ref Unsafe.As<T, byte>(ref to), ref length);
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

    private static void SlowCopy<T>(ref T from, ref T to, nint numElements, bool dstGreaterThanSrc)
    {
        nint direction = dstGreaterThanSrc ? -1 : 1;
        if (dstGreaterThanSrc)
        {
            from = ref Unsafe.Add(ref from, numElements - 1);
            to = ref Unsafe.Add(ref to, numElements - 1);
        }
        if ((numElements & 1) != 0)
        {
            to = from;
            from = ref Unsafe.Add(ref from, direction);
            to = ref Unsafe.Add(ref to, direction);
            numElements--;
            if (numElements == 0) return;
        }
        do
        {
            to = from;
            Unsafe.Add(ref to, direction) = Unsafe.Add(ref from, direction);
            from = ref Unsafe.Add(ref from, direction * 2);
            to = ref Unsafe.Add(ref to, direction * 2);
            numElements -= 2;
        } while (numElements > 0);
    }
    private static void CopyWithReference<T>(ref T from, ref T to, nint numElements)
    {
        nint length = numElements * Unsafe.SizeOf<T>();
        if (BulkMoveWithWriteBarrier != null)
        {
            BulkMoveWithWriteBarrier(ref Unsafe.As<T, byte>(ref to), ref Unsafe.As<T, byte>(ref from), (nuint)length);
        }
        else
        {
            SlowCopy(ref from, ref to, numElements, (nuint)Unsafe.ByteOffset(ref from, ref to) < (nuint)length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(ref T from, ref T to, nint numElements)
    {
        if (numElements == 0) return;
        if (Unsafe.AreSame(ref from, ref to)) return;
        if (numElements == 1) { to = from; return; }
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            CopyWithReference(ref from, ref to, numElements);
        }
        else
        {
            nint length = numElements * Unsafe.SizeOf<T>();
            if ((nuint)Unsafe.ByteOffset(ref from, ref to) >= (nuint)length)
            {
                CopyForward(ref from, ref to, length);
            }
            else
            {
                CopyBackward(ref from, ref to, length);
            }
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(T[] from, nint fromIndex, T[] to, nint toIndex, nint numElements)
    {
        if (numElements == 0) return;
        ref T src = ref Unsafe.Add(ref from.GetReference(), fromIndex);
        ref T dst = ref Unsafe.Add(ref to.GetReference(), toIndex);
        if (Unsafe.AreSame(ref src, ref dst)) return;
        if (numElements == 1) { dst = src; return; }
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            System.Array.Copy(from, fromIndex, to, toIndex, numElements);
        }
        else
        {
            nint length = numElements * Unsafe.SizeOf<T>();
            if ((nuint)Unsafe.ByteOffset(ref src, ref dst) >= (nuint)length)
            {
                CopyForward(ref src, ref dst, length);
            }
            else
            {
                CopyBackward(ref src, ref dst, length);
            }
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(T[] from, T[] to, nint numElements) => Copy(from, 0, to, 0, numElements);
}