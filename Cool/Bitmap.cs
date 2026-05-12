using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Cool;

public unsafe sealed class Bitmap : IDisposable
{
    private readonly uint* _pBitmap;
    private readonly GCHandle _gcHandle;
    public readonly uint BitHighLimit;
    /// <summary>
    /// The size of the bitmap in bytes, which is determined by the highest bit position (BitHighLimit) that the bitmap supports.
    /// This property returns the total number of bytes allocated for the bitmap,
    /// which is determined by the formula: ((BitHighLimit >> 5) + 1) * sizeof(uint).
    /// </summary>
    public int Size => ((int)(BitHighLimit >> 5) + 1) * sizeof(uint);

    public Bitmap(uint bitHighLimit, string range)
    {
        BitHighLimit = bitHighLimit;
        var bitmap = new uint[(int)(BitHighLimit >> 5) + 1];
        _gcHandle = GCHandle.Alloc(bitmap, GCHandleType.Pinned);
        _pBitmap = (uint*)_gcHandle.AddrOfPinnedObject();
        string[] parts = range.Split(',');
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
    public void SetBit(uint pos) { if (pos <= BitHighLimit) { _pBitmap[pos >> 5] |= 1u << (int)(pos & 31); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetBit(uint pos) => (pos <= BitHighLimit) && ((_pBitmap[pos >> 5] & (1u << (int)(pos & 31))) != 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearBit(uint pos) { if (pos <= BitHighLimit) { _pBitmap[pos >> 5] &= ~(1u << (int)(pos & 31)); } }

    public void Dispose()
    {
        if (_gcHandle.IsAllocated) _gcHandle.Free();
        GC.SuppressFinalize(this);
    }

    public override string ToString()
    {
        // Build ranges in the same hex format used by the constructor: "START-END,POS,..."
        int words = (int)(BitHighLimit >> 5) + 1;
        int lastBits = (int)(BitHighLimit & 31) + 1;
        uint lastMask = (lastBits == 32) ? uint.MaxValue : ((1u << lastBits) - 1u);

        var sb = StringBuilderPool.Shared.Rent();
        try
        {
            bool first = true;
            bool inRange = false;
            uint rangeStart = 0, rangeEnd = 0;

            for (int wi = 0; wi < words; wi++)
            {
                uint w = _pBitmap[wi];
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
                        AppendRange(sb, rangeStart, rangeEnd);

                        rangeStart = rangeEnd = pos;
                    }

                    // clear lowest set bit
                    w &= w - 1u;
                }
            }

            if (inRange)
            {
                if (!first) sb.Append(',');
                AppendRange(sb, rangeStart, rangeEnd);
            }

            return sb.ToString();
        }
        finally
        {
            StringBuilderPool.Shared.Return(sb);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendRange(StringBuilder sb, uint rangeStart, uint rangeEnd)
    {
        if (rangeStart == rangeEnd)
        {
            AppendHex(sb, rangeStart);
        }
        else
        {
            AppendHex(sb, rangeStart);
            sb.Append('-');
            AppendHex(sb, rangeEnd);
        }
    }

    private static readonly string _hexDigits = "0123456789ABCDEF";
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendHex(StringBuilder sb, uint value)
    {
        if (value == 0u)
        {
            sb.Append('0');
            return;
        }

        const int buflen = 8;
        // max 8 hex digits for a uint
        char* buf = stackalloc char[buflen];
        // fill from the end backwards so digits end up in correct order
        int i = buflen;
        while (value != 0u)
        {
            uint nibble = value & 0xFu;
            buf[--i] = _hexDigits[(int)nibble];
            value >>= 4;
        }

        // bulk append the prepared range
        sb.Append(buf + i, buflen - i);
    }

    private static readonly int[] _multiplyDeBruijnBitPosition = [
        0, 1, 28, 2, 29, 14, 24, 3, 30, 22, 20, 15, 25, 17, 4, 8,
        31, 27, 13, 23, 21, 19, 16, 7, 26, 12, 18, 6, 11, 5, 10, 9
    ];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountTrailingZeros(uint v)
    {
        return _multiplyDeBruijnBitPosition[((uint)((v & -v) * 0x077CB531U)) >> 27];
    }

    /// <summary>
    /// Creates a Bitmap instance that will be automatically disposed when the current AppDomain is unloaded.
    /// This is useful for caching bitmaps that are intended to live for the duration of the application without needing explicit disposal.
    /// Note: 
    ///     Do not call Dispose on the returned Bitmap, as it will be automatically cleaned up on AppDomain unload.
    ///     Calling Dispose manually may lead to ObjectDisposedException if accessed afterward.
    /// </summary>
    /// <param name="bitHighLimit">The highest bit position that the bitmap will support.</param>
    /// <param name="range">A string representing the range of bits to set initially.</param>
    /// <returns>A Bitmap instance that will be automatically disposed when the current AppDomain is unloaded.</returns>
    public static Bitmap CreateStatic(uint bitHighLimit, string range)
    {
        Bitmap bmp = new(bitHighLimit, range);
        AppDomain.CurrentDomain.DomainUnload += (s, e) => bmp.Dispose();
        return bmp;
    }
}
