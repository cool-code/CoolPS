using System.Runtime.InteropServices;
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
    public static ref readonly char GetPinnableReference(this string? str) => ref Unsafe.As<RawString>(str).FirstChar;
    #endregion

    #region aggressive inlining Array.GetReference for .NET Framework
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[] array) => ref Unsafe.As<byte, T>(ref Unsafe.As<T[], RawArray>(ref array).Data);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,] array) => ref Unsafe.As<byte, T>(ref Unsafe.As<T[,], RawArray2D>(ref array).Data);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.As<T[,,], RawArray3D>(ref array).Data);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[,,,] array) => ref Unsafe.As<byte, T>(ref Unsafe.As<T[,,,], RawArray4D>(ref array).Data);

    [StructLayout(LayoutKind.Sequential)]
    private struct MethodTable
    {
        // Low WORD is component size for array and string types, zero otherwise
        public uint Flags;
        // Base size of instance of this class when allocated on the heap
        public uint BaseSize;
        // Number of Methods
        public ushort NumMethods;
        // Number of Virtual Methods
        public ushort NumVirtuals;
        // Number of Interfaces
        public ushort NumInterfaces;
    }

    [StructLayout(LayoutKind.Sequential)]
    private unsafe struct PMethodTable
    {
        public MethodTable* MethodTable;
    }

    [StructLayout(LayoutKind.Sequential)]
    private sealed class RawObject
    {
        public PMethodTable Placeholder = default!;
    }

    [StructLayout(LayoutKind.Sequential)]
    private sealed class RawArray<T>
    {
        public T Placeholder = default!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref T GetReference<T>(this Array array) => ref Unsafe.AddByteOffset(ref Unsafe.As<Array, RawArray<T>>(ref array).Placeholder, Unsafe.SubtractByteOffset(ref Unsafe.As<Array, RawObject>(ref array).Placeholder, (IntPtr)IntPtr.Size).MethodTable->BaseSize - (uint)(IntPtr.Size * 2));
    #endregion

}
#endif