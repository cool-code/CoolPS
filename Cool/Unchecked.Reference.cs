using System.Runtime.CompilerServices;

namespace Cool;


public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(in Span<T> span) => ref span.DangerousGetPinnableReference();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(in ReadOnlySpan<T> span) => ref span.DangerousGetPinnableReference();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(in System.Span<T> span) => ref ((Span<T>)span).DangerousGetPinnableReference();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(in System.ReadOnlySpan<T> span) => ref ((ReadOnlySpan<T>)span).DangerousGetPinnableReference();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref char GetReference(char* buffer) => ref Unsafe.AsRef<char>(buffer);
}
