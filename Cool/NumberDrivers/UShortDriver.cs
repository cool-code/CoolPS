using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct UShortDriver : INumberDriver<ushort>
{
    public ushort Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ShiftLeft(ushort value, int shift) => (ushort)(value << shift);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort AddByte(ushort left, byte right) => (ushort)(left + right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort Negate(ushort value) => (ushort)(~value + 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref ushort value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(ushort left, ushort right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort Min(ushort left, ushort right) => Math.Min(left, right);
}
