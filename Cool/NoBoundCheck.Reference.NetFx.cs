#if NETFRAMEWORK
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class NoBoundCheck
{
    internal static readonly IntPtr StringAdjustment = MeasureStringAdjustment();

    private static IntPtr MeasureStringAdjustment()
    {
        string sampleString = "a";
        unsafe
        {
            fixed (char* pSampleString = sampleString)
            {
                return Unsafe.ByteOffset(ref Unsafe.As<Pinnable<char>>(sampleString).Data, ref Unsafe.AsRef<char>(pSampleString));
            }
        }
    }

    /// <summary>
    /// Return a managed reference to the first character of <paramref name="str"/>.
    /// Contract: the caller must ensure <paramref name="str"/> is non-null and non-empty.
    /// The returned reference must not be stored beyond the lifetime of the string and
    /// must not be used to write into the string (strings are immutable).
    /// Implementation: this uses an overlay to obtain a <c>ref</c> to the string's first
    /// character (via <c>Unsafe.As&lt;Pinnable&lt;char&gt;&gt;(str).Data</c>) and applies a
    /// precomputed adjustment to produce a managed byref without converting the object
    /// reference to a native pointer. This is a low-level, runtime-layout-dependent
    /// optimization and should be used only where callers can satisfy the contract.
    /// </summary>
    /// <param name="str">Input string. Must be non-null and non-empty.</param>
    /// <returns>Reference to the first <c>char</c> of the string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char GetReference(string str)
    {
        Debug.Assert((str != null) && (str.Length > 0), "GetReference: caller must ensure str != null and str.Length > 0");
        return ref Unsafe.AddByteOffset(ref Unsafe.As<Pinnable<char>>(str).Data, StringAdjustment);
    }

    private static class PerTypeValues<T>
    {
        public static readonly IntPtr ArrayAdjustment = MeasureArrayAdjustment();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IntPtr MeasureArrayAdjustment()
        {
            T[] sampleArray = new T[1];
            return Unsafe.ByteOffset(ref Unsafe.As<Pinnable<T>>(sampleArray).Data, ref sampleArray[0]);
        }
    }

    /// <summary>
    /// Return a managed reference to the first element of <paramref name="array"/>.
    /// Contract: the caller must ensure <paramref name="array"/> is non-null and non-empty.
    /// The returned reference must not be stored beyond the lifetime of the array.
    /// Implementation: uses an overlay to obtain a <c>ref</c> to the array's first element
    /// (via <c>Unsafe.As&lt;Pinnable&lt;T&gt;&gt;(array).Data</c>) and applies a per-type
    /// adjustment. This is a low-level optimization that relies on runtime object layout.
    /// </summary>
    /// <param name="array">Input array. Must be non-null and non-empty.</param>
    /// <returns>Reference to the first element of the array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(T[] array)
    {
        Debug.Assert((array != null) && (array.Length > 0), "GetReference<T>: caller must ensure array != null and array.Length > 0");
        return ref Unsafe.AddByteOffset(ref Unsafe.As<Pinnable<T>>(array).Data, PerTypeValues<T>.ArrayAdjustment);
    }
}
#endif