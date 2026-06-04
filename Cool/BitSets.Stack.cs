using System.Runtime.CompilerServices;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;

namespace Cool;

public static partial class BitSets
{
    public ref struct Stack
    {
        #region Fields and Constructor
        private Unchecked.Ptr<uint> _bitmap;
        public readonly uint BitHighLimit;
        public readonly int AllocatedSize;
        private readonly int _wordCount;
        private readonly uint _tailMask;
        public unsafe Stack(uint bitHighLimit)
        {
            BitHighLimit = bitHighLimit;
            _wordCount = (int)((bitHighLimit >> 5) + 1);
            AllocatedSize = _wordCount * sizeof(uint);
            int remainingBits = (int)(bitHighLimit & 31);
            _tailMask = (remainingBits == 31) ? uint.MaxValue : (1u << (remainingBits + 1)) - 1u;
            uint* pbitmap = stackalloc uint[_wordCount];
            _bitmap = pbitmap;
        }
        #endregion

        // #region Operator Overloads
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public void operator |=(Stack other) => Union(other);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public void operator &=(Stack other) => Intersect(other);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public void operator ^=(Stack other) => SymmetricDifference(other);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public void operator -=(Stack other) => Difference(other);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Stack operator |(Stack left, Stack right) => left.Clone().Union(right);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Stack operator &(Stack left, Stack right) => left.Clone().Intersect(right);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Stack operator ^(Stack left, Stack right) => left.Clone().SymmetricDifference(right);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Stack operator -(Stack left, Stack right) => left.Clone().Difference(right);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static Stack operator ~(Stack set) => set.Invert();

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static bool operator <(Stack left, Stack right) => left.IsProperSubsetOf(right);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static bool operator <=(Stack left, Stack right) => left.IsSubsetOf(right);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static bool operator >(Stack left, Stack right) => left.IsProperSupersetOf(right);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static bool operator >=(Stack left, Stack right) => left.IsSupersetOf(right);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static bool operator ==(Stack left, Stack right) => left.SetEquals(right);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static bool operator !=(Stack? left, Stack? right) => !(left == right);
        // #endregion

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
        [UnscopedRef]
        public ref Stack Set(string range)
        {
            if (string.IsNullOrWhiteSpace(range)) return ref this;
            foreach (uint pos in new Range<uint>(range, BitHighLimit)) Set(pos);
            return ref this;
        }

        [UnscopedRef]
        public ref Stack Clear(string range)
        {
            if (string.IsNullOrWhiteSpace(range)) return ref this;
            foreach (uint pos in new Range<uint>(range, BitHighLimit)) Clear(pos);
            return ref this;
        }

        [UnscopedRef]
        public ref Stack Invert(string range)
        {
            if (string.IsNullOrWhiteSpace(range)) return ref this;
            foreach (uint pos in new Range<uint>(range, BitHighLimit)) Invert(pos);
            return ref this;
        }

        [UnscopedRef]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Stack SetAll()
        {
            Unchecked.Fill(ref _bitmap.GetReference(), (nuint)_wordCount, uint.MaxValue);
            // Mask off unused bits in the last word to ensure they remain 0
            _bitmap[_wordCount - 1] &= _tailMask;
            return ref this;
        }

        [UnscopedRef]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Stack Clear()
        {
            Unchecked.Fill(ref _bitmap.GetReference(), (nuint)_wordCount, 0u);
            return ref this;
        }

        [UnscopedRef]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Stack Invert()
        {
            Unchecked.Not(ref _bitmap.GetReference(), (nuint)_wordCount);
            // Mask off unused bits in the last word to ensure they remain 0
            _bitmap[_wordCount - 1] &= _tailMask;
            return ref this;
        }
        #endregion
    }
}