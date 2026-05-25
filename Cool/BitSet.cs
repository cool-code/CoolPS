using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cool;

public unsafe sealed class BitSet : IEquatable<BitSet>
{
    #region Fields
    public readonly uint BitHighLimit;
    public readonly int AllocatedSize;
    private readonly int _wordCount;
    private readonly uint _tailMask;
    private readonly Unchecked.Array<uint> _bitmap;
    #endregion

    #region Constructors and Disposal
    public BitSet(uint bitHighLimit, string range) : this(bitHighLimit) { Set(range); }
    public BitSet(uint bitHighLimit)
    {
        BitHighLimit = bitHighLimit;
        _wordCount = (int)((bitHighLimit >> 5) + 1);
        AllocatedSize = _wordCount * sizeof(uint);
        int remainingBits = (int)(bitHighLimit & 31);
        _tailMask = (remainingBits == 31) ? uint.MaxValue : (1u << (remainingBits + 1)) - 1u;
        _bitmap = new uint[_wordCount];
    }
    #endregion

    #region Operator Overloads
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void operator |=(BitSet other) => Union(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void operator &=(BitSet other) => Intersect(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void operator ^=(BitSet other) => SymmetricDifference(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void operator -=(BitSet other) => Difference(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet operator |(BitSet left, BitSet right) => left.Clone().Union(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet operator &(BitSet left, BitSet right) => left.Clone().Intersect(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet operator ^(BitSet left, BitSet right) => left.Clone().SymmetricDifference(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet operator -(BitSet left, BitSet right) => left.Clone().Difference(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet operator ~(BitSet set) => set.Invert();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(BitSet left, BitSet right) => left.IsProperSubsetOf(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(BitSet left, BitSet right) => left.IsSubsetOf(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(BitSet left, BitSet right) => left.IsProperSupersetOf(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(BitSet left, BitSet right) => left.IsSupersetOf(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BitSet? left, BitSet? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.SetEquals(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BitSet? left, BitSet? right) => !(left == right);
    #endregion

    #region Indexer
    public bool this[uint pos]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Contains(pos);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (value) Set(pos);
            else Clear(pos);
        }
    }
    #endregion

    #region Bit Access Methods
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Set(uint* pbitmap, uint pos) { if (pos <= BitHighLimit) { pbitmap[pos >> 5] |= 1u << (int)(pos & 31); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool Contains(uint* pbitmap, uint pos) => (pos <= BitHighLimit) && ((pbitmap[pos >> 5] & (1u << (int)(pos & 31))) != 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Clear(uint* pbitmap, uint pos) { if (pos <= BitHighLimit) { pbitmap[pos >> 5] &= ~(1u << (int)(pos & 31)); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Invert(uint* pbitmap, uint pos) { if (pos <= BitHighLimit) { pbitmap[pos >> 5] ^= 1u << (int)(pos & 31); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(uint pos) { if (pos <= BitHighLimit) { _bitmap[pos >> 5] |= 1u << (int)(pos & 31); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(uint pos) => (pos <= BitHighLimit) && ((_bitmap[pos >> 5] & (1u << (int)(pos & 31))) != 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(uint pos) { if (pos <= BitHighLimit) { _bitmap[pos >> 5] &= ~(1u << (int)(pos & 31)); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Invert(uint pos) { if (pos <= BitHighLimit) { _bitmap[pos >> 5] ^= 1u << (int)(pos & 31); } }
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
    public BitSet Set(string range)
    {
        if (string.IsNullOrWhiteSpace(range)) return this;
        fixed (uint* ptr = _bitmap)
        {
            foreach (uint pos in new Range<uint>(range, BitHighLimit)) Set(ptr, pos);
        }
        return this;
    }

    public BitSet Clear(string range)
    {
        if (string.IsNullOrWhiteSpace(range)) return this;
        fixed (uint* ptr = _bitmap)
        {
            foreach (uint pos in new Range<uint>(range, BitHighLimit)) Clear(ptr, pos);
        }
        return this;
    }

    public BitSet Invert(string range)
    {
        if (string.IsNullOrWhiteSpace(range)) return this;
        fixed (uint* ptr = _bitmap)
        {
            foreach (uint pos in new Range<uint>(range, BitHighLimit)) Invert(ptr, pos);
        }
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet SetAll()
    {
        fixed (uint* ptr = _bitmap)
        {
            Unsafe.InitBlock(ptr, 0xFF, (uint)AllocatedSize);
            // Mask off unused bits in the last word to ensure they remain 0
            ptr[_wordCount - 1] &= _tailMask;
        }
        return this;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet Clear()
    {
        fixed (uint* ptr = _bitmap)
        {
            Unsafe.InitBlock(ptr, 0, (uint)AllocatedSize);
        }
        return this;
    }
    #endregion

    #region Set Operations
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet Invert()
    {
        fixed (uint* ptr = _bitmap)
        {
            int i = 0;
            int count = _wordCount;
            if (IntPtr.Size == 8)
            {
                for (; i <= count - 8; i += 8)
                {
                    ulong* p = (ulong*)(ptr + i);
                    p[0] = ~p[0]; p[1] = ~p[1]; p[2] = ~p[2]; p[3] = ~p[3];
                }
            }
            else
            {
                for (; i <= count - 8; i += 8)
                {
                    uint* p = ptr + i;
                    p[0] = ~p[0]; p[1] = ~p[1]; p[2] = ~p[2]; p[3] = ~p[3];
                    p[4] = ~p[4]; p[5] = ~p[5]; p[6] = ~p[6]; p[7] = ~p[7];
                }
            }

            for (; i < count; i++) ptr[i] = ~ptr[i];
            ptr[_wordCount - 1] &= _tailMask;
        }
        return this;
    }

    public BitSet Union(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        fixed (uint* ptr = _bitmap, otherPtr = other._bitmap)
        {
            int i = 0;
            int count = _wordCount;
            if (IntPtr.Size == 8)
            {
                for (; i <= count - 8; i += 8)
                {
                    ulong* p = (ulong*)(ptr + i);
                    ulong* o = (ulong*)(otherPtr + i);
                    p[0] |= o[0]; p[1] |= o[1]; p[2] |= o[2]; p[3] |= o[3];
                }
            }
            else
            {
                for (; i <= count - 8; i += 8)
                {
                    uint* p = ptr + i;
                    uint* o = otherPtr + i;
                    p[0] |= o[0];
                    p[1] |= o[1];
                    p[2] |= o[2];
                    p[3] |= o[3];
                    p[4] |= o[4];
                    p[5] |= o[5];
                    p[6] |= o[6];
                    p[7] |= o[7];
                }

            }
            for (; i < count; i++) ptr[i] |= otherPtr[i];
        }
        return this;
    }

    public BitSet Intersect(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        fixed (uint* ptr = _bitmap, otherPtr = other._bitmap)
        {
            int i = 0;
            int count = _wordCount;
            if (IntPtr.Size == 8)
            {
                for (; i <= count - 8; i += 8)
                {
                    ulong* p = (ulong*)(ptr + i);
                    ulong* o = (ulong*)(otherPtr + i);
                    p[0] &= o[0]; p[1] &= o[1]; p[2] &= o[2]; p[3] &= o[3];
                }
            }
            else
            {
                for (; i <= count - 8; i += 8)
                {
                    uint* p = ptr + i;
                    uint* o = otherPtr + i;
                    p[0] &= o[0];
                    p[1] &= o[1];
                    p[2] &= o[2];
                    p[3] &= o[3];
                    p[4] &= o[4];
                    p[5] &= o[5];
                    p[6] &= o[6];
                    p[7] &= o[7];
                }
            }
            for (; i < count; i++)
            {
                ptr[i] &= otherPtr[i];
            }
        }
        return this;
    }

    public BitSet Difference(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        fixed (uint* ptr = _bitmap, otherPtr = other._bitmap)
        {
            int i = 0;
            int count = _wordCount;
            if (IntPtr.Size == 8)
            {
                for (; i <= count - 8; i += 8)
                {
                    ulong* p = (ulong*)(ptr + i);
                    ulong* o = (ulong*)(otherPtr + i);
                    p[0] &= ~o[0]; p[1] &= ~o[1]; p[2] &= ~o[2]; p[3] &= ~o[3];
                }
            }
            else
            {
                for (; i <= count - 8; i += 8)
                {
                    uint* p = ptr + i;
                    uint* o = otherPtr + i;
                    p[0] &= ~o[0];
                    p[1] &= ~o[1];
                    p[2] &= ~o[2];
                    p[3] &= ~o[3];
                    p[4] &= ~o[4];
                    p[5] &= ~o[5];
                    p[6] &= ~o[6];
                    p[7] &= ~o[7];
                }
            }
            for (; i < count; i++)
            {
                ptr[i] &= ~otherPtr[i];
            }
        }
        return this;
    }

    public BitSet SymmetricDifference(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch", nameof(other));
        fixed (uint* ptr = _bitmap, otherPtr = other._bitmap)
        {
            int i = 0;
            int count = _wordCount;
            if (IntPtr.Size == 8)
            {
                for (; i <= count - 8; i += 8)
                {
                    ulong* p = (ulong*)(ptr + i);
                    ulong* o = (ulong*)(otherPtr + i);
                    p[0] ^= o[0]; p[1] ^= o[1]; p[2] ^= o[2]; p[3] ^= o[3];
                }
            }
            else
            {
                for (; i <= count - 8; i += 8)
                {
                    uint* p = ptr + i;
                    uint* o = otherPtr + i;
                    p[0] ^= o[0];
                    p[1] ^= o[1];
                    p[2] ^= o[2];
                    p[3] ^= o[3];
                    p[4] ^= o[4];
                    p[5] ^= o[5];
                    p[6] ^= o[6];
                    p[7] ^= o[7];
                }
            }
            for (; i < count; i++)
            {
                ptr[i] ^= otherPtr[i];
            }
        }
        return this;
    }
    #endregion

    #region Query Methods
    // The Cardinality method counts the number of bits that are set to 1 in the bitset,
    // which is also known as the population count or Hamming weight.
    public int Cardinality()
    {
        fixed (uint* ptr = _bitmap)
        {
            int count = _wordCount;
            int total = 0;
            int i = 0;
            if (IntPtr.Size == 8)
            {
                for (; i <= count - 8; i += 8)
                {
                    ulong* p = (ulong*)(ptr + i);
                    total += PopCount(p[0]) +
                             PopCount(p[1]) +
                             PopCount(p[2]) +
                             PopCount(p[3]);
                }
            }
            else
            {
                for (; i <= count - 8; i += 8)
                {
                    uint* p = ptr + i;
                    total += PopCount(p[0]) + PopCount(p[1]) +
                         PopCount(p[2]) + PopCount(p[3]) +
                         PopCount(p[4]) + PopCount(p[5]) +
                         PopCount(p[6]) + PopCount(p[7]);
                }
            }
            for (; i < count; i++) total += PopCount(ptr[i]);
            return total;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int PopCount(ulong x)
    {
        x -= (x >> 1) & 0x5555555555555555UL;
        x = (x & 0x3333333333333333UL) + ((x >> 2) & 0x3333333333333333UL);
        x = (x + (x >> 4)) & 0x0F0F0F0F0F0F0F0FUL;
        return (int)((x * 0x0101010101010101UL) >> 56);
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
        fixed (uint* ptr = _bitmap, otherPtr = other._bitmap)
        {
            int i = 0;
            int count = _wordCount;

            for (; i <= count - 8; i += 8)
            {
                uint* p = ptr + i;
                uint* o = otherPtr + i;
                if (((p[0] & ~o[0]) |
                     (p[1] & ~o[1]) |
                     (p[2] & ~o[2]) |
                     (p[3] & ~o[3]) |
                     (p[4] & ~o[4]) |
                     (p[5] & ~o[5]) |
                     (p[6] & ~o[6]) |
                     (p[7] & ~o[7])) != 0)
                {
                    return false;
                }
            }
            for (; i < count; i++) if ((ptr[i] & ~otherPtr[i]) != 0) return false;
        }
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsSupersetOf(BitSet other) => other.IsSubsetOf(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsProperSubsetOf(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch");

        fixed (uint* ptr = _bitmap, otherPtr = other._bitmap)
        {
            int i = 0;
            int count = _wordCount;

            uint strictConflictBucket = 0;

            for (; i <= count - 8; i += 8)
            {
                uint* p = ptr + i;
                uint* o = otherPtr + i;
                uint subsetConflict = (p[0] & ~o[0]) |
                                     (p[1] & ~o[1]) |
                                     (p[2] & ~o[2]) |
                                     (p[3] & ~o[3]) |
                                     (p[4] & ~o[4]) |
                                     (p[5] & ~o[5]) |
                                     (p[6] & ~o[6]) |
                                     (p[7] & ~o[7]);

                if (subsetConflict != 0) return false;

                strictConflictBucket |= (o[0] & ~p[0]) |
                                        (o[1] & ~p[1]) |
                                        (o[2] & ~p[2]) |
                                        (o[3] & ~p[3]) |
                                        (o[4] & ~p[4]) |
                                        (o[5] & ~p[5]) |
                                        (o[6] & ~p[6]) |
                                        (o[7] & ~p[7]);
            }

            for (; i < count; i++)
            {
                if ((ptr[i] & ~otherPtr[i]) != 0) return false;
                strictConflictBucket |= (otherPtr[i] & ~ptr[i]);
            }
            return strictConflictBucket != 0;
        }
    }

    public bool IsProperSupersetOf(BitSet other) => other.IsProperSubsetOf(this);

    public bool Overlaps(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch");
        fixed (uint* ptr = _bitmap, otherPtr = other._bitmap)
        {
            int i = 0;
            int count = _wordCount;

            for (; i <= count - 8; i += 8)
            {
                uint* p = ptr + i;
                uint* o = otherPtr + i;
                if (((p[0] & o[0]) |
                     (p[1] & o[1]) |
                     (p[2] & o[2]) |
                     (p[3] & o[3]) |
                     (p[4] & o[4]) |
                     (p[5] & o[5]) |
                     (p[6] & o[6]) |
                     (p[7] & o[7])) != 0)
                {
                    return true;
                }
            }
            for (; i < count; i++) if ((ptr[i] & otherPtr[i]) != 0) return true;
        }
        return false;
    }

    public bool SetEquals(BitSet other)
    {
        if (other.BitHighLimit != BitHighLimit) throw new ArgumentException("BitHighLimit mismatch");
        fixed (uint* ptr = _bitmap, otherPtr = other._bitmap)
        {
            int i = 0;
            int count = _wordCount;

            for (; i <= count - 8; i += 8)
            {
                uint* p = ptr + i;
                uint* o = otherPtr + i;
                if (((p[0] ^ o[0]) |
                     (p[1] ^ o[1]) |
                     (p[2] ^ o[2]) |
                     (p[3] ^ o[3]) |
                     (p[4] ^ o[4]) |
                     (p[5] ^ o[5]) |
                     (p[6] ^ o[6]) |
                     (p[7] ^ o[7])) != 0)
                {
                    return false;
                }
            }
            for (; i < count; i++) if (ptr[i] != otherPtr[i]) return false;
        }
        return true;
    }

    public bool IsEmpty()
    {
        fixed (uint* ptr = _bitmap)
        {
            int i = 0;
            int count = _wordCount;

            for (; i <= count - 8; i += 8)
            {
                uint* p = ptr + i;
                if ((p[0] | p[1] | p[2] | p[3] | p[4] | p[5] | p[6] | p[7]) != 0)
                {
                    return false;
                }
            }
            for (; i < count; i++) if (ptr[i] != 0) return false;
        }
        return true;
    }
    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(BitSet? other) => this == other;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet Clone()
    {
        var clone = new BitSet(BitHighLimit);
        fixed (uint* ptr = _bitmap, clonePtr = clone._bitmap)
        {
            Unsafe.CopyBlock(clonePtr, ptr, (uint)AllocatedSize);
        }
        return clone;
    }

    #region Overrides
    public override bool Equals(object? obj) => obj is BitSet other && Equals(other);
    public override int GetHashCode()
    {
        fixed (uint* ptr = _bitmap)
        {
            int count = _wordCount;
            uint hash = 17;
            int i = 0;
            for (; i < count - 8; i += 8)
            {
                uint* p = ptr + i;
                hash = (hash * 31) + p[0];
                hash = (hash * 31) + p[1];
                hash = (hash * 31) + p[2];
                hash = (hash * 31) + p[3];
                hash = (hash * 31) + p[4];
                hash = (hash * 31) + p[5];
                hash = (hash * 31) + p[6];
                hash = (hash * 31) + p[7];
            }
            for (; i < count; i++)
            {
                hash = (hash * 31) + ptr[i];
            }
            return (int)hash;
        }
    }
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
                uint w = _bitmap[wi];
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
            sb.Append('~');
            AppendHex(sb, rangeEnd, buf);
        }
    }

    private static readonly Unchecked.String _hexDigits = "0123456789ABCDEF";
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
            buf[--i] = _hexDigits[nibble];
            value >>= 4;
        }

        // bulk append the prepared range
        sb.Append(buf + i, 8 - i);
    }

    private static readonly Unchecked.Array<int> _multiplyDeBruijnBitPosition = new int[32]{
        0, 1, 28, 2, 29, 14, 24, 3, 30, 22, 20, 15, 25, 17, 4, 8,
        31, 27, 13, 23, 21, 19, 16, 7, 26, 12, 18, 6, 11, 5, 10, 9
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountTrailingZeros(uint v)
    {
        return _multiplyDeBruijnBitPosition[((uint)((v & -v) * 0x077CB531U)) >> 27];
    }
    #endregion
}
