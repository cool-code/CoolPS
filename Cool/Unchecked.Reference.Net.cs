#if NET7_0_OR_GREATER
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class Unchecked
{
    /// <summary>
    /// Return a managed reference to the first character of <paramref name="str"/>.
    /// Contract: the caller must ensure <paramref name="str"/> is non-null and non-empty.
    /// The returned reference must not be stored beyond the lifetime of the string and
    /// must not be used to write into the string (strings are immutable).
    /// Implementation: this uses the runtime-provided <c>GetPinnableReference()</c> to
    /// obtain a byref to the first character and returns it as a managed reference.
    /// </summary>
    /// <param name="str">Input string. Must be non-null and non-empty.</param>
    /// <returns>Reference to the first <c>char</c> of the string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char GetReference(string str)
    {
        Debug.Assert((str != null) && (str.Length > 0), "GetReference: caller must ensure str != null and str.Length > 0");
        return ref Unsafe.AsRef(in str.GetPinnableReference());
    }

    /// <summary>
    /// Return a managed reference to the first element of <paramref name="array"/>.
    /// Contract: the caller must ensure <paramref name="array"/> is non-null and non-empty.
    /// The returned reference must not be stored beyond the lifetime of the array.
    /// Implementation: this uses <c>MemoryMarshal.GetArrayDataReference</c> which returns
    /// a managed byref to the array's first element.
    /// </summary>
    /// <param name="array">Input array. Must be non-null and non-empty.</param>
    /// <returns>Reference to the first element of the array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(T[] array)
    {
        Debug.Assert((array != null) && (array.Length > 0), "GetReference<T>: caller must ensure array != null and array.Length > 0");
        return ref MemoryMarshal.GetArrayDataReference(array);
    }
}
#endif
