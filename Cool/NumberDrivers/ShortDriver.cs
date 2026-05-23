using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct ShortDriver : INumberDriver<short>
{
    public short Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ShiftLeft(short value, int shift) => (short)(value << shift);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short AddByte(short left, byte right) => (short)(left + right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short Negate(short value) => (short)-value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref short value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(short left, short right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short Min(short left, short right) => Math.Min(left, right);
}
