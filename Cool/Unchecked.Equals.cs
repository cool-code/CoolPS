using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FastEquals(ref byte left, ref byte right, nuint length)
    {
        nuint offset = 0;
        ref Vector<byte> vleft = ref Unsafe.As<byte, Vector<byte>>(ref left);
        ref Vector<byte> vright = ref Unsafe.As<byte, Vector<byte>>(ref right);

        for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
        {
            if (Unsafe.AddByteOffset(ref vleft, offset) != Unsafe.AddByteOffset(ref vright, offset)) return false;
            if (Unsafe.AddByteOffset(ref vleft, offset + (nuint)Vector<byte>.Count) != Unsafe.AddByteOffset(ref vright, offset + (nuint)Vector<byte>.Count)) return false;
        }

        if ((length & (nuint)Vector<byte>.Count) != 0)
        {
            if (Unsafe.AddByteOffset(ref vleft, offset) != Unsafe.AddByteOffset(ref vright, offset)) return false;
        }
        if (Unsafe.AddByteOffset(ref vleft, length - (nuint)Vector<byte>.Count) != Unsafe.AddByteOffset(ref vright, length - (nuint)Vector<byte>.Count)) return false;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe bool SlowEquals(ref ulong left, ref ulong right, nuint length, out nuint offset, out nuint remaining)
    {
        offset = 0;
        remaining = length;
        ref nuint nleft = ref Unsafe.As<ulong, nuint>(ref left);
        ref nuint nright = ref Unsafe.As<ulong, nuint>(ref right);

        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            if (Unsafe.AddByteOffset(ref nleft, offset) != Unsafe.AddByteOffset(ref nright, offset)) return false;
            if (Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) != Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint))) return false;
        }

        remaining = length - offset;
        if ((remaining & sizeof(ulong)) != 0)
        {
            if (Unsafe.AddByteOffset(ref left, offset) != Unsafe.AddByteOffset(ref right, offset)) return false;
            offset += sizeof(ulong);
        }
        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SlowEquals(ref uint left, ref uint right, nuint length, out nuint offset, out nuint remaining)
    {
        if (!SlowEquals(ref Unsafe.As<uint, ulong>(ref left), ref Unsafe.As<uint, ulong>(ref right), length, out offset, out remaining)) return false;
        if ((remaining & sizeof(uint)) != 0)
        {
            if (Unsafe.AddByteOffset(ref left, offset) != Unsafe.AddByteOffset(ref right, offset)) return false;
            offset += sizeof(uint);
        }
        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SlowEquals(ref ushort left, ref ushort right, nuint length, out nuint offset, out nuint remaining)
    {
        if (!SlowEquals(ref Unsafe.As<ushort, uint>(ref left), ref Unsafe.As<ushort, uint>(ref right), length, out offset, out remaining)) return false;
        if ((remaining & sizeof(ushort)) != 0)
        {
            if (Unsafe.AddByteOffset(ref left, offset) != Unsafe.AddByteOffset(ref right, offset)) return false;
            offset += sizeof(ushort);
        }
        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SlowEquals(ref byte left, ref byte right, nuint length)
    {
        if (!SlowEquals(ref Unsafe.As<byte, ushort>(ref left), ref Unsafe.As<byte, ushort>(ref right), length, out nuint offset, out nuint remaining)) return false;
        if ((remaining & sizeof(byte)) != 0)
        {
            if (Unsafe.AddByteOffset(ref left, offset) != Unsafe.AddByteOffset(ref right, offset)) return false;
        }
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(ref ulong left, ref ulong right, nuint numElements)
    {
        if (numElements == 0) return true;
        if (left != right) return false;
        if (numElements == 1) return true;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ulong>();
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            return FastEquals(ref Unsafe.As<ulong, byte>(ref left), ref Unsafe.As<ulong, byte>(ref right), length);
        }
        else
        {
            return SlowEquals(ref left, ref right, length, out _, out _);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(ref uint left, ref uint right, nuint numElements)
    {
        if (numElements == 0) return true;
        if (left != right) return false;
        if (numElements == 1) return true;
        nuint length = numElements * (nuint)Unsafe.SizeOf<uint>();
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            return FastEquals(ref Unsafe.As<uint, byte>(ref left), ref Unsafe.As<uint, byte>(ref right), length);
        }
        else
        {
            return SlowEquals(ref left, ref right, length, out _, out _);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(ref ushort left, ref ushort right, nuint numElements)
    {
        if (numElements == 0) return true;
        if (left != right) return false;
        if (numElements == 1) return true;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ushort>();
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            return FastEquals(ref Unsafe.As<ushort, byte>(ref left), ref Unsafe.As<ushort, byte>(ref right), length);
        }
        else
        {
            return SlowEquals(ref left, ref right, length, out _, out _);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(ref byte left, ref byte right, nuint length)
    {
        if (length == 0) return true;
        if (left != right) return false;
        if (length == 1) return true;
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            return FastEquals(ref left, ref right, length);
        }
        else
        {
            return SlowEquals(ref left, ref right, length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() % sizeof(ulong) == 0)
        {
            return Equals(ref Unsafe.As<T, ulong>(ref left), ref Unsafe.As<T, ulong>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ulong)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(uint) == 0)
        {
            return Equals(ref Unsafe.As<T, uint>(ref left), ref Unsafe.As<T, uint>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(uint)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(ushort) == 0)
        {
            return Equals(ref Unsafe.As<T, ushort>(ref left), ref Unsafe.As<T, ushort>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ushort)));
        }
        else
        {
            return Equals(ref Unsafe.As<T, byte>(ref left), ref Unsafe.As<T, byte>(ref right), numElements * (nuint)Unsafe.SizeOf<T>());
        }
    }
}