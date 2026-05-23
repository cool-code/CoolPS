using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct DecimalDriver : INumberDriver<decimal>
{
    public decimal Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal ShiftLeft(decimal value, int shift) => value * (1 << shift);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal AddByte(decimal left, byte right) => left + right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal Negate(decimal value) => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref decimal value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(decimal left, decimal right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal Min(decimal left, decimal right) => Math.Min(left, right);
}