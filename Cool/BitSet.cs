using System;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class BitSet
{
    #region Platform-Adaptive Constants
    internal static unsafe int ShiftCount
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => sizeof(IntPtr) == 8 ? 6 : 5;
    }
    internal static unsafe nuint BitMask
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => sizeof(IntPtr) == 8 ? 63u : 31u;
    }
    #endregion

    #region Internal Helper Methods
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static nuint WordCount(nuint bitHighLimit) => (bitHighLimit >> ShiftCount) + 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe nuint ByteCount(nuint bitHighLimit) => WordCount(bitHighLimit) * (uint)sizeof(nuint);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static nuint TailMask(nuint bitHighLimit)
    {
        nuint remainingBits = bitHighLimit & BitMask;
        return (remainingBits == BitMask) ? nuint.MaxValue : ((nuint)1 << (int)(remainingBits + 1)) - 1;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static nuint WordIndex(nuint bitPosition) => bitPosition >> ShiftCount;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static nuint BitIndex(nuint bitPosition) => (nuint)1 << (int)(bitPosition & BitMask);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static nuint Min(nuint left, nuint right) => Math.Min((UIntPtr)left, (UIntPtr)right);
    #endregion
}