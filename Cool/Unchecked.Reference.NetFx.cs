#if NETFRAMEWORK
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;


public static partial class Unchecked
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

    #region aggressive inlining Array.GetReference for .NET Framework
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[] array) => ref ((Array<T>)array).GetReference();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this Array array)
    {
        int rank = array.Rank;
        ref T baseRef = ref Unsafe.As<Array<T>>(array).GetReference();
        if (rank == 1 && array.GetLowerBound(0) == 0) return ref baseRef;
        return ref Unsafe.AddByteOffset(ref baseRef, (IntPtr)(rank << 3));
    }
    #endregion

}
#endif