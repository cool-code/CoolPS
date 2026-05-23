using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct ULongDriver : INumberDriver<ulong>
{
    public ulong Zero => 0UL;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ShiftLeft(ulong value, int shift) => value << shift;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong AddByte(ulong left, byte right) => left + right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Negate(ulong value) => ~value + 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref ulong value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(ulong left, ulong right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong Min(ulong left, ulong right) => Math.Min(left, right);
}