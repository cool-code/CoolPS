using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char GetReference(this string str) => ref Unsafe.AsRef(in str.GetPinnableReference());
}
