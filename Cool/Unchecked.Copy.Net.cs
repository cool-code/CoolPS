#if !NETFRAMEWORK
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class Unchecked
{
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static void Copy(ref ulong left, ref ulong right, nuint numElements)
    // {
    //     if (numElements == 0) return;
    //     MemoryMarshal.CreateSpan(ref right, (int)numElements).CopyTo(MemoryMarshal.CreateSpan(ref left, (int)numElements));
    // }
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static void Copy(ref uint left, ref uint right, nuint numElements)
    // {
    //     if (numElements == 0) return;
    //     MemoryMarshal.CreateSpan(ref right, (int)numElements).CopyTo(MemoryMarshal.CreateSpan(ref left, (int)numElements));
    // }
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static void Copy(ref ushort left, ref ushort right, nuint numElements)
    // {
    //     if (numElements == 0) return;
    //     MemoryMarshal.CreateSpan(ref right, (int)numElements).CopyTo(MemoryMarshal.CreateSpan(ref left, (int)numElements));
    // }
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static void Copy(ref byte left, ref byte right, nuint length)
    // {
    //     if (length == 0) return;
    //     MemoryMarshal.CreateSpan(ref right, (int)length).CopyTo(MemoryMarshal.CreateSpan(ref left, (int)length));
    // }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Copy<T>(ref T left, ref T right, int numElements)
    {
        if (numElements == 0) return;
        MemoryMarshal.CreateSpan(ref right, numElements).CopyTo(MemoryMarshal.CreateSpan(ref left, numElements));
    }
}
#endif