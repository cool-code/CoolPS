#if NETFRAMEWORK
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    #region aggressive inlining string.GetPinnableReference for .NET Framework
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static ref readonly char GetPinnableReference(this string str) => ref Unsafe.As<string, RawString>(ref str).FirstChar;
    #endregion

    #region aggressive inlining Array.GetReference for .NET Framework
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[] array) => ref Unsafe.As<byte, T>(ref Unsafe.As<T[], RawArray>(ref array).Data);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,] array) => ref Unsafe.As<byte, T>(ref Unsafe.As<T[,], Array2D>(ref array).Data);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.As<T[,,], Array3D>(ref array).Data);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.As<T[,,,], Array4D>(ref array).Data);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref T GetReference<T>(this Array array)
    {
        // Write separately for JIT addressing optimization, do not try to merge into one line
        nint baseSize = (nint)Unsafe.GetBaseSize(array);
        return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<Array, RawArray>(ref array).Data, baseSize - (3 * sizeof(IntPtr))));
    }
    #endregion
}
#endif