#if NETFRAMEWORK
namespace System;

/// <summary>
/// https://learn.microsoft.com/en-us/dotnet/api/system.range
/// </summary>
public readonly struct Range(Index start, Index end)
{
    public Index Start { get; } = start;
    public Index End { get; } = end;
    public static Range All => new(Index.Start, Index.End);
    public static Range StartAt(Index start) => new(start, Index.End);
    public static Range EndAt(Index end) => new(Index.Start, end);
    public (int Offset, int Length) GetOffsetAndLength(int length)
    {
        int start = Start.GetOffset(length);
        int end = End.GetOffset(length);
        return (start, end - start);
    }
}
#endif