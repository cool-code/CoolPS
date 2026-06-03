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
    public static ref T GetReference<T>(this T[,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,], RawArray>(ref array).Data, 4 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,], RawArray>(ref array).Data, 6 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,], RawArray>(ref array).Data, 8 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,], RawArray>(ref array).Data, 10 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,], RawArray>(ref array).Data, 12 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,], RawArray>(ref array).Data, 14 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,], RawArray>(ref array).Data, 16 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,], RawArray>(ref array).Data, 18 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,], RawArray>(ref array).Data, 20 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,], RawArray>(ref array).Data, 22 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,], RawArray>(ref array).Data, 24 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,], RawArray>(ref array).Data, 26 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,], RawArray>(ref array).Data, 28 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,], RawArray>(ref array).Data, 30 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 32 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 34 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 36 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 38 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 40 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 42 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 44 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 46 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 48 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 50 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 52 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 54 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 56 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 58 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 60 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 62 * sizeof(int)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref Unsafe.As<T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,], RawArray>(ref array).Data, 64 * sizeof(int)));
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