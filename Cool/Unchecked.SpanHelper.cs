using System.Runtime.CompilerServices;
using static InlineIL.IL.Emit;

namespace Cool;

public static partial class Unchecked
{
    #region Span Helper

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.Span<T> AsSystemSpan<T>(ref Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.Span<T> AsSystemSpanRef<T>(ref readonly Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.Span<T> AsSystemSpan<T>(ref ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.Span<T> AsSystemSpanRef<T>(ref readonly ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.Span<T> AsSystemSpan<T>(ref System.ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.Span<T> AsSystemSpanRef<T>(ref readonly System.ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.ReadOnlySpan<T> AsSystemReadOnlySpan<T>(ref ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.ReadOnlySpan<T> AsSystemReadOnlySpanRef<T>(ref readonly ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.ReadOnlySpan<T> AsSystemReadOnlySpan<T>(ref Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.ReadOnlySpan<T> AsSystemReadOnlySpanRef<T>(ref readonly Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.ReadOnlySpan<T> AsSystemReadOnlySpan<T>(ref System.Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref System.ReadOnlySpan<T> AsSystemReadOnlySpanRef<T>(ref readonly System.Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Span<T> AsSpan<T>(ref System.Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Span<T> AsSpanRef<T>(ref readonly System.Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Span<T> AsSpan<T>(ref System.ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Span<T> AsSpanRef<T>(ref readonly System.ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Span<T> AsSpan<T>(ref ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Span<T> AsSpanRef<T>(ref readonly ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ReadOnlySpan<T> AsReadOnlySpan<T>(ref System.ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ReadOnlySpan<T> AsReadOnlySpanRef<T>(ref readonly System.ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ReadOnlySpan<T> AsReadOnlySpan<T>(ref System.Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ReadOnlySpan<T> AsReadOnlySpanRef<T>(ref readonly System.Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ReadOnlySpan<T> AsReadOnlySpan<T>(ref Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ReadOnlySpan<T> AsReadOnlySpanRef<T>(ref readonly Span<T> source)
    {
        Ldarg(nameof(source));
        Ret();
        throw null!;
    }

    #endregion
}