using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;

namespace Cool;

public static partial class Unchecked
{
#if NETFRAMEWORK
    private const nint MemmoveNativeThreshold = 64 * 1024 * 1024;
#else
    private const nint MemmoveNativeThreshold = 32 * 1024 * 1024;
#endif
    private delegate void MemmoveDelegate(ref byte dest, ref byte src, nuint len);
    private static readonly MemmoveDelegate? memmove = CreateMemmove();
    private static MemmoveDelegate? CreateMemmove()
    {
        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;
        System.Type[] types = [typeof(byte).MakeByRefType(), typeof(byte).MakeByRefType(), typeof(nuint)];
        try
        {
            // .NET 11.0
            var memmove = typeof(object).Assembly?.GetType("System.SpanHelpers")?.GetMethod("MemmoveNative", flags, null, types, null);
            if (memmove != null)
            {
                return (MemmoveDelegate)memmove.CreateDelegate(typeof(MemmoveDelegate));
            }
        }
        catch { }
        try
        {
            // .NET 10.0
            var method = typeof(System.Buffer).GetMethod("MemmoveInternal", flags, null, types, null);
            if (method != null)
            {
                return (MemmoveDelegate)method.CreateDelegate(typeof(MemmoveDelegate));
            }
        }
        catch (System.Exception e)
        {
            System.Console.WriteLine(e.ToString());
        }
        try
        {
            // .NET 7.0 ~ .NET 9.0
            var method = typeof(System.Buffer).GetMethod("_Memmove", flags, null, types, null);
            if (method != null)
            {
                return (MemmoveDelegate)method.CreateDelegate(typeof(MemmoveDelegate));
            }
        }
        catch { }
        return null;
    }
    private unsafe delegate void MemmoveX64Delegate(byte* dest, byte* src, ulong len);
    private static readonly MemmoveX64Delegate? memmoveX64 = CreateMemmoveX64();
    private static MemmoveX64Delegate? CreateMemmoveX64()
    {
        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;
        System.Type[] types = [typeof(byte).MakePointerType(), typeof(byte).MakePointerType(), typeof(ulong)];
        try
        {
            // .NET 4.x on X64 Platform
            var method = typeof(System.Buffer).GetMethod("_Memmove", flags, null, types, null);
            if (method != null)
            {
                return (MemmoveX64Delegate)method.CreateDelegate(typeof(MemmoveX64Delegate));
            }
        }
        catch { }
        return null;
    }
    private unsafe delegate void MemmoveX86Delegate(byte* dest, byte* src, uint len);
    private static readonly MemmoveX86Delegate? memmoveX86 = CreateMemmoveX86();
    private static MemmoveX86Delegate? CreateMemmoveX86()
    {
        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;
        System.Type[] types = [typeof(byte).MakePointerType(), typeof(byte).MakePointerType(), typeof(uint)];
        try
        {
            // .NET 4.x on X86 Platform
            var method = typeof(System.Buffer).GetMethod("_Memmove", flags, null, types, null);
            if (method != null)
            {
                return (MemmoveX86Delegate)method.CreateDelegate(typeof(MemmoveX86Delegate));
            }
        }
        catch { }
        return null;
    }

    private static unsafe void MemmoveNative(ref byte dest, ref byte src, nuint len)
    {
        if (memmove != null)
        {
            memmove(ref dest, ref src, len);
        }
        else fixed (byte* s = &src) fixed (byte* d = &dest)
        {
            if (memmoveX64 != null)
            {
                memmoveX64(d, s, len);
            }
            else if (memmoveX86 != null)
            {
                memmoveX86(d, s, checked((uint)len));
            }
            else
            {
                // for Native AOT or unknown platforms
                System.Buffer.MemoryCopy(s, d, len, len);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe nint MisAligned<T>(ref T from, ref T to, nint length) => (nint)Unsafe.AsPointer(ref (length > 32768) ? ref from : ref to) & (32 - 1);

#if !NETFRAMEWORK
    private static unsafe void FastCopyForward(ref Block64 from, ref Block64 to, nint length)
    {
        nint misaligned = (nint)Unsafe.AsPointer(ref to) & (64 - 1);
        if (misaligned > 0)
        {
            nint alignmentOffset = 64 - misaligned;
            to = from;
            from = ref Unsafe.AddByteOffset(ref from, alignmentOffset);
            to = ref Unsafe.AddByteOffset(ref to, alignmentOffset);
            length -= alignmentOffset;
        }
        do
        {
            to = from;
            from = ref Unsafe.AddByteOffset(ref from, 64);
            to = ref Unsafe.AddByteOffset(ref to, 64);
            length -= 64;
        } while (length > 64);
        Unsafe.AddByteOffset(ref to, length - 64) = Unsafe.AddByteOffset(ref from, length - 64);
    }
#endif
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TailsCopyForward(ref Block32 from, ref Block32 to, nint length, nint stopOffset = 0)
    {
        if (X86Base.IsSupported && length >= MemmoveNativeThreshold)
        {
            MemmoveNative(ref Unsafe.As<Block32, byte>(ref to), ref Unsafe.As<Block32, byte>(ref from), (nuint)length);
            return;
        }
        do
        {
            to = from;
            Unsafe.Add(ref to, 1) = Unsafe.Add(ref from, 1);
            from = ref Unsafe.Add(ref from, 2);
            to = ref Unsafe.Add(ref to, 2);
            length -= 64;
        } while (length > stopOffset);
        if (stopOffset == 0) return;
        if (length > 32) to = from;
        Unsafe.AddByteOffset(ref to, length - 32) = Unsafe.AddByteOffset(ref from, length - 32);
    }
    private static void FastCopyForward(ref Block32 from, ref Block32 to, nint length)
    {
        nint misaligned = MisAligned(ref from, ref to, length);
        if (misaligned > 0)
        {
            nint alignmentOffset = 32 - misaligned;
            to = from;
            from = ref Unsafe.AddByteOffset(ref from, alignmentOffset);
            to = ref Unsafe.AddByteOffset(ref to, alignmentOffset);
            length -= alignmentOffset;
        }
        TailsCopyForward(ref from, ref to, length, 64);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TailsCopyForward(ref byte from, ref byte to, nint length, nint stopOffset = 0)
    {
        if (X86Base.IsSupported && length >= MemmoveNativeThreshold)
        {
            MemmoveNative(ref to, ref from, (nuint)length);
            return;
        }
        do
        {
            Unsafe.WriteUnaligned(ref to, Unsafe.ReadUnaligned<Vector<byte>>(ref from));
            Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref to, 32), Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref from, 32)));
            from = ref Unsafe.AddByteOffset(ref from, 64);
            to = ref Unsafe.AddByteOffset(ref to, 64);
            length -= 64;
        } while (length > stopOffset);
        if (stopOffset == 0) return;
        if (length > 32) Unsafe.WriteUnaligned(ref to, Unsafe.ReadUnaligned<Vector<byte>>(ref from));
        Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref to, length - 32), Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref from, length - 32)));
    }
    private static void FastCopyForward(ref byte from, ref byte to, nint length)
    {
        nint misaligned = MisAligned(ref from, ref to, length);
        if (misaligned > 0)
        {
            nint alignmentOffset = 32 - misaligned;
            Unsafe.WriteUnaligned(ref to, Unsafe.ReadUnaligned<Vector<byte>>(ref from));
            from = ref Unsafe.AddByteOffset(ref from, alignmentOffset);
            to = ref Unsafe.AddByteOffset(ref to, alignmentOffset);
            length -= alignmentOffset;
        }
        TailsCopyForward(ref from, ref to, length, 64);
    }
    private static void CopyForward<T>(ref byte from, ref byte to, nint length)
    {
        if ((Unsafe.SizeOf<T>() % 2) != 0 && (length & 1) != 0)
        {
            to = from;
            length &= ~1;
            if (length == 0) return;
            from = ref Unsafe.Add(ref from, 1);
            to = ref Unsafe.Add(ref to, 1);
        }
        if ((Unsafe.SizeOf<T>() % 4) != 0 && (length & 2) != 0)
        {
            Unsafe.As<byte, ushort>(ref to) = Unsafe.As<byte, ushort>(ref from);
            length &= ~2;
            if (length == 0) return;
            from = ref Unsafe.Add(ref from, 2);
            to = ref Unsafe.Add(ref to, 2);
        }
        if ((Unsafe.SizeOf<T>() % 8) != 0 && (length & 4) != 0)
        {
            Unsafe.As<byte, uint>(ref to) = Unsafe.As<byte, uint>(ref from);
            length &= ~4;
            if (length == 0) return;
            from = ref Unsafe.Add(ref from, 4);
            to = ref Unsafe.Add(ref to, 4);
        }
        if ((Unsafe.SizeOf<T>() % 16) != 0 && (length & 8) != 0)
        {
            Unsafe.As<byte, ulong>(ref to) = Unsafe.As<byte, ulong>(ref from);
            length &= ~8;
            if (length == 0) return;
            from = ref Unsafe.Add(ref from, 8);
            to = ref Unsafe.Add(ref to, 8);
        }
        if ((Unsafe.SizeOf<T>() % 32) != 0 && (length & 16) != 0)
        {
            Unsafe.As<byte, Block16>(ref to) = Unsafe.As<byte, Block16>(ref from);
            length &= ~16;
            if (length == 0) return;
            from = ref Unsafe.Add(ref from, 16);
            to = ref Unsafe.Add(ref to, 16);
        }
        if ((Unsafe.SizeOf<T>() % 64) != 0 && (length & 32) != 0)
        {
            if (Vector<byte>.Count == 32)
            {
                Unsafe.WriteUnaligned(ref to, Unsafe.ReadUnaligned<Vector<byte>>(ref from));
            }
            else
            {
                Unsafe.As<byte, Block32>(ref to) = Unsafe.As<byte, Block32>(ref from);
            }
            length &= ~32;
            if (length == 0) return;
            from = ref Unsafe.Add(ref from, 32);
            to = ref Unsafe.Add(ref to, 32);
        }
        if (Vector<byte>.Count == 32)
        {
            TailsCopyForward(ref from, ref to, length);
        }
        else
        {
            TailsCopyForward(ref Unsafe.As<byte, Block32>(ref from), ref Unsafe.As<byte, Block32>(ref to), length);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyForward<T>(ref T from, ref T to, nint length)
    {
        if ((length < 256) || (nuint)Unsafe.ByteOffset(ref to, ref from) < 32)
        {
            CopyForward<T>(ref Unsafe.As<T, byte>(ref from), ref Unsafe.As<T, byte>(ref to), length);
        }
#if NETFRAMEWORK
        // AVX2 instructions are faster than Sse2 instructions,
        // but .NET 4.x does not automatically generate AVX2 instructions
        // for custom structures (such as Block32),
        // so here try to use Vector<byte> instead of Block32.
        else if (Vector<byte>.Count == 32)
        {
            FastCopyForward(ref Unsafe.As<T, byte>(ref from), ref Unsafe.As<T, byte>(ref to), length);
        }
#else
        // When the copy length is not greater than 2048, AVX-512 instructions are faster than AVX2/Sse2 instructions.
        // After exceeding, it will become slower, so the length is limited here.
        else if (length <= 2048 && ((nuint)Unsafe.ByteOffset(ref to, ref from) >= 64))
        {
            FastCopyForward(ref Unsafe.As<T, Block64>(ref from), ref Unsafe.As<T, Block64>(ref to), length);
        }
#endif
        else
        {
            FastCopyForward(ref Unsafe.As<T, Block32>(ref from), ref Unsafe.As<T, Block32>(ref to), length);
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TailsCopyBackward<T>(ref T from, ref T to, nint length)
    {
        do
        {
            length -= Unsafe.SizeOf<T>();
            Unsafe.AddByteOffset(ref to, length) = Unsafe.AddByteOffset(ref from, length);
        } while (length > 0);
    }
    private static void FastCopyBackward(ref Block32 from, ref Block32 to, nint length)
    {
        length -= 32;
        nint misaligned = MisAligned(ref from, ref to, length);
        if (misaligned > 0)
        {
            Unsafe.AddByteOffset(ref to, length) = Unsafe.AddByteOffset(ref from, length);
            length -= misaligned;
        }
        while (length >= 64)
        {
            Unsafe.AddByteOffset(ref to, length) = Unsafe.AddByteOffset(ref from, length);
            Unsafe.AddByteOffset(ref to, length - 32) = Unsafe.AddByteOffset(ref from, length - 32);
            length -= 64;
        }
        Unsafe.AddByteOffset(ref to, length) = Unsafe.AddByteOffset(ref from, length);
        to = from;
    }
#if NETFRAMEWORK
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void TailsCopyBackward(ref byte from, ref byte to, nint length)
    {
        do
        {
            length -= 32;
            Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref to, length), Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref from, length)));
        } while (length > 0);
    }
    private static void FastCopyBackward(ref byte from, ref byte to, nint length)
    {
        length -= 32;
        nint misaligned = MisAligned(ref from, ref to, length);
        if (misaligned > 0)
        {
            Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref to, length), Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref from, length)));
            length -= misaligned;
        }
        while (length >= 64)
        {
            Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref to, length), Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref from, length)));
            Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref to, length - 32), Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref from, length - 32)));
            length -= 64;
        }
        Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref to, length), Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.AddByteOffset(ref from, length)));
        Unsafe.WriteUnaligned(ref to, Unsafe.ReadUnaligned<Vector<byte>>(ref from));
    }
#endif
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref Block32 from, ref Block32 to, ref nint length)
    {
        if (CheckAndCopyBackward(ref from, ref to, ref length)) return;
#if NETFRAMEWORK
        if (Vector<byte>.Count == 32)
        {
            TailsCopyBackward(ref Unsafe.As<Block32, byte>(ref from), ref Unsafe.As<Block32, byte>(ref to), length);
        }
        else
        {
            TailsCopyBackward(ref from, ref to, length);
        }
#else
        TailsCopyBackward(ref from, ref to, length);
#endif
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref Block16 from, ref Block16 to, ref nint length)
    {
        if (CheckAndCopyBackward(ref from, ref to, ref length)) return;
        CopyBackward(ref Unsafe.As<Block16, Block32>(ref from), ref Unsafe.As<Block16, Block32>(ref to), ref length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBackward(ref ulong from, ref ulong to, ref nint length)
    {
        if (CheckAndCopyBackward(ref from, ref to, ref length)) return;
        CopyBackward(ref Unsafe.As<ulong, Block16>(ref from), ref Unsafe.As<ulong, Block16>(ref to), ref length);
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
        if (Unsafe.SizeOf<T>() % 16 == 0)
        {
            CopyBackward(ref Unsafe.As<T, Block16>(ref from), ref Unsafe.As<T, Block16>(ref to), ref length);
        }
        else if (Unsafe.SizeOf<T>() % 8 == 0)
        {
            CopyBackward(ref Unsafe.As<T, ulong>(ref from), ref Unsafe.As<T, ulong>(ref to), ref length);
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
    private static void CopyBackward<T>(ref T from, ref T to, nint length, nuint diff)
    {
        if (X86Base.IsSupported && length >= 16 && (!Vector.IsHardwareAccelerated || diff < 32))
        {
            MemmoveNative(ref Unsafe.As<T, byte>(ref to), ref Unsafe.As<T, byte>(ref from), (nuint)length);
        }
        else if (length < 256)
        {
            CopyBackward(ref from, ref to, length);
        }
#if NETFRAMEWORK
        else if (Vector<byte>.Count == 32)
        {
            FastCopyBackward(ref Unsafe.As<T, byte>(ref from), ref Unsafe.As<T, byte>(ref to), length);
        }
#endif
        else
        {
            FastCopyBackward(ref Unsafe.As<T, Block32>(ref from), ref Unsafe.As<T, Block32>(ref to), length);
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    private static void CopyWithoutReference<T>(ref T from, ref T to, nint numElements)
    {
        nint length = numElements * Unsafe.SizeOf<T>();
        nuint diff = (nuint)Unsafe.ByteOffset(ref from, ref to);
        if (diff >= (nuint)length)
        {
            CopyForward(ref from, ref to, length);
        }
        else
        {
            CopyBackward(ref from, ref to, length, diff);
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
            CopyWithoutReference(ref from, ref to, numElements);
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
            CopyWithoutReference(ref src, ref dst, numElements);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(T[] from, T[] to, nint numElements) => Copy(from, 0, to, 0, numElements);
}