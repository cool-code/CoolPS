using System;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct ByteDriver : INumberDriver<byte>
{
    public byte Zero => 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ParseHexChar(char c) => Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte AccumulateHex(byte current, byte hexValue) => (byte)((current << 4) + hexValue);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte Negate(byte value) => (byte)(~value + 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref byte value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(byte left, byte right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte Min(byte left, byte right) => Math.Min(left, right);
}
