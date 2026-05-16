using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Cool;

public unsafe sealed class BitSet : IDisposable
{
    #region Fields
    public readonly uint BitHighLimit;
    public readonly int AllocatedSize;
    private readonly int _wordCount;
    private readonly uint _tailMask;
    private uint* _pBitMap;
    #endregion

    #region Constructors and Disposal
    public BitSet(uint bitHighLimit)
    {
        BitHighLimit = bitHighLimit;
        _wordCount = (int)((bitHighLimit >> 5) + 1);
        AllocatedSize = _wordCount * sizeof(uint);
        int remainingBits = (int)(bitHighLimit & 31);
        _tailMask = (remainingBits == 31) ? 0xFFFFFFFFu : (1u << (remainingBits + 1)) - 1u;
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
    #endregion

    #region Operator Overloads
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet operator ~(BitSet set) => set.Invert();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet operator |(BitSet left, BitSet right) => left.Union(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet operator &(BitSet left, BitSet right) => left.Intersect(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet operator ^(BitSet left, BitSet right) => left.SymmetricDifference(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet operator -(BitSet left, BitSet right) => left.Difference(right);
    #endregion

    #region Bit Access Methods
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetBit(uint pos) { if (pos <= BitHighLimit) { _pBitMap[pos >> 5] |= 1u << (int)(pos & 31); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetBit(uint pos) => (pos <= BitHighLimit) && ((_pBitMap[pos >> 5] & (1u << (int)(pos & 31))) != 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ClearBit(uint pos) { if (pos <= BitHighLimit) { _pBitMap[pos >> 5] &= ~(1u << (int)(pos & 31)); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void InvertBit(uint pos) { if (pos <= BitHighLimit) { _pBitMap[pos >> 5] ^= 1u << (int)(pos & 31); } }
    #endregion

    #region Bit Manipulation Methods
    /// <summary>
    /// Sets the bits specified by the input range string.
    /// For example, the range string "0~3,5,7~FF" would set bits 0 through 3, bit 5, and bits 7 through 255 (0xFF). 
    /// The method parses the range string, determines which bits to set based on the specified ranges, and updates the bitset accordingly.
    /// </summary>
    /// <param name="range">
    /// A string representing the range of bits to set. 
    /// The format of the range string should be a comma-separated list of individual bit positions or ranges of bit positions.
    /// Each range can be specified in hexadecimal format.
    /// Examples of valid range strings include:
    /// - "0~3,5,7~FF": Sets bits 0 through 3, bit 5, and bits 7 through 255 (0xFF).
    /// - "10,20~2F": Sets bit 10 and bits 20 through 47 (0x2F).
    /// - "1~1F": Sets bits 1 through 31 (0x1F).
    /// The method will parse the range string, determine which bits to set based on the specified ranges, and update the bitset accordingly.
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
        Unsafe.InitBlock(_pBitMap, 0xFF, (uint)AllocatedSize);
        // Mask off unused bits in the last word to ensure they remain 0
        _pBitMap[_wordCount - 1] &= _tailMask;
        return this;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet ClearAll()
    {
        Unsafe.InitBlock(_pBitMap, 0, (uint)AllocatedSize);
        return this;
    }
    #endregion

    #region Set Operations
    public BitSet Invert()
    {
        uint* ptr = _pBitMap;
        int i = 0;
        int count = _wordCount;
        // Process 8 uints at a time to leverage CPU's instruction-level parallelism (ILP)
        // for maximum performance when inverting large bitsets.
        for (; i <= count - 8; i += 8)
        {
            ptr[i] = ~ptr[i];
            ptr[i + 1] = ~ptr[i + 1];
            ptr[i + 2] = ~ptr[i + 2];
            ptr[i + 3] = ~ptr[i + 3];
            ptr[i + 4] = ~ptr[i + 4];
            ptr[i + 5] = ~ptr[i + 5];
            ptr[i + 6] = ~ptr[i + 6];
            ptr[i + 7] = ~ptr[i + 7];
        }
        // Clean up the remaining tail slots (less than 8)
        for (; i < count; i++)
        {
            ptr[i] = ~ptr[i];
        }
        // Mask off unused bits in the last word to ensure they remain 0 after inversion
        ptr[count - 1] &= _tailMask;
        return this;
    }

    public BitSet Union(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        uint* ptr = _pBitMap;
        uint* otherPtr = other._pBitMap;
        int i = 0;
        int count = _wordCount;
        // Similar ILP optimization as in Invert method, processing 8 uints at a time for maximum performance.
        for (; i <= count - 8; i += 8)
        {
            ptr[i] |= otherPtr[i];
            ptr[i + 1] |= otherPtr[i + 1];
            ptr[i + 2] |= otherPtr[i + 2];
            ptr[i + 3] |= otherPtr[i + 3];
            ptr[i + 4] |= otherPtr[i + 4];
            ptr[i + 5] |= otherPtr[i + 5];
            ptr[i + 6] |= otherPtr[i + 6];
            ptr[i + 7] |= otherPtr[i + 7];
        }
        // Clean up the remaining tail slots (less than 8)
        for (; i < count; i++)
        {
            ptr[i] |= otherPtr[i];
        }
        return this;
    }

    public BitSet Intersect(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        uint* ptr = _pBitMap;
        uint* otherPtr = other._pBitMap;
        int i = 0;
        int count = _wordCount;
        // Similar ILP optimization as in Invert method, processing 8 uints at a time for maximum performance.
        for (; i <= count - 8; i += 8)
        {
            ptr[i] &= otherPtr[i];
            ptr[i + 1] &= otherPtr[i + 1];
            ptr[i + 2] &= otherPtr[i + 2];
            ptr[i + 3] &= otherPtr[i + 3];
            ptr[i + 4] &= otherPtr[i + 4];
            ptr[i + 5] &= otherPtr[i + 5];
            ptr[i + 6] &= otherPtr[i + 6];
            ptr[i + 7] &= otherPtr[i + 7];
        }
        // Clean up the remaining tail slots (less than 8)
        for (; i < count; i++)
        {
            ptr[i] &= otherPtr[i];
        }
        return this;
    }

    public BitSet Difference(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        uint* ptr = _pBitMap;
        uint* otherPtr = other._pBitMap;
        int i = 0;
        int count = _wordCount;
        // Similar ILP optimization as in Invert method, processing 8 uints at a time for maximum performance.
        for (; i <= count - 8; i += 8)
        {
            ptr[i] &= ~otherPtr[i];
            ptr[i + 1] &= ~otherPtr[i + 1];
            ptr[i + 2] &= ~otherPtr[i + 2];
            ptr[i + 3] &= ~otherPtr[i + 3];
            ptr[i + 4] &= ~otherPtr[i + 4];
            ptr[i + 5] &= ~otherPtr[i + 5];
            ptr[i + 6] &= ~otherPtr[i + 6];
            ptr[i + 7] &= ~otherPtr[i + 7];
        }
        // Clean up the remaining tail slots (less than 8)
        for (; i < count; i++)
        {
            ptr[i] &= ~otherPtr[i];
        }
        return this;
    }

    public BitSet SymmetricDifference(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        uint* ptr = _pBitMap;
        uint* otherPtr = other._pBitMap;
        int i = 0;
        int count = _wordCount;
        // Similar ILP optimization as in Invert method, processing 8 uints at a time for maximum performance.
        for (; i <= count - 8; i += 8)
        {
            ptr[i] ^= otherPtr[i];
            ptr[i + 1] ^= otherPtr[i + 1];
            ptr[i + 2] ^= otherPtr[i + 2];
            ptr[i + 3] ^= otherPtr[i + 3];
            ptr[i + 4] ^= otherPtr[i + 4];
            ptr[i + 5] ^= otherPtr[i + 5];
            ptr[i + 6] ^= otherPtr[i + 6];
            ptr[i + 7] ^= otherPtr[i + 7];
        }
        // Clean up the remaining tail slots (less than 8)
        for (; i < count; i++)
        {
            ptr[i] ^= otherPtr[i];
        }
        return this;
    }
    #endregion

    #region Query Methods
    // The Cardinality method counts the number of bits that are set to 1 in the bitset,
    // which is also known as the population count or Hamming weight.
    public int Cardinality()
    {
        uint* ptr = _pBitMap;
        int count = _wordCount;
        int total = 0;
        int i = 0;
        // It iterates through the bitmap and uses a helper method PopCount
        // to count the number of set bits in each uint word.
        // The method processes 8 uints at a time for performance optimization,
        // leveraging CPU's instruction-level parallelism (ILP) when counting bits in large bitsets.
        for (; i <= count - 8; i += 8)
        {
            total += PopCount(ptr[i]) + PopCount(ptr[i + 1]) +
                     PopCount(ptr[i + 2]) + PopCount(ptr[i + 3]) +
                     PopCount(ptr[i + 4]) + PopCount(ptr[i + 5]) +
                     PopCount(ptr[i + 6]) + PopCount(ptr[i + 7]);
        }
        for (; i < count; i++) total += PopCount(ptr[i]);
        return total;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int PopCount(uint x)
    {
        // classic SWAR (SIMD Within A Register) branchless bit counting magic that uses only arithmetic and bitwise operations,
        // JIT compiles it into just a few register add/subtract instructions with 0 memory reads, achieving performance of around 1 nanosecond!
        x -= (x >> 1) & 0x55555555;
        x = (x & 0x33333333) + ((x >> 2) & 0x33333333);
        x = (x + (x >> 4)) & 0x0F0F0F0F;
        return (int)((x * 0x01010101) >> 24);
    }

    public bool IsSubsetOf(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch");
        uint* ptr = _pBitMap;
        uint* otherPtr = other._pBitMap;
        for (int i = 0; i < _wordCount; i++) if ((ptr[i] & ~otherPtr[i]) != 0) return false;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsSupersetOf(BitSet other) => other.IsSubsetOf(this);

    public bool Overlaps(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch");
        uint* ptr = _pBitMap;
        uint* otherPtr = other._pBitMap;
        for (int i = 0; i < _wordCount; i++) if ((ptr[i] & otherPtr[i]) != 0) return true;
        return false;
    }

    public bool SetEquals(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch");
        uint* ptr = _pBitMap;
        uint* otherPtr = other._pBitMap;
        for (int i = 0; i < _wordCount; i++) if (ptr[i] != otherPtr[i]) return false;
        return true;
    }

    public bool IsEmpty()
    {
        uint* ptr = _pBitMap;
        for (int i = 0; i < _wordCount; i++) if (ptr[i] != 0) return false;
        return true;
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        // Build ranges in the same hex format used by the constructor: "START-END,POS,..."

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
                if (wi == _wordCount - 1) w &= _tailMask; // mask off unused bits in the last word

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
    #endregion

    #region Helper Methods for ToString

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
        return Unsafe.Read(_multiplyDeBruijnBitPosition, (int)(((uint)((v & -v) * 0x077CB531U)) >> 27));
    }
    #endregion

    #region Static Factory Methods
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
    #endregion

}
