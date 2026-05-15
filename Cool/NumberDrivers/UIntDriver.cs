using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct UIntDriver(uint highLimit) : INumberDriver<uint>
{
    public uint Zero => 0u;
    public uint HighLimit { get; } = highLimit;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ParseHexChar(char c) => Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint AccumulateHex(uint current, uint hexValue) => (current << 4) + hexValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Increment(uint value) => value + 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(uint left, uint right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Min(uint left, uint right) => Math.Min(left, right);
}
