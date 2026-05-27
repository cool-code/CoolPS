#if NETFRAMEWORK
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;


public static partial class Unchecked
{
    #region aggressive inlining string.GetPinnableReference for .NET Framework
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static ref readonly char GetPinnableReference(this string? str) => ref Unsafe.As<String>(str)._firstChar;
    #endregion

    #region aggressive inlining Array.GetReference for .NET Framework
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[] array) => ref Unsafe.As<byte, T>(ref Unsafe.As<T[], RawArray>(ref array).Data);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,] array) => ref Unsafe.As<byte, T>(ref Unsafe.As<T[,], RawArray2D>(ref array).Data);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this Array array)
    {
        int rank = array.Rank;
        ref T baseRef = ref Unsafe.As<byte, T>(ref Unsafe.As<Array, RawArray>(ref array).Data);
        if (rank == 1 && array.GetLowerBound(0) == 0) return ref baseRef;
        return ref Unsafe.AddByteOffset(ref baseRef, (IntPtr)(rank << 3));
    }
    #endregion

}
#endif