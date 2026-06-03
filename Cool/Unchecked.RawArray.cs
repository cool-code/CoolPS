using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class RawArray
    {
        internal IntPtr LengthAndPadding;
        internal byte Data;
    }

    #region helper methods for Array
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref byte GetRawArrayData(this Array array) => ref Unsafe.As<Array, RawArray>(ref array).Data;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint GetLength(this Array array) => Unsafe.As<IntPtr, uint>(ref Unsafe.As<Array, RawArray>(ref array).LengthAndPadding);
    internal static void ThrowIndexOutOfRange() => throw new IndexOutOfRangeException("dimension");
    #endregion
}