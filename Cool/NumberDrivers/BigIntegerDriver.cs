using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

public readonly struct BigIntegerDriver : INumberDriver<BigInteger>
{
    public BigInteger Zero => BigInteger.Zero;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigInteger ShiftLeft(BigInteger value, int shift) => value << shift;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigInteger AddByte(BigInteger left, byte right) => left + right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigInteger Negate(BigInteger value) => -value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Increment(ref BigInteger value) => ++value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool LessThan(BigInteger left, BigInteger right) => left < right;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BigInteger Min(BigInteger left, BigInteger right) => BigInteger.Min(left, right);
}
