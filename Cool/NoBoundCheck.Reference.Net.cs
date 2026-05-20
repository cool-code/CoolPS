#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class NoBoundCheck
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char GetReference(this string str)
    {
        return ref Unsafe.AsRef(in str.GetPinnableReference());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[] array)
    {
        return ref MemoryMarshal.GetArrayDataReference(array);
    }
}
#endif
