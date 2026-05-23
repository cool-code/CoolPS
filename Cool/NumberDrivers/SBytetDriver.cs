using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct SByteDriver : INumberDriver<sbyte>
{
    public sbyte Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ShiftLeft(sbyte value, int shift) => (sbyte)(value << shift);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte AddByte(sbyte left, byte right) => (sbyte)(left + right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte Negate(sbyte value) => (sbyte)-value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref sbyte value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(sbyte left, sbyte right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte Min(sbyte left, sbyte right) => Math.Min(left, right);
}
