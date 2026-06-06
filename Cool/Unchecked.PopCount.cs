using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint PopCount<T>(ref T reference, nuint numElements) where T : unmanaged
    {
        if (numElements == 1)
        {
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(ulong)) return (nuint)BitOperations.PopCount(Unsafe.As<T, ulong>(ref reference));
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(uint)) return (nuint)BitOperations.PopCount(Unsafe.As<T, uint>(ref reference));
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(ushort)) return (nuint)BitOperations.PopCount(Unsafe.As<T, ushort>(ref reference));
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(byte)) return (nuint)BitOperations.PopCount(Unsafe.As<T, byte>(ref reference));
        }
        return PopCount(ref Unsafe.As<T, byte>(ref reference), numElements * (nuint)Unsafe.SizeOf<T>());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe nuint PopCount(ref byte source, nuint length)
    {
        if (length == 0) return 0;
        nuint count = 0;
        nuint offset = 0;
        if (length > sizeof(ulong))
        {
            ref nuint n = ref Unsafe.As<byte, nuint>(ref source);
            for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 8 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 8)
            {
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 4));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 5));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 6));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 7));
            }
            if ((length & (nuint)sizeof(nuint) * 4) != 0)
            {
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 2));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint) * 3));
                offset += (nuint)sizeof(nuint) * 4;
            }
            if ((length & (nuint)sizeof(nuint) * 2) != 0)
            {
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset));
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset + (nuint)sizeof(nuint)));
                offset += (nuint)sizeof(nuint) * 2;
            }
            if ((length & (nuint)sizeof(nuint)) != 0)
            {
                count += (nuint)BitOperations.PopCount(Unsafe.AddByteOffset(ref n, offset));
                offset += (nuint)sizeof(nuint);
            }
        }
        else
        {
            if ((length & sizeof(ulong)) != 0)
            {
                count += (nuint)BitOperations.PopCount(Unsafe.As<byte, ulong>(ref Unsafe.AddByteOffset(ref source, offset)));
                offset += sizeof(ulong);
            }
        }
        nuint remainingBytes = length - offset;
        if ((remainingBytes & sizeof(uint)) != 0)
        {
            count += (nuint)BitOperations.PopCount(Unsafe.As<byte, uint>(ref Unsafe.AddByteOffset(ref source, offset)));
            offset += sizeof(uint);
        }
        if ((remainingBytes & sizeof(ushort)) != 0)
        {
            count += (nuint)BitOperations.PopCount(Unsafe.As<byte, ushort>(ref Unsafe.AddByteOffset(ref source, offset)));
            offset += sizeof(ushort);
        }
        if ((remainingBytes & sizeof(byte)) != 0)
        {
            count += (nuint)BitOperations.PopCount(Unsafe.As<byte, byte>(ref Unsafe.AddByteOffset(ref source, offset)));
        }
        return count;
    }

}
