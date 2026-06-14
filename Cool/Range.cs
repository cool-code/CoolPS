using System.Diagnostics.CodeAnalysis;
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

    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref BatchRange<T> AsBatch() => ref Unsafe.AsRef<Range<T>, BatchRange<T>>(in this);
}

[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public readonly struct BatchRange<T>(string range, T highLimit)
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
    public static RangeIterator<nint, NIntDriver> GetEnumerator(this in Range<nint> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<nuint, NUIntDriver> GetEnumerator(this in Range<nuint> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<decimal, DecimalDriver> GetEnumerator(this in Range<decimal> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RangeIterator<BigInteger, BigIntegerDriver> GetEnumerator(this in Range<BigInteger> r) => new(r.ToString(), r.HighLimit);
}

public static class BatchRangeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<byte, ByteDriver> GetEnumerator(this in BatchRange<byte> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<sbyte, SByteDriver> GetEnumerator(this in BatchRange<sbyte> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<short, ShortDriver> GetEnumerator(this in BatchRange<short> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<ushort, UShortDriver> GetEnumerator(this in BatchRange<ushort> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<int, IntDriver> GetEnumerator(this in BatchRange<int> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<uint, UIntDriver> GetEnumerator(this in BatchRange<uint> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<long, LongDriver> GetEnumerator(this in BatchRange<long> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<ulong, ULongDriver> GetEnumerator(this in BatchRange<ulong> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<nint, NIntDriver> GetEnumerator(this in BatchRange<nint> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<nuint, NUIntDriver> GetEnumerator(this in BatchRange<nuint> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<decimal, DecimalDriver> GetEnumerator(this in BatchRange<decimal> r) => new(r.ToString(), r.HighLimit);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BatchRangeIterator<BigInteger, BigIntegerDriver> GetEnumerator(this in BatchRange<BigInteger> r) => new(r.ToString(), r.HighLimit);
}