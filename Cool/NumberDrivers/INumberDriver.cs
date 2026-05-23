namespace Cool.NumberDrivers;

public interface INumberDriver<T> where T : struct
{
    T Zero { get; }
    // T ParseHexChar(char c);
    // T AccumulateHex(T current, T hexValue);
    T ShiftLeft(T value, int shift);
    T AddByte(T left, byte right);
    T Negate(T value);
    void Increment(ref T value);
    bool LessThan(T left, T right);
    T Min(T left, T right);
}
