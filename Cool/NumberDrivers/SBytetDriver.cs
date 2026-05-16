using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct SByteDriver : INumberDriver<sbyte>
{
    public sbyte Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte ParseHexChar(char c) => (sbyte)Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte AccumulateHex(sbyte current, sbyte hexValue) => (sbyte)((current << 4) + hexValue);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte Negate(sbyte value) => (sbyte)-value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref sbyte value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(sbyte left, sbyte right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte Min(sbyte left, sbyte right) => Math.Min(left, right);
}
