#if NETFRAMEWORK
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static class ReferenceExtensions
{
    #region aggressive inlining string.GetPinnableReference for .NET Framework
    [StructLayout(LayoutKind.Sequential)]
    private sealed class RawStringData
    {
        public uint Length;
        public char Data;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static ref readonly char GetPinnableReference(this string? str) => ref Unsafe.As<RawStringData>(str).Data;
    #endregion

    #region aggressive inlining MemoryMarshal.GetArrayDataReference for .NET Framework
    [StructLayout(LayoutKind.Sequential)]
    private sealed class RawArrayData
    {
        public nuint Length;
        public byte Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    private sealed class RawData
    {
        public byte Data;
    }

    extension(MemoryMarshal)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetArrayDataReference<T>(T[]? array)
        {
            return ref Unsafe.As<byte, T>(ref Unsafe.As<RawArrayData>(array).Data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ref byte GetArrayDataReference(Array array)
        {
            int rank = array.Rank;
            ref byte baseRef = ref Unsafe.As<RawArrayData>(array).Data;
            if (rank == 1 && array.GetLowerBound(0) == 0) return ref baseRef;
            return ref Unsafe.AddByteOffset(ref baseRef, (IntPtr)(rank << 3));
        }
    }
    #endregion
}
#endif