using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class Unchecked
{
    #region Block Structs
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    private struct Block16 { }
    [StructLayout(LayoutKind.Sequential, Size = 32)]
    private struct Block32 { }
    [StructLayout(LayoutKind.Sequential, Size = 64)]
    private struct Block64 { }
    #endregion

    #region ref T Accessors
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Read<T>(ref T reference, int index) => Unsafe.Add(ref reference, index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T>(ref T reference, int index, in T value) => Unsafe.Add(ref reference, index) = value;
    #endregion
}