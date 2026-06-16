#if NETFRAMEWORK || !NET9_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    private struct Block16 { }
    [StructLayout(LayoutKind.Sequential, Size = 64)]
    private struct Block64 { }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe bool FastCopy(ref byte left, ref byte right, nuint length)
    {
        if ((nuint)Unsafe.ByteOffset(ref right, ref left) < length || length > 2048)
        {
            Unsafe.CopyBlockUnaligned(ref left, ref right, length);
            return true;
        }
        if (length < 16) return false;
        nuint offset = 0;
        if (length <= 64)
        {
            ref Block16 left16 = ref Unsafe.As<byte, Block16>(ref left);
            ref Block16 right16 = ref Unsafe.As<byte, Block16>(ref right);
            if ((length & 32) != 0)
            {
                Unsafe.AddByteOffset(ref left16, offset) = Unsafe.AddByteOffset(ref right16, offset);
                Unsafe.AddByteOffset(ref left16, offset + 16) = Unsafe.AddByteOffset(ref right16, offset + 16);
                offset += 32;
            }
            if ((length & 16) != 0)
            {
                Unsafe.AddByteOffset(ref left16, offset) = Unsafe.AddByteOffset(ref right16, offset);
            }
            Unsafe.AddByteOffset(ref left16, length - 16) = Unsafe.AddByteOffset(ref right16, length - 16);
            return true;
        }
        ref Block64 left64 = ref Unsafe.As<byte, Block64>(ref left);
        ref Block64 right64 = ref Unsafe.As<byte, Block64>(ref right);
        if (length >= 256)
        {
            nuint misalignedElements = 64 - ((nuint)Unsafe.AsPointer(ref left) & (64 - 1));
            left64 = right64;
            right64 = ref Unsafe.AddByteOffset(ref right64, misalignedElements);
            left64 = ref Unsafe.AddByteOffset(ref left64, misalignedElements);
            length -= misalignedElements;
        }
        for (nuint stopLoopAtOffset = length & ~(nuint)127; offset < stopLoopAtOffset; offset += 128)
        {
            Unsafe.AddByteOffset(ref left64, offset) = Unsafe.AddByteOffset(ref right64, offset);
            Unsafe.AddByteOffset(ref left64, offset + 64) = Unsafe.AddByteOffset(ref right64, offset + 64);
        }
        if ((length & 64) != 0)
        {
            Unsafe.AddByteOffset(ref left64, offset) = Unsafe.AddByteOffset(ref right64, offset);
        }
        Unsafe.AddByteOffset(ref left64, length - 64) = Unsafe.AddByteOffset(ref right64, length - 64);
        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SlowCopy(ref ulong left, ref ulong right, nuint length, out nuint offset, out nuint remaining)
    {
        offset = 0;
        ref nuint nleft = ref Unsafe.As<ulong, nuint>(ref left);
        ref nuint nright = ref Unsafe.As<ulong, nuint>(ref right);

        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            Unsafe.AddByteOffset(ref nleft, offset) = Unsafe.AddByteOffset(ref nright, offset);
            Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) = Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint));
        }

        remaining = length - offset;
        if ((remaining & sizeof(ulong)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) = Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(ulong);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowCopy(ref uint left, ref uint right, nuint length, out nuint offset, out nuint remaining)
    {
        SlowCopy(ref Unsafe.As<uint, ulong>(ref left), ref Unsafe.As<uint, ulong>(ref right), length, out offset, out remaining);
        if ((remaining & sizeof(uint)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) = Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(uint);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowCopy(ref ushort left, ref ushort right, nuint length, out nuint offset, out nuint remaining)
    {
        SlowCopy(ref Unsafe.As<ushort, uint>(ref left), ref Unsafe.As<ushort, uint>(ref right), length, out offset, out remaining);
        if ((remaining & sizeof(ushort)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) = Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(ushort);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowCopy(ref byte left, ref byte right, nuint length)
    {
        SlowCopy(ref Unsafe.As<byte, ushort>(ref left), ref Unsafe.As<byte, ushort>(ref right), length, out nuint offset, out nuint remaining);
        if ((remaining & sizeof(byte)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) = Unsafe.AddByteOffset(ref right, offset);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy(ref ulong left, ref ulong right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left = right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ulong>();
        if (FastCopy(ref Unsafe.As<ulong, byte>(ref left), ref Unsafe.As<ulong, byte>(ref right), length)) return;
        SlowCopy(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy(ref uint left, ref uint right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left = right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<uint>();
        if (FastCopy(ref Unsafe.As<uint, byte>(ref left), ref Unsafe.As<uint, byte>(ref right), length)) return;
        SlowCopy(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy(ref ushort left, ref ushort right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left = right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ushort>();
        if (FastCopy(ref Unsafe.As<ushort, byte>(ref left), ref Unsafe.As<ushort, byte>(ref right), length)) return;
        SlowCopy(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy(ref byte left, ref byte right, nuint length)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (length == 1) { left = right; return; }
        if (length == 0) return;
        if (FastCopy(ref left, ref right, length)) return;
        SlowCopy(ref left, ref right, length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() % sizeof(ulong) == 0)
        {
            Copy(ref Unsafe.As<T, ulong>(ref left), ref Unsafe.As<T, ulong>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ulong)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(uint) == 0)
        {
            Copy(ref Unsafe.As<T, uint>(ref left), ref Unsafe.As<T, uint>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(uint)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(ushort) == 0)
        {
            Copy(ref Unsafe.As<T, ushort>(ref left), ref Unsafe.As<T, ushort>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ushort)));
        }
        else
        {
            Copy(ref Unsafe.As<T, byte>(ref left), ref Unsafe.As<T, byte>(ref right), numElements * (nuint)Unsafe.SizeOf<T>());
        }
    }
}
#endif