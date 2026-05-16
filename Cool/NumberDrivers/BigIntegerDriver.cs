using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct BigIntegerDriver : INumberDriver<BigInteger>
{
    public BigInteger Zero => BigInteger.Zero;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigInteger ParseHexChar(char c) => Hex.Lookup(c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigInteger AccumulateHex(BigInteger current, BigInteger hexValue) => (current << 4) + hexValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigInteger Negate(BigInteger value) => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref BigInteger value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(BigInteger left, BigInteger right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigInteger Min(BigInteger left, BigInteger right) => BigInteger.Min(left, right);
}
