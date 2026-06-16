#if NET9_0_OR_GREATER
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy(ref ulong left, ref ulong right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left = right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ulong>();
        Unsafe.CopyBlockUnaligned(ref Unsafe.As<ulong, byte>(ref left), ref Unsafe.As<ulong, byte>(ref right), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy(ref uint left, ref uint right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left = right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<uint>();
        Unsafe.CopyBlockUnaligned(ref Unsafe.As<uint, byte>(ref left), ref Unsafe.As<uint, byte>(ref right), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy(ref ushort left, ref ushort right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left = right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ushort>();
        Unsafe.CopyBlockUnaligned(ref Unsafe.As<ushort, byte>(ref left), ref Unsafe.As<ushort, byte>(ref right), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy(ref byte left, ref byte right, nuint length)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (length == 1) { left = right; return; }
        if (length == 0) return;
        Unsafe.CopyBlockUnaligned(ref left, ref right, length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref left), ref Unsafe.As<T, byte>(ref right), numElements * (nuint)Unsafe.SizeOf<T>());
    }
}
#endif