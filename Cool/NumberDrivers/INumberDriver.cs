namespace Cool.NumberDrivers;

public interface INumberDriver<T> where T : unmanaged
{
    T Zero { get; }
    T HighLimit { get; }
    T ParseHexChar(char c);
    T AccumulateHex(T current, T hexValue);
    T Increment(T value);
    bool LessThan(T left, T right);
    T Min(T left, T right);
}
