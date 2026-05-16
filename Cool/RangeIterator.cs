using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cool.NumberDrivers;

namespace Cool;

public unsafe ref struct RangeIterator<T, TNumberDriver>
    where T : unmanaged
    where TNumberDriver : struct, INumberDriver<T>
{
    private readonly int _length;
    private readonly T _highLimit;
    private readonly TNumberDriver _driver;
    private GCHandle _gcHandle;
    private readonly char* _ptr;

    private int _index;
    private T _currentValue;
    private T _endValue;
    private bool _inRangeMode;

    public readonly T Current => _currentValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal RangeIterator(string rangeStr, T highLimit)
    {
        _highLimit = highLimit;
        _driver = default;
        _length = rangeStr.Length;
        _gcHandle = GCHandle.Alloc(rangeStr, GCHandleType.Pinned);
        _ptr = (char*)_gcHandle.AddrOfPinnedObject().ToPointer();

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

        while (_index < _length && (_ptr[_index] == ',' || _ptr[_index] == ' '))
        {
            _index++;
        }

        if (_index >= _length) return false;

        T startVal = ParseNextNumber(out bool hasStart);
        if (!hasStart) return false;

        if (_index < _length && _ptr[_index] == '~')
        {
            _index++;

            T endVal = ParseNextNumber(out bool hasEnd);
            _currentValue = startVal;
            _endValue = hasEnd ? _driver.Min(_highLimit, endVal) : startVal;

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
    private T ParseNextNumber(out bool success)
    {
        success = false;
        if (_index >= _length) return _driver.Zero;

        while (_index < _length && _ptr[_index] == ' ') _index++;

        bool isNegative = false;
        if (_index < _length)
        {
            char c = _ptr[_index];
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

        T val = _driver.Zero;
        while (_index < _length)
        {
            char c = _ptr[_index];

            if (c == ',' || c == '~') break;

            if (c != ' ')
            {
                T hexChar = _driver.ParseHexChar(c);
                val = _driver.AccumulateHex(val, hexChar);
                success = true;
            }
            _index++;
        }

        if (isNegative && success)
        {
            val = _driver.Negate(val);
        }

        return val;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (_gcHandle.IsAllocated) _gcHandle.Free();
    }
}