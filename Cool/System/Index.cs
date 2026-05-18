#if NETFRAMEWORK
namespace System;

/// <summary>
/// https://learn.microsoft.com/en-us/dotnet/api/system.index
/// </summary>
public readonly struct Index(int value, bool fromEnd = false)
{
    public int Value { get; } = value;
    public bool IsFromEnd { get; } = fromEnd;
    public static Index Start => new(0);
    public static Index End => new(0, fromEnd: true);
    public static Index FromStart(int value) => new(value);
    public static Index FromEnd(int value) => new(value, fromEnd: true);
    public int GetOffset(int length) => IsFromEnd ? length - Value : Value;

    public static implicit operator Index(int value) => FromStart(value);
}
#endif