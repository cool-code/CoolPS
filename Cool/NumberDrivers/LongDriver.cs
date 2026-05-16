using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct LongDriver : INumberDriver<long>
{
    public long Zero => 0L;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ParseHexChar(char c) => Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long AccumulateHex(long current, long hexValue) => (current << 4) + hexValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Negate(long value) => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref long value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(long left, long right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Min(long left, long right) => Math.Min(left, right);
}