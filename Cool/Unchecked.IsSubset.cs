using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FastIsSubset(ref byte left, ref byte right, nuint length)
    {
        nuint offset = 0;
        ref Vector<byte> vleft = ref Unsafe.As<byte, Vector<byte>>(ref left);
        ref Vector<byte> vright = ref Unsafe.As<byte, Vector<byte>>(ref right);
        for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
        {
            if ((Unsafe.AddByteOffset(ref vleft, offset) & ~Unsafe.AddByteOffset(ref vright, offset)) != Vector<byte>.Zero) return false;
            if ((Unsafe.AddByteOffset(ref vleft, offset + (nuint)Vector<byte>.Count) & ~Unsafe.AddByteOffset(ref vright, offset + (nuint)Vector<byte>.Count)) != Vector<byte>.Zero) return false;
        }
        if ((length & (nuint)Vector<byte>.Count) != 0)
        {
            if ((Unsafe.AddByteOffset(ref vleft, offset) & ~Unsafe.AddByteOffset(ref vright, offset)) != Vector<byte>.Zero) return false;
        }
        return (Unsafe.AddByteOffset(ref vleft, length - (nuint)Vector<byte>.Count) & ~Unsafe.AddByteOffset(ref vright, length - (nuint)Vector<byte>.Count)) == Vector<byte>.Zero;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe bool SlowIsSubset(ref byte left, ref byte right, nuint length)
    {
        nuint offset = 0;
        ref nuint nleft = ref Unsafe.As<byte, nuint>(ref left);
        ref nuint nright = ref Unsafe.As<byte, nuint>(ref right);
        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            if ((Unsafe.AddByteOffset(ref nleft, offset) & ~Unsafe.AddByteOffset(ref nright, offset)) != 0) return false;
            if ((Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) & ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint))) != 0) return false;
        }
        nuint remainingBytes = length - offset;
        if ((remainingBytes & sizeof(ulong)) != 0)
        {
            if ((Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref left, offset)) & ~Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref right, offset))) != 0) return false;
            offset += sizeof(ulong);
        }
        if ((remainingBytes & sizeof(uint)) != 0)
        {
            if ((Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref left, offset)) & ~Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref right, offset))) != 0) return false;
            offset += sizeof(uint);
        }
        if ((remainingBytes & sizeof(ushort)) != 0)
        {
            if ((Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref left, offset)) & (ushort)~Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref right, offset))) != 0) return false;
            offset += sizeof(ushort);
        }
        if ((remainingBytes & sizeof(byte)) != 0)
        {
            if ((Unsafe.AddByteOffset(ref left, offset) & (byte)~Unsafe.AddByteOffset(ref right, offset)) != 0) return false;
        }
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSubset(ref byte left, ref byte right, nuint length)
    {
        if (length == 0) return true;

        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            return FastIsSubset(ref left, ref right, length);
        }
        return SlowIsSubset(ref left, ref right, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSubset<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        if (numElements == 1)
        {
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(ulong)) return (Unsafe.As<T, ulong>(ref left) & ~Unsafe.As<T, ulong>(ref right)) == 0;
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(uint)) return (Unsafe.As<T, uint>(ref left) & ~Unsafe.As<T, uint>(ref right)) == 0;
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(ushort)) return (Unsafe.As<T, ushort>(ref left) & (ushort)~Unsafe.As<T, ushort>(ref right)) == 0;
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(byte)) return (Unsafe.As<T, byte>(ref left) & (byte)~Unsafe.As<T, byte>(ref right)) == 0;
        }
        return IsSubset(ref Unsafe.As<T, byte>(ref left), ref Unsafe.As<T, byte>(ref right), numElements * (nuint)Unsafe.SizeOf<T>());
    }
}