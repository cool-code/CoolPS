using System.Runtime.CompilerServices;
using Cool.NumberDrivers;

namespace Cool;

public struct RangeIterator<T, TNumberDriver>
    where T : struct
    where TNumberDriver : struct, INumberDriver<T>
{
    private readonly int _length;
    private readonly T _highLimit;
    private readonly TNumberDriver _driver;
    private readonly Unchecked.String _range;

    private int _index;
    private T _currentValue;
    private T _endValue;
    private bool _inRangeMode;

    private static readonly Unchecked.Array<byte> _hexTable = CreateHexTable();
    private static byte[] CreateHexTable()
    {
        var table = new byte[128];
        for (char c = '0'; c <= '9'; c++)
            table[c] = (byte)(c - '0');
        for (char c = 'A'; c <= 'F'; c++)
            table[c] = (byte)(c - 'A' + 10);
        for (char c = 'a'; c <= 'f'; c++)
            table[c] = (byte)(c - 'a' + 10);
        return table;
    }

    public readonly T Current => _currentValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal RangeIterator(string range, T highLimit)
    {
        _highLimit = highLimit;
        _driver = default;
        _length = range?.Length ?? 0;
        _range = range ?? string.Empty;

        _index = 0;
        _currentValue = _driver.Zero;
        _endValue = _driver.Zero;
        _inRangeMode = false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        if (_inRangeMode)
        {
            if (_driver.LessThan(_currentValue, _endValue))
            {
                _driver.Increment(ref _currentValue);
                return true;
            }
            _inRangeMode = false;
        }

        if (_index >= _length) return false;

        while (_index < _length && (_range[_index] == ',' || _range[_index] == ' '))
        {
            _index++;
        }

        if (_index >= _length) return false;

        if (!ParseNextNumber(out T startVal)) return false;

        if (_index < _length && _range[_index] == '~')
        {
            _index++;

            _currentValue = startVal;
            _endValue = ParseNextNumber(out T endVal) ? _driver.Min(_highLimit, endVal) : startVal;

            if (_driver.LessThan(_currentValue, _endValue))
            {
                _inRangeMode = true;
            }
        }
        else
        {
            _currentValue = startVal;
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ParseNextNumber(out T val)
    {
        val = _driver.Zero;
        if (_index >= _length) return false;

        while (_index < _length && _range[_index] == ' ') _index++;

        bool isNegative = false;
        if (_index < _length)
        {
            char c = _range[_index];
            if (c == '-')
            {
                isNegative = true;
                _index++;
            }
            else if (c == '+')
            {
                _index++;
            }
        }

        bool success = false;
        while (_index < _length)
        {
            char c = _range[_index];

            if (c == ',' || c == '~') break;

            if (c.IsAsciiHexDigit())
            {
                byte hexValue = _hexTable[c];
                val = _driver.ShiftLeft(val, 4);
                val = _driver.AddByte(val, hexValue);
                success = true;
            }
            _index++;
        }

        if (isNegative && success)
        {
            val = _driver.Negate(val);
        }

        return success;
    }
}