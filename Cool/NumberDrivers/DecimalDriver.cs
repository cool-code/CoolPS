using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct DecimalDriver : INumberDriver<decimal>
{
    public decimal Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal ParseHexChar(char c) => Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal AccumulateHex(decimal current, decimal hexValue) => (current * 16) + hexValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal Negate(decimal value) => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref decimal value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(decimal left, decimal right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal Min(decimal left, decimal right) => Math.Min(left, right);
}