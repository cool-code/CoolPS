using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct NIntDriver : INumberDriver<nint>
{
    public nint Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nint ShiftLeft(nint value, int shift) => value << shift;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nint AddByte(nint left, byte right) => left + right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nint Negate(nint value) => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref nint value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(nint left, nint right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nint Min(nint left, nint right) => Math.Min((IntPtr)left, (IntPtr)right);
}
