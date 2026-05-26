using System;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class BitOperationsTests
    {
        [Fact]
        public void IsPow2_Unsigned()
        {
            Assert.True(BitOperations.IsPow2(1u));
            Assert.True(BitOperations.IsPow2(2u));
            Assert.True(BitOperations.IsPow2(1u << 31));
            Assert.False(BitOperations.IsPow2(0u));
            Assert.False(BitOperations.IsPow2(3u));
            Assert.False(BitOperations.IsPow2(6u));
        }

        [Fact]
        public void RoundUpToPowerOf2_UInt()
        {
            Assert.Equal(0u, BitOperations.RoundUpToPowerOf2(0u));
            Assert.Equal(1u, BitOperations.RoundUpToPowerOf2(1u));
            Assert.Equal(2u, BitOperations.RoundUpToPowerOf2(2u));
            Assert.Equal(4u, BitOperations.RoundUpToPowerOf2(3u));
            Assert.Equal(8u, BitOperations.RoundUpToPowerOf2(5u));
            Assert.Equal(0u, BitOperations.RoundUpToPowerOf2(uint.MaxValue));
        }

        [Fact]
        public void RoundUpToPowerOf2_ULong()
        {
            Assert.Equal(0ul, BitOperations.RoundUpToPowerOf2(0ul));
            Assert.Equal(1ul, BitOperations.RoundUpToPowerOf2(1ul));
            Assert.Equal(8ul, BitOperations.RoundUpToPowerOf2(5ul));
            Assert.Equal(0ul, BitOperations.RoundUpToPowerOf2(ulong.MaxValue));
            Assert.Equal(1ul << 63, BitOperations.RoundUpToPowerOf2(1ul << 63));
        }

        [Fact]
        public void LeadingZeroCount_And_Log2_PopCount_TrailingZeroCount()
        {
            Assert.Equal(32, BitOperations.LeadingZeroCount(0u));
            Assert.Equal(31, BitOperations.LeadingZeroCount(1u));
            Assert.Equal(0, BitOperations.LeadingZeroCount(0x80000000u));

            Assert.Equal(0, BitOperations.Log2(0u));
            Assert.Equal(0, BitOperations.Log2(1u));
            Assert.Equal(1, BitOperations.Log2(2u));
            Assert.Equal(1, BitOperations.Log2(3u));
            Assert.Equal(2, BitOperations.Log2(4u));

            Assert.Equal(0, BitOperations.PopCount(0u));
            Assert.Equal(4, BitOperations.PopCount(0xFu));
            Assert.Equal(32, BitOperations.PopCount(uint.MaxValue));

            Assert.Equal(32, BitOperations.TrailingZeroCount(0u));
            Assert.Equal(0, BitOperations.TrailingZeroCount(1u));
            Assert.Equal(1, BitOperations.TrailingZeroCount(2u));
            Assert.Equal(4, BitOperations.TrailingZeroCount(16u));
        }

        [Fact]
        public void RotateLeftRight_Wraps()
        {
            Assert.Equal(1u, BitOperations.RotateLeft(0x80000000u, 1));
            Assert.Equal(0x80000000u, BitOperations.RotateRight(1u, 1));
            // rotation by more than width is congruent mod width
            Assert.Equal(BitOperations.RotateLeft(0x12345678u, 33), BitOperations.RotateLeft(0x12345678u, 1));
        }

        [Fact]
        public void PopCount_ULong()
        {
            Assert.Equal(0, BitOperations.PopCount(0ul));
            Assert.Equal(1, BitOperations.PopCount(1ul << 63));
            Assert.Equal(64, BitOperations.PopCount(ulong.MaxValue));
        }

        [Fact]
        public void TrailingZeroCount_ULong()
        {
            Assert.Equal(0, BitOperations.TrailingZeroCount(1ul));
            Assert.Equal(32, BitOperations.TrailingZeroCount(1ul << 32));
            Assert.Equal(63, BitOperations.TrailingZeroCount(1ul << 63));
        }
    }
}
