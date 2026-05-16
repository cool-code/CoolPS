using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct IntDriver : INumberDriver<int>
{
    public int Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ParseHexChar(char c) => Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int AccumulateHex(int current, int hexValue) => (current << 4) + hexValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Negate(int value) => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref int value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(int left, int right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Min(int left, int right) => Math.Min(left, right);
}
