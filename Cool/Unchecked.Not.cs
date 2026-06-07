using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Not(ref ulong source, nuint numElements)
    {
        if (numElements == 1) { source = ~source; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ulong>();
        nuint offset = FastNot(ref Unsafe.As<ulong, byte>(ref source), length);
        if (offset < length) SlowNot(ref source, length, offset, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Not(ref uint source, nuint numElements)
    {
        if (numElements == 1) { source = ~source; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<uint>();
        nuint offset = FastNot(ref Unsafe.As<uint, byte>(ref source), length);
        if (offset < length) SlowNot(ref source, length, offset, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Not(ref ushort source, nuint numElements)
    {
        if (numElements == 1) { source = (ushort)~source; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ushort>();
        nuint offset = FastNot(ref Unsafe.As<ushort, byte>(ref source), length);
        if (offset < length) SlowNot(ref source, length, offset, out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Not(ref byte source, nuint length)
    {
        if (length == 1) { source = (byte)~source; return; }
        if (length == 0) return;
        nuint offset = FastNot(ref source, length);
        if (offset < length) SlowNot(ref source, length, offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Not<T>(ref T source, nuint numElements) where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() % sizeof(ulong) == 0)
        {
            Not(ref Unsafe.As<T, ulong>(ref source), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ulong)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(uint) == 0)
        {
            Not(ref Unsafe.As<T, uint>(ref source), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(uint)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(ushort) == 0)
        {
            Not(ref Unsafe.As<T, ushort>(ref source), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ushort)));
        }
        else
        {
            Not(ref Unsafe.As<T, byte>(ref source), numElements * (nuint)Unsafe.SizeOf<T>());
        }
    }
}