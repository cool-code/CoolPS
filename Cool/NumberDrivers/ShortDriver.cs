using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct ShortDriver : INumberDriver<short>
{
    public short Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ParseHexChar(char c) => Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short AccumulateHex(short current, short hexValue) => (short)((current << 4) + hexValue);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short Negate(short value) => (short)-value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref short value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(short left, short right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short Min(short left, short right) => Math.Min(left, right);
}
