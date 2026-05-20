using System.Numerics;
using System.Runtime.CompilerServices;
using Cool.NumberDrivers;

namespace Cool;

[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public readonly struct Range<T, TNumberDriver>(string range, T highLimit)
    where T : struct
    where TNumberDriver : struct, INumberDriver<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly RangeIterator<T, TNumberDriver> GetEnumerator()
    {
        return new RangeIterator<T, TNumberDriver>(range, highLimit);
    }
    public override string ToString() => range;
}

// Defining Range<T> as a generic static class is to avoid conflicts with System.Range and to provide type-safe factory methods for creating Range instances.
// With the Range<T>.Create method, users can directly create Range instances of specific types without worrying about type conversions or incorrect type parameters.
public static class Range<T>
where T : struct
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<byte, ByteDriver> Create(string range, byte highLimit)
    {
        return new Range<byte, ByteDriver>(range, highLimit);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<sbyte, SByteDriver> Create(string range, sbyte highLimit)
    {
        return new Range<sbyte, SByteDriver>(range, highLimit);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<short, ShortDriver> Create(string range, short highLimit)
    {
        return new Range<short, ShortDriver>(range, highLimit);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<ushort, UShortDriver> Create(string range, ushort highLimit)
    {
        return new Range<ushort, UShortDriver>(range, highLimit);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<int, IntDriver> Create(string range, int highLimit)
    {
        return new Range<int, IntDriver>(range, highLimit);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<uint, UIntDriver> Create(string range, uint highLimit)
    {
        return new Range<uint, UIntDriver>(range, highLimit);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<long, LongDriver> Create(string range, long highLimit)
    {
        return new Range<long, LongDriver>(range, highLimit);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<ulong, ULongDriver> Create(string range, ulong highLimit)
    {
        return new Range<ulong, ULongDriver>(range, highLimit);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<decimal, DecimalDriver> Create(string range, decimal highLimit)
    {
        return new Range<decimal, DecimalDriver>(range, highLimit);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<BigInteger, BigIntegerDriver> Create(string range, BigInteger highLimit)
    {
        return new Range<BigInteger, BigIntegerDriver>(range, highLimit);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Range<T, TNumberDriver> Create<TNumberDriver>(string range, T highLimit)
        where TNumberDriver : struct, INumberDriver<T>
    {
        return new Range<T, TNumberDriver>(range, highLimit);
    }
}
