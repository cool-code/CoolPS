using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void FastAndNot(ref byte left, ref byte right, nuint length)
    {
        nuint offset = 0;
        ref Vector<byte> vleft = ref Unsafe.As<byte, Vector<byte>>(ref left);
        ref Vector<byte> vright = ref Unsafe.As<byte, Vector<byte>>(ref right);
        for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
        {
            Unsafe.AddByteOffset(ref vleft, offset) &= ~Unsafe.AddByteOffset(ref vright, offset);
            Unsafe.AddByteOffset(ref vleft, offset + (nuint)Vector<byte>.Count) &= ~Unsafe.AddByteOffset(ref vright, offset + (nuint)Vector<byte>.Count);
        }
        if ((length & (nuint)Vector<byte>.Count) != 0)
        {
            Unsafe.AddByteOffset(ref vleft, offset) &= ~Unsafe.AddByteOffset(ref vright, offset);
        }

        Unsafe.AddByteOffset(ref vleft, length - (nuint)Vector<byte>.Count) &= ~Unsafe.AddByteOffset(ref vright, length - (nuint)Vector<byte>.Count);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SlowAndNot(ref byte left, ref byte right, nuint length)
    {
        nuint offset = 0;
        if (length > sizeof(ulong))
        {
            ref nuint nleft = ref Unsafe.As<byte, nuint>(ref left);
            ref nuint nright = ref Unsafe.As<byte, nuint>(ref right);
            for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 8 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 8)
            {
                Unsafe.AddByteOffset(ref nleft, offset) &= ~Unsafe.AddByteOffset(ref nright, offset);
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint));
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint) * 2) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint) * 2);
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint) * 3) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint) * 3);
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint) * 4) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint) * 4);
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint) * 5) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint) * 5);
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint) * 6) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint) * 6);
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint) * 7) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint) * 7);
            }
            if ((length & (nuint)sizeof(nuint) * 4) != 0)
            {
                Unsafe.AddByteOffset(ref nleft, offset) &= ~Unsafe.AddByteOffset(ref nright, offset);
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint));
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint) * 2) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint) * 2);
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint) * 3) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint) * 3);
                offset += (nuint)sizeof(nuint) * 4;
            }
            if ((length & (nuint)sizeof(nuint) * 2) != 0)
            {
                Unsafe.AddByteOffset(ref nleft, offset) &= ~Unsafe.AddByteOffset(ref nright, offset);
                Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint));
                offset += (nuint)sizeof(nuint) * 2;
            }
            if ((length & (nuint)sizeof(nuint)) != 0)
            {
                Unsafe.AddByteOffset(ref nleft, offset) &= ~Unsafe.AddByteOffset(ref nright, offset);
                offset += (nuint)sizeof(nuint);
            }
        }
        else
        {
            if ((length & sizeof(ulong)) != 0)
            {
                Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref left, offset)) &= ~Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref right, offset));
                offset += sizeof(ulong);
            }
        }
        nuint remainingBytes = length - offset;
        if ((remainingBytes & sizeof(uint)) != 0)
        {
            Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref left, offset)) &= ~Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref right, offset));
            offset += sizeof(uint);
        }
        if ((remainingBytes & sizeof(ushort)) != 0)
        {
            Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref left, offset)) &= (ushort)~Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref right, offset));
            offset += sizeof(ushort);
        }
        if ((remainingBytes & sizeof(byte)) != 0)
        {
            Unsafe.As<byte, byte>(ref Unsafe.AddByteOffset(ref left, offset)) &= (byte)~Unsafe.As<byte, byte>(ref Unsafe.AddByteOffset(ref right, offset));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AndNot(ref byte left, ref byte right, nuint length)
    {
        if (length == 0) return;
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            FastAndNot(ref left, ref right, length);
        }
        else
        {
            SlowAndNot(ref left, ref right, length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AndNot<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        if (numElements == 1)
        {
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(ulong))
            {
                Unsafe.As<T, ulong>(ref left) &= ~Unsafe.As<T, ulong>(ref right);
                return;
            }
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(uint))
            {
                Unsafe.As<T, uint>(ref left) &= ~Unsafe.As<T, uint>(ref right);
                return;
            }
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(ushort))
            {
                Unsafe.As<T, ushort>(ref left) &= (ushort)~Unsafe.As<T, ushort>(ref right);
                return;
            }
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(byte))
            {
                Unsafe.As<T, byte>(ref left) &= (byte)~Unsafe.As<T, byte>(ref right);
                return;
            }
        }
        AndNot(ref Unsafe.As<T, byte>(ref left), ref Unsafe.As<T, byte>(ref right), numElements * (nuint)Unsafe.SizeOf<T>());
    }
}