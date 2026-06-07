using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe nuint PopCount(ref ulong source, nuint length, out nuint offset, out nuint remaining)
    {
        nuint count = 0;
        offset = 0;
        ref nuint n = ref Unsafe.As<ulong, nuint>(ref source);
        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset));
            count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)));
        }
        remaining = length - offset;
        if ((remaining & sizeof(ulong)) != 0)
        {
            count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref source, offset));
            offset += sizeof(ulong);
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint PopCount(ref uint source, nuint length, out nuint offset, out nuint remaining)
    {
        nuint count = PopCount(ref Unsafe.As<uint, ulong>(ref source), length, out offset, out remaining);
        if ((remaining & sizeof(uint)) != 0)
        {
            count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref source, offset));
            offset += sizeof(uint);
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nuint PopCount(ref ushort source, nuint length, out nuint offset, out nuint remaining)
    {
        nuint count = PopCount(ref Unsafe.As<ushort, uint>(ref source), length, out offset, out remaining);
        if ((remaining & sizeof(ushort)) != 0)
        {
            count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref source, offset));
            offset += sizeof(ushort);
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint PopCount(ref ulong source, nuint numElements)
    {
        if (numElements == 1) return (nuint)BitOperations.PopCount(source);
        if (numElements == 0) return 0;
        return PopCount(ref source, numElements * sizeof(ulong), out _, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint PopCount(ref uint source, nuint numElements)
    {
        if (numElements == 1) return (nuint)BitOperations.PopCount(source);
        if (numElements == 0) return 0;
        return PopCount(ref source, numElements * sizeof(uint), out _, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint PopCount(ref ushort source, nuint numElements)
    {
        if (numElements == 1) return (nuint)BitOperations.PopCount(source);
        if (numElements == 0) return 0;
        return PopCount(ref source, numElements * sizeof(ushort), out _, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint PopCount(ref byte source, nuint length)
    {
        if (length == 1) return (nuint)BitOperations.PopCount(source);
        if (length == 0) return 0;
        nuint count = PopCount(ref Unsafe.As<byte, ushort>(ref source), length, out nuint offset, out nuint remaining);
        if ((remaining & sizeof(byte)) != 0)
        {
            count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref source, offset));
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint PopCount<T>(ref T reference, nuint numElements) where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() % sizeof(ulong) == 0)
        {
            return PopCount(ref Unsafe.As<T, ulong>(ref reference), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ulong)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(uint) == 0)
        {
            return PopCount(ref Unsafe.As<T, uint>(ref reference), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(uint)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(ushort) == 0)
        {
            return PopCount(ref Unsafe.As<T, ushort>(ref reference), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ushort)));
        }
        else
        {
            // fallback to byte-wise counting for non-standard sizes
            return PopCount(ref Unsafe.As<T, byte>(ref reference), numElements * (nuint)Unsafe.SizeOf<T>());
        }
    }
}
