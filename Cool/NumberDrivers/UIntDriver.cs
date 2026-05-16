using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct UIntDriver : INumberDriver<uint>
{
    public uint Zero => 0U;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ParseHexChar(char c) => Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint AccumulateHex(uint current, uint hexValue) => (current << 4) + hexValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Negate(uint value) => ~value + 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref uint value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(uint left, uint right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint Min(uint left, uint right) => Math.Min(left, right);
}
