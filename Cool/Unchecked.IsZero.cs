using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FastIsZero(ref byte source, nuint length)
    {
        nuint offset = 0;
        ref Vector<byte> v = ref Unsafe.As<byte, Vector<byte>>(ref source);

        for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
        {
            if (Unsafe.AddByteOffset(ref v, offset) != Vector<byte>.Zero) return false;
            if (Unsafe.AddByteOffset(ref v, offset + (nuint)Vector<byte>.Count) != Vector<byte>.Zero) return false;
        }

        if ((length & (nuint)Vector<byte>.Count) != 0)
        {
            if (Unsafe.AddByteOffset(ref v, offset) != Vector<byte>.Zero) return false;
        }
        if (Unsafe.AddByteOffset(ref v, length - (nuint)Vector<byte>.Count) != Vector<byte>.Zero) return false;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe bool SlowIsZero(ref ulong source, nuint length, out nuint offset, out nuint remaining)
    {
        offset = 0;
        remaining = length;
        ref nuint n = ref Unsafe.As<ulong, nuint>(ref source);

        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            if (Unsafe.AddByteOffset(ref n, offset) != 0) return false;
            if (Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)) != 0) return false;
        }

        remaining = length - offset;
        if ((remaining & sizeof(ulong)) != 0)
        {
            if (Unsafe.AddByteOffset(ref source, offset) != 0) return false;
            offset += sizeof(ulong);
        }
        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SlowIsZero(ref uint source, nuint length, out nuint offset, out nuint remaining)
    {
        if (!SlowIsZero(ref Unsafe.As<uint, ulong>(ref source), length, out offset, out remaining)) return false;
        if ((remaining & sizeof(uint)) != 0)
        {
            if (Unsafe.AddByteOffset(ref source, offset) != 0) return false;
            offset += sizeof(uint);
        }
        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SlowIsZero(ref ushort source, nuint length, out nuint offset, out nuint remaining)
    {
        if (!SlowIsZero(ref Unsafe.As<ushort, uint>(ref source), length, out offset, out remaining)) return false;
        if ((remaining & sizeof(ushort)) != 0)
        {
            if (Unsafe.AddByteOffset(ref source, offset) != 0) return false;
            offset += sizeof(ushort);
        }
        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SlowIsZero(ref byte source, nuint length)
    {
        if (!SlowIsZero(ref Unsafe.As<byte, ushort>(ref source), length, out nuint offset, out nuint remaining)) return false;
        if ((remaining & sizeof(byte)) != 0)
        {
            if (Unsafe.AddByteOffset(ref source, offset) != 0) return false;
        }
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(ref ulong source, nuint numElements)
    {
        if (numElements == 0) return true;
        if (source != 0) return false;
        if (numElements == 1) return true;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ulong>();
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            return FastIsZero(ref Unsafe.As<ulong, byte>(ref source), length);
        }
        else
        {
            return SlowIsZero(ref source, length, out _, out _);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(ref uint source, nuint numElements)
    {
        if (numElements == 0) return true;
        if (source != 0) return false;
        if (numElements == 1) return true;
        nuint length = numElements * (nuint)Unsafe.SizeOf<uint>();
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            return FastIsZero(ref Unsafe.As<uint, byte>(ref source), length);
        }
        else
        {
            return SlowIsZero(ref source, length, out _, out _);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(ref ushort source, nuint numElements)
    {
        if (numElements == 0) return true;
        if (source != 0) return false;
        if (numElements == 1) return true;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ushort>();
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            return FastIsZero(ref Unsafe.As<ushort, byte>(ref source), length);
        }
        else
        {
            return SlowIsZero(ref source, length, out _, out _);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero(ref byte source, nuint length)
    {
        if (length == 0) return true;
        if (source != 0) return false;
        if (length == 1) return true;
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            return FastIsZero(ref source, length);
        }
        else
        {
            return SlowIsZero(ref source, length);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsZero<T>(ref T source, nuint numElements) where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() % sizeof(ulong) == 0)
        {
            return IsZero(ref Unsafe.As<T, ulong>(ref source), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ulong)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(uint) == 0)
        {
            return IsZero(ref Unsafe.As<T, uint>(ref source), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(uint)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(ushort) == 0)
        {
            return IsZero(ref Unsafe.As<T, ushort>(ref source), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ushort)));
        }
        else
        {
            return IsZero(ref Unsafe.As<T, byte>(ref source), numElements * (nuint)Unsafe.SizeOf<T>());
        }
    }
}