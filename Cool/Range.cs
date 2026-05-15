using System.Runtime.CompilerServices;
using Cool.NumberDrivers;

namespace Cool;

using UIntRange = Range<uint, UIntDriver>;
using ULongRange = Range<ulong, ULongDriver>;

[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public readonly struct Range<T, TNumberDriver>(string range, T highLimit)
    where T : unmanaged
    where TNumberDriver : struct, INumberDriver<T>
{
    internal readonly string RangeString = range ?? string.Empty;
    internal readonly T HighLimit = highLimit;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly RangeIterator<T, TNumberDriver> GetEnumerator()
    {
        return new RangeIterator<T, TNumberDriver>(RangeString, HighLimit);
    }
}

public static class Range
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UIntRange Create(string range, uint highLimit = uint.MaxValue)
    {
        return new UIntRange(range, highLimit);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ULongRange Create(string range, ulong highLimit = ulong.MaxValue)
    {
        return new ULongRange(range, highLimit);
    }
}
