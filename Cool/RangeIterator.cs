using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cool.NumberDrivers;

namespace Cool;

public unsafe ref struct RangeIterator<T, TNumberDriver>
    where T : unmanaged
    where TNumberDriver : struct, INumberDriver<T>
{
    private readonly int _length;
    private readonly TNumberDriver _driver;
    private GCHandle _gcHandle;
    private readonly char* _ptr;

    private int _index;
    private T _currentValue;
    private T _endValue;
    private bool _inRangeMode;

    public readonly T Current => _currentValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal RangeIterator(string rangeStr, T HighLimit)
    {
        if (typeof(T) == typeof(uint) && typeof(TNumberDriver) == typeof(UIntDriver))
        {
            var driver = new UIntDriver(Unsafe.As<T, uint>(ref HighLimit));
            _driver = Unsafe.As<UIntDriver, TNumberDriver>(ref driver);
        }
        else if (typeof(T) == typeof(ulong) && typeof(TNumberDriver) == typeof(ULongDriver))
        {
            var driver = new ULongDriver(Unsafe.As<T, ulong>(ref HighLimit));
            _driver = Unsafe.As<ULongDriver, TNumberDriver>(ref driver);
        }
        else
        {
            throw new NotSupportedException($"The type '{typeof(T).Name}' is not supported by RangeIterator.");
        }
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
                _currentValue = _driver.Increment(_currentValue);
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

        T startVal = _driver.Zero;
        bool hasStart = false;
        while (_index < _length)
        {
            char c = _ptr[_index];
            if (c == ',' || c == '-') break;

            if (c != ' ')
            {
                T hexChar = _driver.ParseHexChar(c);
                startVal = _driver.AccumulateHex(startVal, hexChar);
                hasStart = true;
            }
            _index++;
        }

        if (!hasStart) return false;

        if (_index < _length && _ptr[_index] == '-')
        {
            _index++;

            T endVal = _driver.Zero;
            bool hasEnd = false;
            while (_index < _length)
            {
                char c = _ptr[_index];
                if (c == ',') break;

                if (c != ' ')
                {
                    T hexChar = _driver.ParseHexChar(c);
                    endVal = _driver.AccumulateHex(endVal, hexChar);
                    hasEnd = true;
                }
                _index++;
            }

            _currentValue = startVal;
            _endValue = hasEnd ? _driver.Min(_driver.HighLimit, endVal) : startVal;

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
    public void Dispose()
    {
        if (_gcHandle.IsAllocated)
        {
            _gcHandle.Free();
        }
    }
}