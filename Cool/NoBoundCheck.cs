using System.Runtime.CompilerServices;

namespace Cool;

public static partial class NoBoundCheck
{
    #region String and Array Accessors
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Read<T>(ref T reference, int index) => Unsafe.Add(ref reference, index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T>(ref T reference, int index, in T value) => Unsafe.Add(ref reference, index) = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char Read(string str, int index) => Unsafe.Add(ref str.GetReference(), index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(string str, int index, char value) => Unsafe.Add(ref str.GetReference(), index) = value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Read<T>(T[] array, int index) => Unsafe.Add(ref array.GetReference(), index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T>(T[] array, int index, in T value) => Unsafe.Add(ref array.GetReference(), index) = value;
    #endregion
}