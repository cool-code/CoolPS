using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    #region ref T Accessors
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Read<T>(ref T reference, int index) => Unsafe.Add(ref reference, index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T>(ref T reference, int index, in T value) => Unsafe.Add(ref reference, index) = value;
    #endregion
}