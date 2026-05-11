using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Cool;

public unsafe readonly struct Bitmap : IDisposable
{
    private readonly uint* _pbitmap;
    private readonly uint _bitHighLimit;

    public Bitmap(uint bitHighLimit, string ranage)
    {
        _bitHighLimit = bitHighLimit;
        int size = ((int)(bitHighLimit + 1) >> 5) * sizeof(uint);
        _pbitmap = (uint*)Marshal.AllocHGlobal(size);
        Marshal.Copy(new byte[size], 0, (IntPtr)_pbitmap, size);
        string[] parts = ranage.Split(',');
        foreach (var part in parts)
        {
            int dashIndex = part.IndexOf('-');
            if (dashIndex > 0)
            {
                uint start = uint.Parse(part.Substring(0, dashIndex), NumberStyles.HexNumber);
                uint end = Math.Min(bitHighLimit, uint.Parse(part.Substring(dashIndex + 1), NumberStyles.HexNumber));
                for (uint i = start; i <= end; i++) SetBit(i);
            }
            else
            {
                uint pos = uint.Parse(part, NumberStyles.HexNumber);
                SetBit(pos);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void SetBit(uint pos) { if (pos <= _bitHighLimit) { _pbitmap[pos >> 5] |= 1u << (int)(pos & 31); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool GetBit(uint pos) => (pos <= _bitHighLimit) && ((_pbitmap[pos >> 5] & (1u << (int)(pos & 31))) != 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void ClearBit(uint pos) { if (pos <= _bitHighLimit) { _pbitmap[pos >> 5] &= ~(1u << (int)(pos & 31)); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Dispose() => Marshal.FreeHGlobal((IntPtr)_pbitmap);

    private static readonly char[] _hexDigits = "0123456789ABCDEF".ToCharArray();
    public override string ToString()
    {
        // Build ranges in the same hex format used by the constructor: "START-END,POS,..."
        int words = (int)(_bitHighLimit >> 5) + 1;
        int lastBits = (int)(_bitHighLimit & 31) + 1;
        uint lastMask = (lastBits == 32) ? uint.MaxValue : ((1u << lastBits) - 1u);

        var sb = StringBuilderPool.Shared.Rent(256);
        try
        {
            bool first = true;
            bool inRange = false;
            uint rangeStart = 0, rangeEnd = 0;

            fixed (char* hex = _hexDigits)
            {
                for (int wi = 0; wi < words; wi++)
                {
                    uint w = _pbitmap[wi];
                    if (wi == words - 1) w &= lastMask;

                    while (w != 0u)
                    {
                        int tz = CountTrailingZeros(w);
                        uint pos = ((uint)wi << 5) + (uint)tz;

                        if (!inRange)
                        {
                            inRange = true;
                            rangeStart = rangeEnd = pos;
                        }
                        else if (pos == rangeEnd + 1)
                        {
                            rangeEnd = pos;
                        }
                        else
                        {
                            if (!first) { sb.Append(','); } else { first = false; }
                            AppendRange(sb, rangeStart, rangeEnd, hex);

                            rangeStart = rangeEnd = pos;
                        }

                        // clear lowest set bit
                        w &= w - 1u;
                    }
                }

                if (inRange)
                {
                    if (!first) sb.Append(',');
                    AppendRange(sb, rangeStart, rangeEnd, hex);
                }
            }

            return sb.ToString();
        }
        finally
        {
            StringBuilderPool.Shared.Return(sb);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendRange(StringBuilder sb, uint rangeStart, uint rangeEnd, char* hex)
    {
        if (rangeStart == rangeEnd)
        {
            AppendHex(sb, rangeStart, hex);
        }
        else
        {
            AppendHex(sb, rangeStart, hex);
            sb.Append('-');
            AppendHex(sb, rangeEnd, hex);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendHex(StringBuilder sb, uint value, char* hexDigits)
    {
        if (value == 0u)
        {
            sb.Append('0');
            return;
        }

        const int bufferLength = 8;
        // max 8 hex digits for a uint
        char* buf = stackalloc char[bufferLength];
        // fill from the end backwards so digits end up in correct order
        int i = bufferLength;
        while (value != 0u)
        {
            uint nibble = value & 0xFu;
            buf[--i] = hexDigits[nibble];
            value >>= 4;
        }

        // bulk append the prepared range
        sb.Append(buf + i, bufferLength - i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountTrailingZeros(uint v)
    {
        // v != 0 when called
        int c = 0;
        while ((v & 1u) == 0)
        {
            v >>= 1;
            c++;
        }
        return c;
    }
}
