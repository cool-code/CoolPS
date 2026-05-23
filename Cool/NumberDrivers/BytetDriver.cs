using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct ByteDriver : INumberDriver<byte>
{
    public byte Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ShiftLeft(byte value, int shift) => (byte)(value << shift);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte AddByte(byte left, byte right) => (byte)(left + right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte Negate(byte value) => (byte)(~value + 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref byte value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(byte left, byte right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte Min(byte left, byte right) => Math.Min(left, right);
}
