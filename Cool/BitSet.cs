using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Cool;

public unsafe sealed class BitSet : IDisposable
{
    public readonly uint BitHighLimit;
    private readonly int _wordCount;
    private uint* _pBitMap;

    /// <summary>
    /// The size of the bitmap in bytes, which is determined by the highest bit position (BitHighLimit) that the bitmap supports.
    /// This property returns the total number of bytes allocated for the bitmap,
    /// which is determined by the formula: ((BitHighLimit >> 5) + 1) * sizeof(uint).
    /// </summary>
    public int AllocatedSize => _wordCount * sizeof(uint);

    public BitSet(uint bitHighLimit)
    {
        BitHighLimit = bitHighLimit;
        _wordCount = (int)(bitHighLimit >> 5) + 1;
        _pBitMap = (uint*)Marshal.AllocHGlobal(AllocatedSize).ToPointer();
        ClearAll();
    }
    public void Dispose()
    {
        if (_pBitMap != null)
        {
            Marshal.FreeHGlobal((IntPtr)_pBitMap);
            _pBitMap = null;
        }
        GC.SuppressFinalize(this);
    }
    ~BitSet() => Dispose();

    /// <summary>
    /// Sets the bits specified by the input range string. The range string can contain individual bit positions or ranges of bit positions, separated by commas.
    /// For example, the range string "0-3,5,7-FF" would set bits 0 through 3, bit 5, and bits 7 through 255 (0xFF). 
    /// The method parses the range string, determines which bits to set based on the specified ranges, and updates the bitmap accordingly.
    /// </summary>
    /// <param name="range">
    /// A string representing the range of bits to set. 
    /// The format of the range string should be a comma-separated list of individual bit positions or ranges of bit positions.
    /// Each range can be specified in hexadecimal format.
    /// Examples of valid range strings include:
    /// - "0-3,5,7-FF": Sets bits 0 through 3, bit 5, and bits 7 through 255 (0xFF).
    /// - "10,20-2F": Sets bit 10 and bits 20 through 47 (0x2F).
    /// - "1-1F": Sets bits 1 through 31 (0x1F).
    /// The method will parse the range string, determine which bits to set based on the specified ranges, and update the bitmap accordingly.
    /// </param>
    /// <returns>The current BitSet instance with the specified bits set.</returns>
    public BitSet SetRange(string range)
    {
        if (string.IsNullOrWhiteSpace(range)) return this;
        foreach (uint pos in Range.Create(range, BitHighLimit)) SetBit(pos);
        return this;
    }

    public BitSet ClearRange(string range)
    {
        if (string.IsNullOrWhiteSpace(range)) return this;
        foreach (uint pos in Range.Create(range, BitHighLimit)) ClearBit(pos);
        return this;
    }

    public BitSet InvertRange(string range)
    {
        if (string.IsNullOrWhiteSpace(range)) return this;
        foreach (uint pos in Range.Create(range, BitHighLimit)) InvertBit(pos);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet SetAll()
    {
        for (int i = 0; i < _wordCount; i++) _pBitMap[i] = uint.MaxValue;
        return this;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet ClearAll()
    {
        uint* ptr = _pBitMap;
        if (ptr == null) return this;
        int count = _wordCount;
        for (int i = 0; i < count; i++) ptr[i] = 0u;
        return this;
    }

    public BitSet Invert()
    {
        for (int i = 0; i < _wordCount; i++) _pBitMap[i] = ~_pBitMap[i];
        return this;
    }

    public BitSet Union(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        for (int i = 0; i < _wordCount; i++) _pBitMap[i] |= other._pBitMap[i];
        return this;
    }

    public BitSet Intersect(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        for (int i = 0; i < _wordCount; i++) _pBitMap[i] &= other._pBitMap[i];
        return this;
    }

    public BitSet Difference(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        for (int i = 0; i < _wordCount; i++) _pBitMap[i] &= ~other._pBitMap[i];
        return this;
    }

    public BitSet SymmetricDifference(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        for (int i = 0; i < _wordCount; i++) _pBitMap[i] ^= other._pBitMap[i];
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetBit(uint pos) { if (pos <= BitHighLimit) { _pBitMap[pos >> 5] |= 1u << (int)(pos & 31); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetBit(uint pos) => (pos <= BitHighLimit) && ((_pBitMap[pos >> 5] & (1u << (int)(pos & 31))) != 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearBit(uint pos) { if (pos <= BitHighLimit) { _pBitMap[pos >> 5] &= ~(1u << (int)(pos & 31)); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void InvertBit(uint pos) { if (pos <= BitHighLimit) { _pBitMap[pos >> 5] ^= 1u << (int)(pos & 31); } }
    public override string ToString()
    {
        // Build ranges in the same hex format used by the constructor: "START-END,POS,..."
        int lastBits = (int)(BitHighLimit & 31) + 1;
        uint lastMask = (lastBits == 32) ? uint.MaxValue : ((1u << lastBits) - 1u);

        var sb = StringBuilderPool.Shared.Rent();
        try
        {
            bool first = true;
            bool inRange = false;
            uint rangeStart = 0, rangeEnd = 0;
            char* buf = stackalloc char[8];

            for (int wi = 0; wi < _wordCount; wi++)
            {
                uint w = _pBitMap[wi];
                if (wi == _wordCount - 1) w &= lastMask;

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
                        AppendRange(sb, rangeStart, rangeEnd, buf);

                        rangeStart = rangeEnd = pos;
                    }

                    // clear lowest set bit
                    w &= w - 1u;
                }
            }

            if (inRange)
            {
                if (!first) sb.Append(',');
                AppendRange(sb, rangeStart, rangeEnd, buf);
            }

            return sb.ToString();
        }
        finally
        {
            StringBuilderPool.Shared.Return(sb);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendRange(StringBuilder sb, uint rangeStart, uint rangeEnd, char* buf)
    {
        if (rangeStart == rangeEnd)
        {
            AppendHex(sb, rangeStart, buf);
        }
        else
        {
            AppendHex(sb, rangeStart, buf);
            sb.Append('-');
            AppendHex(sb, rangeEnd, buf);
        }
    }

    private static readonly string _hexDigits = "0123456789ABCDEF";
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendHex(StringBuilder sb, uint value, char* buf)
    {
        if (value == 0u)
        {
            sb.Append('0');
            return;
        }

        // max 8 hex digits for a uint
        // fill from the end backwards so digits end up in correct order
        int i = 8;
        while (value != 0u)
        {
            uint nibble = value & 0xFu;
            buf[--i] = _hexDigits[(int)nibble];
            value >>= 4;
        }

        // bulk append the prepared range
        sb.Append(buf + i, 8 - i);
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
    /// Creates a BitSet instance that will be automatically disposed when the current AppDomain is unloaded.
    /// This is useful for caching bitsets that are intended to live for the duration of the application without needing explicit disposal.
    /// Note: 
    ///     Do not call Dispose on the returned BitSet, as it will be automatically cleaned up on AppDomain unload.
    ///     Calling Dispose manually may lead to ObjectDisposedException if accessed afterward.
    /// </summary>
    /// <param name="bitHighLimit">The highest bit position that the bitset will support.</param>
    /// <param name="range">A string representing the range of bits to set initially.</param>
    /// <returns>A BitSet instance that will be automatically disposed when the current AppDomain is unloaded.</returns>
    public static BitSet CreateStatic(string range, uint bitHighLimit)
    {
        return CreateStatic(bitHighLimit).SetRange(range);
    }
    public static BitSet CreateStatic(uint bitHighLimit)
    {
        BitSet bitSet = new(bitHighLimit);
        AppDomain.CurrentDomain.DomainUnload += (s, e) => bitSet.Dispose();
        return bitSet;
    }
}
