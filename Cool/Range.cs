using System.Numerics;
using System.Runtime.CompilerServices;
using Cool.NumberDrivers;

namespace Cool;

[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public readonly struct Range<T>(string range, T highLimit)
    where T : struct
{
    public readonly T HighLimit = highLimit;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => range;
}

public static class RangeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<byte, ByteDriver> GetEnumerator(this in Range<byte> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<sbyte, SByteDriver> GetEnumerator(this in Range<sbyte> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<short, ShortDriver> GetEnumerator(this in Range<short> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<ushort, UShortDriver> GetEnumerator(this in Range<ushort> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<int, IntDriver> GetEnumerator(this in Range<int> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<uint, UIntDriver> GetEnumerator(this in Range<uint> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<long, LongDriver> GetEnumerator(this in Range<long> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<ulong, ULongDriver> GetEnumerator(this in Range<ulong> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<decimal, DecimalDriver> GetEnumerator(this in Range<decimal> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<BigInteger, BigIntegerDriver> GetEnumerator(this in Range<BigInteger> r) => new(r.ToString(), r.HighLimit);
}