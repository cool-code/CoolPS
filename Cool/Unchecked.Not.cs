using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Not(ref byte source, nuint length)
    {
        if (length == 0) return;
        nuint offset = FastNot(ref source, length);
        if (offset < length) SlowNot(ref source, length, offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Not<T>(ref T reference, nuint numElements) where T : unmanaged
    {
        if (numElements == 1)
        {
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(ulong)) { ref ulong r = ref Unsafe.As<T, ulong>(ref reference); r = ~r; return; }
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(uint)) { ref uint r = ref Unsafe.As<T, uint>(ref reference); r = ~r; return; }
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(ushort)) { ref ushort r = ref Unsafe.As<T, ushort>(ref reference); r = (ushort)~r; return; }
            if ((nuint)Unsafe.SizeOf<T>() == sizeof(byte)) { ref byte r = ref Unsafe.As<T, byte>(ref reference); r = (byte)~r; return; }
        }
        Not(ref Unsafe.As<T, byte>(ref reference), numElements * (nuint)Unsafe.SizeOf<T>());
    }
}