using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct LongDriver : INumberDriver<long>
{
    public long Zero => 0L;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ShiftLeft(long value, int shift) => value << shift;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long AddByte(long left, byte right) => left + right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Negate(long value) => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref long value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(long left, long right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long Min(long left, long right) => Math.Min(left, right);
}