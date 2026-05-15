using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct ULongDriver(ulong highLimit) : INumberDriver<ulong>
{
    public ulong Zero => 0ul;
    public ulong HighLimit { get; } = highLimit;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ParseHexChar(char c) => Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong AccumulateHex(ulong current, ulong hexValue) => (current << 4) + hexValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Increment(ulong value) => value + 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(ulong left, ulong right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Min(ulong left, ulong right) => Math.Min(left, right);
}