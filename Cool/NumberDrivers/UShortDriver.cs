using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct UShortDriver : INumberDriver<ushort>
{
    public ushort Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ParseHexChar(char c) => Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort AccumulateHex(ushort current, ushort hexValue) => (ushort)((current << 4) + hexValue);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort Negate(ushort value) => (ushort)(~value + 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref ushort value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(ushort left, ushort right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort Min(ushort left, ushort right) => Math.Min(left, right);
}
