
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Cool;

public interface IBitSet
{
    nuint WordCount { get; }
    ref nuint Bitmap { get; }
}

public class BitSet<TAlloc> : IBitSet, IDisposable where TAlloc : struct, BitSet.IAllocator
{
    #region Fields, Properties, Constructor and Dispose
    private TAlloc _allocator;

    public nuint BitHighLimit
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _allocator.BitHighLimit;
    }

    private nuint WordCount
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => BitSet.WordCount(BitHighLimit);
    }

    nuint IBitSet.WordCount => WordCount;

    private nuint TailMask
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => BitSet.TailMask(BitHighLimit);
    }

    private ref nuint Bitmap
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref _allocator.GetReference();
    }

    ref nuint IBitSet.Bitmap => ref Bitmap;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BitSet() { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet(nuint bitHighLimit) : this()
    {
        _allocator.Init(bitHighLimit);
        Clear();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet(nuint bitHighLimit, string range) : this(bitHighLimit) => Set(range);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<UAlloc> Clone<UAlloc>() where UAlloc : struct, BitSet.IAllocator
    {
        BitSet<UAlloc> to = new();
        Unsafe.SkipInit(out _allocator);
        to._allocator.Init(BitHighLimit);
        ref byte dest = ref Unsafe.As<nuint, byte>(ref to.Bitmap);
        ref byte src = ref Unsafe.As<nuint, byte>(ref Bitmap);
        Unsafe.CopyBlock(ref dest, ref src, BitSet.ByteCount(to.BitHighLimit));
        return to;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> Clone() => Clone<TAlloc>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (_allocator is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
    #endregion

    #region Operator Overloads
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void operator |=(IBitSet right) => Union(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void operator &=(IBitSet right) => Intersect(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void operator ^=(IBitSet right) => SymmetricDifference(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void operator -=(IBitSet right) => Difference(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet<TAlloc> operator |(BitSet<TAlloc> left, IBitSet right) => left.Clone().Union(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet<TAlloc> operator &(BitSet<TAlloc> left, IBitSet right) => left.Clone().Intersect(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet<TAlloc> operator ^(BitSet<TAlloc> left, IBitSet right) => left.Clone().SymmetricDifference(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet<TAlloc> operator -(BitSet<TAlloc> left, IBitSet right) => left.Clone().Difference(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BitSet<TAlloc> operator ~(BitSet<TAlloc> set) => set.Invert();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(BitSet<TAlloc> left, IBitSet right) => left.IsProperSubsetOf(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(BitSet<TAlloc> left, IBitSet right) => left.IsSubsetOf(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(BitSet<TAlloc> left, IBitSet right) => left.IsProperSupersetOf(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(BitSet<TAlloc> left, IBitSet right) => left.IsSupersetOf(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(BitSet<TAlloc> left, IBitSet right)
    {
        if (left is null) return right is null;
        if (right is null) return false;
        return left.SetEquals(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(BitSet<TAlloc> left, IBitSet right) => !(left == right);
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
    private ref nuint Word(nuint pos) => ref Unsafe.Add(ref Bitmap, BitSet.WordIndex(pos));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(nuint pos) { if (pos <= BitHighLimit) { Word(pos) |= BitSet.BitIndex(pos); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(nuint pos) => (pos <= BitHighLimit) && (Word(pos) & BitSet.BitIndex(pos)) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear(nuint pos) { if (pos <= BitHighLimit) { Word(pos) &= ~BitSet.BitIndex(pos); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Invert(nuint pos) { if (pos <= BitHighLimit) { Word(pos) ^= BitSet.BitIndex(pos); } }
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> Set(string range)
    {
        if (string.IsNullOrWhiteSpace(range)) return this;
        foreach (nuint pos in new Range<nuint>(range, BitHighLimit)) Set(pos);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> Clear(string range)
    {
        if (string.IsNullOrWhiteSpace(range)) return this;
        foreach (nuint pos in new Range<nuint>(range, BitHighLimit)) Clear(pos);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> Invert(string range)
    {
        if (string.IsNullOrWhiteSpace(range)) return this;
        foreach (nuint pos in new Range<nuint>(range, BitHighLimit)) Invert(pos);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void MaskTail() => Unsafe.Add(ref Bitmap, WordCount - 1) &= TailMask;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> SetAll()
    {
        nuint wordCount = WordCount;
        Unchecked.Fill(ref Bitmap, wordCount, nuint.MaxValue);
        // Mask off unused bits in the last word to ensure they remain 0
        MaskTail();
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> Clear()
    {
        Unsafe.InitBlockUnaligned(ref Unsafe.As<nuint, byte>(ref Bitmap), 0, BitSet.ByteCount(BitHighLimit));
        return this;
    }
    #endregion

    #region Set Operations
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> Invert()
    {
        nuint wordCount = WordCount;
        Unchecked.Not(ref Bitmap, wordCount);
        // Mask off unused bits in the last word to ensure they remain 0
        MaskTail();
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BitSet<TAlloc> Intersect(IBitSet other)
    {
        nuint leftWordCount = WordCount;
        nuint rightWordCount = other.WordCount;
        nuint wordCount = BitSet.Min(leftWordCount, rightWordCount);
        Unchecked.And(ref Bitmap, ref other.Bitmap, wordCount);
        if (leftWordCount > rightWordCount)
        {
            // Clear any remaining words beyond the that set's limit
            Unchecked.Fill(ref Unsafe.Add(ref Bitmap, wordCount), leftWordCount - rightWordCount, 0u);
        }
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> Intersect<UAlloc>(BitSet<UAlloc> other) where UAlloc : struct, BitSet.IAllocator => Intersect((IBitSet)other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BitSet<TAlloc> Union(IBitSet other)
    {
        nuint wordCount = BitSet.Min(WordCount, other.WordCount);
        Unchecked.Or(ref Bitmap, ref other.Bitmap, wordCount);
        // Mask off unused bits in the last word to ensure they remain 0
        MaskTail();
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> Union<UAlloc>(BitSet<UAlloc> other) where UAlloc : struct, BitSet.IAllocator => Union((IBitSet)other);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> SymmetricDifference(IBitSet other)
    {
        nuint wordCount = BitSet.Min(WordCount, other.WordCount);
        Unchecked.Xor(ref Bitmap, ref other.Bitmap, wordCount);
        // Mask off unused bits in the last word to ensure they remain 0
        MaskTail();
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> SymmetricDifference<UAlloc>(BitSet<UAlloc> other) where UAlloc : struct, BitSet.IAllocator => SymmetricDifference((IBitSet)other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BitSet<TAlloc> Difference(IBitSet other)
    {
        nuint wordCount = BitSet.Min(WordCount, other.WordCount);
        Unchecked.AndNot(ref Bitmap, ref other.Bitmap, wordCount);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BitSet<TAlloc> Difference<UAlloc>(BitSet<UAlloc> other) where UAlloc : struct, BitSet.IAllocator => Difference((IBitSet)other);
    #endregion

    #region Query Methods
    // The Cardinality method counts the number of bits that are set to 1 in the bitset,
    // which is also known as the population count or Hamming weight.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public nuint Cardinality() => Unchecked.PopCount(ref Bitmap, WordCount);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEmpty() => Unchecked.IsZero(ref Bitmap, WordCount);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool SetEquals(IBitSet other)
    {
        nuint leftWordCount = WordCount;
        nuint rightWordCount = other.WordCount;
        nuint wordCount = BitSet.Min(leftWordCount, rightWordCount);
        if (!Unchecked.Equals(ref Bitmap, ref other.Bitmap, wordCount)) return false;
        if (leftWordCount == rightWordCount) return true;
        if (leftWordCount > rightWordCount) return Unchecked.IsZero(ref Unsafe.Add(ref Bitmap, wordCount), leftWordCount - rightWordCount);
        return Unchecked.IsZero(ref Unsafe.Add(ref other.Bitmap, wordCount), rightWordCount - leftWordCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool SetEquals<UAlloc>(BitSet<UAlloc> other) where UAlloc : struct, BitSet.IAllocator => SetEquals((IBitSet)other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsSubsetOf(IBitSet other)
    {
        nuint leftWordCount = WordCount;
        nuint rightWordCount = other.WordCount;
        nuint wordCount = BitSet.Min(leftWordCount, rightWordCount);
        if (!Unchecked.IsSubset(ref Bitmap, ref other.Bitmap, wordCount)) return false;
        if (leftWordCount <= rightWordCount) return true;
        return Unchecked.IsZero(ref Unsafe.Add(ref Bitmap, wordCount), leftWordCount - rightWordCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsSubsetOf<UAlloc>(BitSet<UAlloc> other) where UAlloc : struct, BitSet.IAllocator => IsSubsetOf((IBitSet)other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsProperSubsetOf(IBitSet other)
    {
        return IsSubsetOf(other) && !SetEquals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsProperSubsetOf<UAlloc>(BitSet<UAlloc> other) where UAlloc : struct, BitSet.IAllocator => IsProperSubsetOf((IBitSet)other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsSupersetOf(IBitSet other)
    {
        nuint leftWordCount = WordCount;
        nuint rightWordCount = other.WordCount;
        nuint wordCount = BitSet.Min(leftWordCount, rightWordCount);
        if (!Unchecked.IsSubset(ref other.Bitmap, ref Bitmap, wordCount)) return false;
        if (leftWordCount >= rightWordCount) return true;
        return Unchecked.IsZero(ref Unsafe.Add(ref other.Bitmap, wordCount), rightWordCount - leftWordCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsSupersetOf<UAlloc>(BitSet<UAlloc> other) where UAlloc : struct, BitSet.IAllocator => IsSupersetOf((IBitSet)other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsProperSupersetOf(IBitSet other)
    {
        return IsSupersetOf(other) && !SetEquals(other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsProperSupersetOf<UAlloc>(BitSet<UAlloc> other) where UAlloc : struct, BitSet.IAllocator => IsProperSupersetOf((IBitSet)other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Overlaps(IBitSet other)
    {
        nuint wordCount = BitSet.Min(WordCount, other.WordCount);
        return Unchecked.Overlaps(ref Bitmap, ref other.Bitmap, wordCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Overlaps<UAlloc>(BitSet<UAlloc> other) where UAlloc : struct, BitSet.IAllocator => Overlaps((IBitSet)other);

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(BitSet<TAlloc> other) => this == other;

    #region Overrides
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (obj is BitSet<TAlloc> other)
        {
            return this == other;
        }
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return Unchecked.GetHashCode(ref Bitmap, WordCount);
    }

    private const int bufferLength = 20;
    public override unsafe string ToString()
    {
        // Build ranges in the same hex format used by the constructor: "START~END,POS,..."

        var sb = StringBuilderPool.Shared.Rent();
        try
        {
            bool first = true;
            bool inRange = false;
            nuint rangeStart = 0, rangeEnd = 0;
            char* buf = stackalloc char[bufferLength];
            nuint wordCount = WordCount;

            for (nuint wi = 0; wi < wordCount; wi++)
            {
                nuint w = Unsafe.Add(ref Bitmap, wi);
                while (w != 0u)
                {
                    int tz = BitOperations.TrailingZeroCount(w);
                    nuint pos = (wi << BitSet.ShiftCount) + (nuint)tz;

                    if (!inRange)
                    {
                        inRange = true;
                        rangeStart = pos;
                    }
                    else if (pos != rangeEnd + 1)
                    {
                        if (!first) { sb.Append(','); } else { first = false; }
                        AppendRange(sb, rangeStart, rangeEnd, buf);

                        rangeStart = pos;
                    }
                    if (tz == 0 && (w + 1u) == 0u)
                    {
                        // [Full positioning express path]: One step to achieve the goal, directly add the entire Word length to rangeEnd
                        rangeEnd = pos + (1u << BitSet.ShiftCount) - 1u;
                        w = 0;
                    }
                    else if (((w >> (tz + 1)) & 1u) == 0)
                    {
                        //[Sparse fast path]: isolated 1, direct single point stepping
                        rangeEnd = pos;
                        w &= w - 1u;
                    }
                    else
                    {
                        //[Dense fast path]: There are continuous 1s, start high-energy bit mask crossing
                        nuint remaining = w >> tz;
                        int continuousOnes = BitOperations.TrailingZeroCount(~remaining);

                        rangeEnd = pos + (nuint)continuousOnes - 1;

                        if (continuousOnes < (1 << BitSet.ShiftCount))
                        {
                            nuint mask = (((nuint)1 << continuousOnes) - 1u) << tz;
                            w &= ~mask;
                        }
                        else
                        {
                            w = 0;
                        }
                    }
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
    private static unsafe void AppendRange(StringBuilder sb, nuint rangeStart, nuint rangeEnd, char* buf)
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

    private static readonly NativeMemoryManager HexTableManager = InitHexTableManager();
    private static readonly unsafe uint* HexTable = (uint*)HexTableManager.GetPointer();

    private static unsafe NativeMemoryManager InitHexTableManager()
    {
        var table = new NativeMemoryManager(256 * sizeof(uint));
        var ptr = (uint*)table.GetPointer();
        string hex = "0123456789ABCDEF";
        bool isLittleEndian = BitConverter.IsLittleEndian;
        for (int i = 0; i < 256; i++)
        {
            uint c1 = hex[i >> 4];
            uint c2 = hex[i & 0x0F];
            ptr[i] = isLittleEndian ? (c1 | (c2 << 16)) : ((c1 << 16) | c2);
        }
        return table;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void AppendHex(StringBuilder sb, nuint value, char* buf)
    {
        if (value == 0u)
        {
            sb.Append('0');
            return;
        }
        int i = bufferLength;
        while (value >= 0x10u)
        {
            *(uint*)(buf + (i -= 2)) = HexTable[value & 0xFFu];
            value >>= 8;
        }
        if (value > 0)
        {
            Unchecked.String hexDigits = "0123456789ABCDEF";
            buf[--i] = hexDigits[(int)value];
        }
        sb.Append(buf + i, bufferLength - i);
    }
    #endregion
}