using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint FastXor(ref byte left, ref byte right, nuint length)
    {
        nuint offset = 0;
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            ref Vector<byte> vleft = ref Unsafe.As<byte, Vector<byte>>(ref left);
            ref Vector<byte> vright = ref Unsafe.As<byte, Vector<byte>>(ref right);
            for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
            {
                Unsafe.AddByteOffset(ref vleft, offset) ^= Unsafe.AddByteOffset(ref vright, offset);
                Unsafe.AddByteOffset(ref vleft, offset + (nuint)Vector<byte>.Count) ^= Unsafe.AddByteOffset(ref vright, offset + (nuint)Vector<byte>.Count);
            }
            if ((length & (nuint)Vector<byte>.Count) != 0)
            {
                Unsafe.AddByteOffset(ref vleft, offset) ^= Unsafe.AddByteOffset(ref vright, offset);
                offset += (nuint)Vector<byte>.Count;
            }
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe nuint SlowXor(ref ulong left, ref ulong right, nuint length, nuint offset, out nuint remaining)
    {
        ref nuint nleft = ref Unsafe.As<ulong, nuint>(ref left);
        ref nuint nright = ref Unsafe.As<ulong, nuint>(ref right);
        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            Unsafe.AddByteOffset(ref nleft, offset) ^= Unsafe.AddByteOffset(ref nright, offset);
            Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) ^= Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint));
        }
        remaining = length - offset;
        if ((remaining & sizeof(ulong)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(ulong);
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint SlowXor(ref uint left, ref uint right, nuint length, nuint offset, out nuint remaining)
    {
        offset = SlowXor(ref Unsafe.As<uint, ulong>(ref left), ref Unsafe.As<uint, ulong>(ref right), length, offset, out remaining);
        if ((remaining & sizeof(uint)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(uint);
        }
        return offset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint SlowXor(ref ushort left, ref ushort right, nuint length, nuint offset, out nuint remaining)
    {
        offset = SlowXor(ref Unsafe.As<ushort, uint>(ref left), ref Unsafe.As<ushort, uint>(ref right), length, offset, out remaining);
        if ((remaining & sizeof(ushort)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(ushort);
        }
        return offset;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowXor(ref byte left, ref byte right, nuint length, nuint offset)
    {
        offset = SlowXor(ref Unsafe.As<byte, ushort>(ref left), ref Unsafe.As<byte, ushort>(ref right), length, offset, out nuint remaining);
        if ((remaining & sizeof(byte)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Xor(ref ulong left, ref ulong right, nuint numElements)
    {
        if (numElements == 1) { left ^= right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ulong>();
        nuint offset = FastXor(ref Unsafe.As<ulong, byte>(ref left), ref Unsafe.As<ulong, byte>(ref right), length);
        if (offset < length) SlowXor(ref left, ref right, length, offset, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Xor(ref uint left, ref uint right, nuint numElements)
    {
        if (numElements == 1) { left ^= right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<uint>();
        nuint offset = FastXor(ref Unsafe.As<uint, byte>(ref left), ref Unsafe.As<uint, byte>(ref right), length);
        if (offset < length) SlowXor(ref left, ref right, length, offset, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Xor(ref ushort left, ref ushort right, nuint numElements)
    {
        if (numElements == 1) { left ^= right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ushort>();
        nuint offset = FastXor(ref Unsafe.As<ushort, byte>(ref left), ref Unsafe.As<ushort, byte>(ref right), length);
        if (offset < length) SlowXor(ref left, ref right, length, offset, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Xor(ref byte left, ref byte right, nuint length)
    {
        if (length == 1) { left ^= right; return; }
        if (length == 0) return;
        nuint offset = FastXor(ref left, ref right, length);
        if (offset < length) SlowXor(ref left, ref right, length, offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Xor<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() % sizeof(ulong) == 0)
        {
            Xor(ref Unsafe.As<T, ulong>(ref left), ref Unsafe.As<T, ulong>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ulong)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(uint) == 0)
        {
            Xor(ref Unsafe.As<T, uint>(ref left), ref Unsafe.As<T, uint>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(uint)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(ushort) == 0)
        {
            Xor(ref Unsafe.As<T, ushort>(ref left), ref Unsafe.As<T, ushort>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ushort)));
        }
        else
        {
            Xor(ref Unsafe.As<T, byte>(ref left), ref Unsafe.As<T, byte>(ref right), numElements * (nuint)Unsafe.SizeOf<T>());
        }
    }
}