using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct NUIntDriver : INumberDriver<nuint>
{
    public nuint Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nuint ShiftLeft(nuint value, int shift) => value << shift;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nuint AddByte(nuint left, byte right) => left + right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nuint Negate(nuint value) => ~value + 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref nuint value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(nuint left, nuint right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nuint Min(nuint left, nuint right) => Math.Min((UIntPtr)left, (UIntPtr)right);
}
