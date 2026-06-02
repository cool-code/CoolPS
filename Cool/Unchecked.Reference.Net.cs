#if !NETFRAMEWORK
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;


public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[] array) => ref MemoryMarshal.GetArrayDataReference(array);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this Array array) => ref Unsafe.As<byte, T>(ref MemoryMarshal.GetArrayDataReference(array));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this ReadOnlySpan<T> span) => ref MemoryMarshal.GetReference(span);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this Span<T> span) => ref MemoryMarshal.GetReference(span);
}
#endif