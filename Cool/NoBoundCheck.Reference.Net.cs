#if NET7_0_OR_GREATER
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class NoBoundCheck
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char GetReference(string str)
    {
        return ref Unsafe.AsRef(in str.GetPinnableReference());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(T[] array)
    {
        return ref MemoryMarshal.GetArrayDataReference(array);
    }
}
#endif
