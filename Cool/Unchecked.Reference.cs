using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;


public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(in Span<T> span) => ref MemoryMarshal.GetReference<T>(span);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(in ReadOnlySpan<T> span) => ref MemoryMarshal.GetReference<T>(span);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(in System.Span<T> span) => ref MemoryMarshal.GetReference<T>((Span<T>)span);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(in System.ReadOnlySpan<T> span) => ref MemoryMarshal.GetReference<T>((ReadOnlySpan<T>)span);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char GetReference(string str) => ref Unsafe.AsRef(in str.GetPinnableReference());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(T[] array) => ref MemoryMarshal.GetArrayDataReference(array);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(Array array) => ref Unsafe.As<byte, T>(ref MemoryMarshal.GetArrayDataReference(array));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref T GetReference<T>(T* buffer) where T : unmanaged => ref Unsafe.AsRef<T>(buffer);
}
