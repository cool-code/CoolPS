using System.Runtime.CompilerServices;

namespace Cool;

public static partial class NoBoundCheck
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this in Span<T> span) => ref span.DangerousGetPinnableReference();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this in ReadOnlySpan<T> span) => ref span.DangerousGetPinnableReference();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this in System.Span<T> span) => ref ((Span<T>)span).DangerousGetPinnableReference();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this in System.ReadOnlySpan<T> span) => ref ((ReadOnlySpan<T>)span).DangerousGetPinnableReference();
}
