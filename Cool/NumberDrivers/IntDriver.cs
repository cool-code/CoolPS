using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct IntDriver : INumberDriver<int>
{
    public int Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ShiftLeft(int value, int shift) => value << shift;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int AddByte(int left, byte right) => left + right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Negate(int value) => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref int value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(int left, int right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Min(int left, int right) => Math.Min(left, right);
}
