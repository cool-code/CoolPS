#if NETFRAMEWORK
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;
using static InlineIL.IL.Emit;

namespace Cool;

public static partial class NoBoundCheck
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IntPtr Add<T>(this IntPtr start, int index)
    {
        Debug.Assert(start.ToInt64() >= 0);
        Debug.Assert(index >= 0);
        unsafe
        {
            if (sizeof(IntPtr) == sizeof(int))
            {
                // 32-bit path.
                uint byteLength = (uint)index * (uint)Unsafe.SizeOf<T>();
                return (IntPtr)(((byte*)start) + byteLength);
            }
            else
            {
                // 64-bit path.
                ulong byteLength = (ulong)index * (ulong)Unsafe.SizeOf<T>();
                return (IntPtr)(((byte*)start) + byteLength);
            }
        }
    }
    #region aggressive inlining Marshal.GetReference for .NET Framework
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class Pinnable<T>
    {
        public T Data = default!;
    }
    [StructLayout(LayoutKind.Sequential)]
    private readonly struct SpanStub<T>
    {
        public readonly Pinnable<T> Pinnable;
        public readonly IntPtr ByteOffset;
        public readonly int Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpanStub(Pinnable<T> pinnable, IntPtr intPtr, int v) : this()
        {
            Pinnable = pinnable;
            ByteOffset = intPtr;
            Length = v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference()
        {
            if (Pinnable == null)
            {
                unsafe { return ref Unsafe.AsRef<T>(ByteOffset.ToPointer()); }
            }
            else
            {
                return ref Unsafe.AddByteOffset<T>(ref Pinnable.Data, ByteOffset);
            }
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref SpanStub<T> AsSpanStub<T>(in System.Span<T> source)
    {
        Ldarg(nameof(source));
        return ref IL.ReturnRef<SpanStub<T>>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref SpanStub<T> AsSpanStub<T>(in System.ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        return ref IL.ReturnRef<SpanStub<T>>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref SpanStub<T> AsSpanStub<T>(in Span<T> source)
    {
        Ldarg(nameof(source));
        return ref IL.ReturnRef<SpanStub<T>>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref SpanStub<T> AsSpanStub<T>(in ReadOnlySpan<T> source)
    {
        Ldarg(nameof(source));
        return ref IL.ReturnRef<SpanStub<T>>();
    }

    #endregion
}
#endif