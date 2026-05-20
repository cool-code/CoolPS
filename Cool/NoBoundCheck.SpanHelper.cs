using System;
using System.Runtime.CompilerServices;
using InlineIL;
using static InlineIL.IL.Emit;

namespace Cool;

public static partial class NoBoundCheck
{
    #region Span Helper

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static System.Span<T> AsSystemSpan<T>(in Span<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(System.Span<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static System.Span<T> AsSystemSpan<T>(in ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(System.Span<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static System.Span<T> AsSystemSpan<T>(in System.ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(System.Span<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static System.ReadOnlySpan<T> AsSystemReadOnlySpan<T>(in ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(System.ReadOnlySpan<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static System.ReadOnlySpan<T> AsSystemReadOnlySpan<T>(in Span<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(System.ReadOnlySpan<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static System.ReadOnlySpan<T> AsSystemReadOnlySpan<T>(in System.Span<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(System.ReadOnlySpan<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in System.Span<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(Span<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in System.ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(Span<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<T> AsSpan<T>(in ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(Span<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(in System.ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(ReadOnlySpan<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(in System.Span<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(ReadOnlySpan<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<T> AsReadOnlySpan<T>(in Span<T> source)
    {
        Ldarg(nameof(source));
        Ldobj(new TypeRef(typeof(ReadOnlySpan<>)).MakeGenericType(typeof(T)));
        Ret();
        return default;
    }

    #endregion
}