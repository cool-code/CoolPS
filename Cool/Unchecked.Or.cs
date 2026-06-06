using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void FastOr(ref byte left, ref byte right, nuint length)
    {
        nuint offset = 0;
        ref Vector<byte> vleft = ref Unsafe.As<byte, Vector<byte>>(ref left);
        ref Vector<byte> vright = ref Unsafe.As<byte, Vector<byte>>(ref right);
        for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
        {
            Unsafe.AddByteOffset(ref vleft, offset) |= Unsafe.AddByteOffset(ref vright, offset);
            Unsafe.AddByteOffset(ref vleft, offset + (nuint)Vector<byte>.Count) |= Unsafe.AddByteOffset(ref vright, offset + (nuint)Vector<byte>.Count);
        }
        if ((length & (nuint)Vector<byte>.Count) != 0)
        {
            Unsafe.AddByteOffset(ref vleft, offset) |= Unsafe.AddByteOffset(ref vright, offset);
        }

        Unsafe.AddByteOffset(ref vleft, length - (nuint)Vector<byte>.Count) |= Unsafe.AddByteOffset(ref vright, length - (nuint)Vector<byte>.Count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SlowOr(ref byte left, ref byte right, nuint length)
    {
        nuint offset = 0;
        ref nuint nleft = ref Unsafe.As<byte, nuint>(ref left);
        ref nuint nright = ref Unsafe.As<byte, nuint>(ref right);
        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            Unsafe.AddByteOffset(ref nleft, offset) |= Unsafe.AddByteOffset(ref nright, offset);
            Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) |= Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint));
        }
        nuint remainingBytes = length - offset;
        if ((remainingBytes & sizeof(ulong)) != 0)
        {
            Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref left, offset)) |= Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref right, offset));
            offset += sizeof(ulong);
        }
        if ((remainingBytes & sizeof(uint)) != 0)
        {
            Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref left, offset)) |= Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref right, offset));
            offset += sizeof(uint);
        }
        if ((remainingBytes & sizeof(ushort)) != 0)
        {
            Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref left, offset)) |= Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref right, offset));
            offset += sizeof(ushort);
        }
        if ((remainingBytes & sizeof(byte)) != 0)
        {
            Unsafe.As<byte, byte>(ref Unsafe.AddByteOffset(ref left, offset)) |= Unsafe.As<byte, byte>(ref Unsafe.AddByteOffset(ref right, offset));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Or(ref byte left, ref byte right, nuint length)
    {
        if (length == 0) return;
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            FastOr(ref left, ref right, length);
        }
        else
        {
            SlowOr(ref left, ref right, length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Or<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        if (numElements == 1)
        {
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(ulong))
            {
                Unsafe.As<T, ulong>(ref left) |= Unsafe.As<T, ulong>(ref right);
                return;
            }
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(uint))
            {
                Unsafe.As<T, uint>(ref left) |= Unsafe.As<T, uint>(ref right);
                return;
            }
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(ushort))
            {
                Unsafe.As<T, ushort>(ref left) |= Unsafe.As<T, ushort>(ref right);
                return;
            }
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(byte))
            {
                Unsafe.As<T, byte>(ref left) |= Unsafe.As<T, byte>(ref right);
                return;
            }
        }
        Or(ref Unsafe.As<T, byte>(ref left), ref Unsafe.As<T, byte>(ref right), numElements * (nuint)Unsafe.SizeOf<T>());
    }
}