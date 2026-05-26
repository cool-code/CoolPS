#if NETFRAMEWORK
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Runtime.CompilerServices;

namespace Cool;

// Some routines inspired by the Stanford Bit Twiddling Hacks by Sean Eron Anderson:
// http://graphics.stanford.edu/~seander/bithacks.html

public static class BitOperations
{
    private static readonly Unchecked.Array<byte> TrailingZeroCountDeBruijn = new byte[]{
            00, 01, 28, 02, 29, 14, 24, 03,
            30, 22, 20, 15, 25, 17, 04, 08,
            31, 27, 13, 23, 21, 19, 16, 07,
            26, 12, 18, 06, 11, 05, 10, 09
        };

    private static readonly Unchecked.Array<byte> Log2DeBruijn = new byte[]{
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

    /// <summary>
    /// Evaluate whether a given integral value is a power of 2.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(int value) => (value & (value - 1)) == 0 && value > 0;

    /// <summary>
    /// Evaluate whether a given integral value is a power of 2.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(uint value) => (value & (value - 1)) == 0 && value != 0;

    /// <summary>
    /// Evaluate whether a given integral value is a power of 2.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(long value) => (value & (value - 1)) == 0 && value > 0;

    /// <summary>
    /// Evaluate whether a given integral value is a power of 2.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(ulong value) => (value & (value - 1)) == 0 && value != 0;

    /// <summary>
    /// Evaluate whether a given integral value is a power of 2.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(nint value) => (value & (value - 1)) == 0 && value > 0;

    /// <summary>
    /// Evaluate whether a given integral value is a power of 2.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(nuint value) => (value & (value - 1)) == 0 && value != 0;

    /// <summary>Round the given integral value up to a power of 2.</summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The smallest power of 2 which is greater than or equal to <paramref name="value"/>.
    /// If <paramref name="value"/> is 0 or the result overflows, returns 0.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint RoundUpToPowerOf2(uint value)
    {
        // Based on https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
        --value;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        return value + 1;
    }

    /// <summary>
    /// Round the given integral value up to a power of 2.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The smallest power of 2 which is greater than or equal to <paramref name="value"/>.
    /// If <paramref name="value"/> is 0 or the result overflows, returns 0.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong RoundUpToPowerOf2(ulong value)
    {
        // Based on https://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
        --value;
        value |= value >> 1;
        value |= value >> 2;
        value |= value >> 4;
        value |= value >> 8;
        value |= value >> 16;
        value |= value >> 32;
        return value + 1;
    }

    /// <summary>
    /// Round the given integral value up to a power of 2.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The smallest power of 2 which is greater than or equal to <paramref name="value"/>.
    /// If <paramref name="value"/> is 0 or the result overflows, returns 0.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint RoundUpToPowerOf2(nuint value)
    {
        if (UIntPtr.Size == 8)
        {
            return (nuint)RoundUpToPowerOf2((ulong)value);
        }
        else
        {
            return RoundUpToPowerOf2((uint)value);
        }
    }

    /// <summary>
    /// Count the number of leading zero bits in a mask.
    /// Similar in behavior to the x86 instruction LZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeadingZeroCount(uint value)
    {
        // Unguarded fallback contract is 0->31, BSR contract is 0->undefined
        if (value == 0)
        {
            return 32;
        }
        return 31 ^ Log2SoftwareFallback(value);
    }

    /// <summary>
    /// Count the number of leading zero bits in a mask.
    /// Similar in behavior to the x86 instruction LZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeadingZeroCount(ulong value)
    {
        uint hi = (uint)(value >> 32);

        if (hi == 0)
        {
            return 32 + LeadingZeroCount((uint)value);
        }

        return LeadingZeroCount(hi);
    }

    /// <summary>
    /// Count the number of leading zero bits in a mask.
    /// Similar in behavior to the x86 instruction LZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeadingZeroCount(nuint value)
    {
        if (UIntPtr.Size == 8)
        {
            return LeadingZeroCount((ulong)value);
        }
        else
        {
            return LeadingZeroCount((uint)value);
        }
    }

    /// <summary>
    /// Returns the integer (floor) log of the specified value, base 2.
    /// Note that by convention, input value 0 returns 0 since log(0) is undefined.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2(uint value)
    {
        // The 0->0 contract is fulfilled by setting the LSB to 1.
        // Log(1) is 0, and setting the LSB for values > 1 does not change the log2 result.
        value |= 1;

        // Fallback contract is 0->0
        return Log2SoftwareFallback(value);
    }

    /// <summary>
    /// Returns the integer (floor) log of the specified value, base 2.
    /// Note that by convention, input value 0 returns 0 since log(0) is undefined.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2(ulong value)
    {
        value |= 1;

        uint hi = (uint)(value >> 32);

        if (hi == 0)
        {
            return Log2((uint)value);
        }

        return 32 + Log2(hi);
    }

    /// <summary>
    /// Returns the integer (floor) log of the specified value, base 2.
    /// Note that by convention, input value 0 returns 0 since log(0) is undefined.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2(nuint value)
    {
        if (UIntPtr.Size == 8)
        {
            return Log2((ulong)value);
        }
        else
        {
            return Log2((uint)value);
        }
    }

    /// <summary>
    /// Returns the integer (floor) log of the specified value, base 2.
    /// Note that by convention, input value 0 returns 0 since Log(0) is undefined.
    /// Does not directly use any hardware intrinsics, nor does it incur branching.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Log2SoftwareFallback(uint value)
    {
        // No AggressiveInlining due to large method size
        // Has conventional contract 0->0 (Log(0) is undefined)

        // Fill trailing zeros with ones, eg 00010010 becomes 00011111
        value |= value >> 01;
        value |= value >> 02;
        value |= value >> 04;
        value |= value >> 08;
        value |= value >> 16;

        // Using deBruijn sequence, k=2, n=5 (2^5=32) : 0b_0000_0111_1100_0100_1010_1100_1101_1101u
        return Log2DeBruijn[(int)((value * 0x07C4ACDDu) >> 27)];
    }

    /// <summary>Returns the integer (ceiling) log of the specified value, base 2.</summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int Log2Ceiling(uint value)
    {
        int result = Log2(value);
        if (PopCount(value) != 1)
        {
            result++;
        }
        return result;
    }

    /// <summary>Returns the integer (ceiling) log of the specified value, base 2.</summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int Log2Ceiling(ulong value)
    {
        int result = Log2(value);
        if (PopCount(value) != 1)
        {
            result++;
        }
        return result;
    }

    /// <summary>
    /// Returns the population count (number of bits set) of a mask.
    /// Similar in behavior to the x86 instruction POPCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PopCount(uint value)
    {
        const uint c1 = 0x_55555555u;
        const uint c2 = 0x_33333333u;
        const uint c3 = 0x_0F0F0F0Fu;
        const uint c4 = 0x_01010101u;

        value -= (value >> 1) & c1;
        value = (value & c2) + ((value >> 2) & c2);
        value = (((value + (value >> 4)) & c3) * c4) >> 24;

        return (int)value;
    }

    /// <summary>
    /// Returns the population count (number of bits set) of a mask.
    /// Similar in behavior to the x86 instruction POPCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PopCount(ulong value)
    {
        if (UIntPtr.Size == 4)
        {
            return PopCount((uint)value) // lo
                + PopCount((uint)(value >> 32)); // hi
        }
        const ulong c1 = 0x_55555555_55555555ul;
        const ulong c2 = 0x_33333333_33333333ul;
        const ulong c3 = 0x_0F0F0F0F_0F0F0F0Ful;
        const ulong c4 = 0x_01010101_01010101ul;

        value -= (value >> 1) & c1;
        value = (value & c2) + ((value >> 2) & c2);
        value = (((value + (value >> 4)) & c3) * c4) >> 56;

        return (int)value;
    }

    /// <summary>
    /// Returns the population count (number of bits set) of a mask.
    /// Similar in behavior to the x86 instruction POPCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PopCount(nuint value)
    {
        if (UIntPtr.Size == 8)
        {
            return PopCount((ulong)value);
        }
        else
        {
            return PopCount((uint)value);
        }
    }

    /// <summary>
    /// Count the number of trailing zero bits in an integer value.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(int value)
        => TrailingZeroCount((uint)value);

    /// <summary>
    /// Count the number of trailing zero bits in an integer value.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(uint value)
    {
        // Unguarded fallback contract is 0->0, BSF contract is 0->undefined
        if (value == 0)
        {
            return 32;
        }

        // uint.MaxValue >> 27 is always in range [0 - 31]
        return TrailingZeroCountDeBruijn[(int)(((value & (uint)-(int)value) * 0x077CB531u) >> 27)];
    }

    /// <summary>
    /// Count the number of trailing zero bits in a mask.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(long value)
        => TrailingZeroCount((ulong)value);

    /// <summary>
    /// Count the number of trailing zero bits in a mask.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(ulong value)
    {
        uint lo = (uint)value;

        if (lo == 0)
        {
            return 32 + TrailingZeroCount((uint)(value >> 32));
        }

        return TrailingZeroCount(lo);
    }

    /// <summary>
    /// Count the number of trailing zero bits in a mask.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(nint value)
    {
        if (UIntPtr.Size == 8)
        {
            return TrailingZeroCount((ulong)(nuint)value);
        }
        else
        {
            return TrailingZeroCount((uint)(nuint)value);
        }
    }

    /// <summary>
    /// Count the number of trailing zero bits in a mask.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(nuint value)
    {
        if (UIntPtr.Size == 8)
        {
            return TrailingZeroCount((ulong)value);
        }
        else
        {
            return TrailingZeroCount((uint)value);
        }
    }

    /// <summary>
    /// Rotates the specified value left by the specified number of bits.
    /// Similar in behavior to the x86 instruction ROL.
    /// </summary>
    /// <param name="value">The value to rotate.</param>
    /// <param name="offset">The number of bits to rotate by.
    /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint RotateLeft(uint value, int offset)
        => (value << offset) | (value >> (32 - offset));

    /// <summary>
    /// Rotates the specified value left by the specified number of bits.
    /// Similar in behavior to the x86 instruction ROL.
    /// </summary>
    /// <param name="value">The value to rotate.</param>
    /// <param name="offset">The number of bits to rotate by.
    /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong RotateLeft(ulong value, int offset)
        => (value << offset) | (value >> (64 - offset));

    /// <summary>
    /// Rotates the specified value left by the specified number of bits.
    /// Similar in behavior to the x86 instruction ROL.
    /// </summary>
    /// <param name="value">The value to rotate.</param>
    /// <param name="offset">The number of bits to rotate by.
    /// Any value outside the range [0..31] is treated as congruent mod 32 on a 32-bit process,
    /// and any value outside the range [0..63] is treated as congruent mod 64 on a 64-bit process.</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint RotateLeft(nuint value, int offset)
    {
        if (UIntPtr.Size == 8)
        {
            return (nuint)RotateLeft((ulong)value, offset);
        }
        else
        {
            return RotateLeft((uint)value, offset);
        }
    }

    /// <summary>
    /// Rotates the specified value right by the specified number of bits.
    /// Similar in behavior to the x86 instruction ROR.
    /// </summary>
    /// <param name="value">The value to rotate.</param>
    /// <param name="offset">The number of bits to rotate by.
    /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint RotateRight(uint value, int offset)
        => (value >> offset) | (value << (32 - offset));

    /// <summary>
    /// Rotates the specified value right by the specified number of bits.
    /// Similar in behavior to the x86 instruction ROR.
    /// </summary>
    /// <param name="value">The value to rotate.</param>
    /// <param name="offset">The number of bits to rotate by.
    /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong RotateRight(ulong value, int offset)
        => (value >> offset) | (value << (64 - offset));

    /// <summary>
    /// Rotates the specified value right by the specified number of bits.
    /// Similar in behavior to the x86 instruction ROR.
    /// </summary>
    /// <param name="value">The value to rotate.</param>
    /// <param name="offset">The number of bits to rotate by.
    /// Any value outside the range [0..31] is treated as congruent mod 32 on a 32-bit process,
    /// and any value outside the range [0..63] is treated as congruent mod 64 on a 64-bit process.</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint RotateRight(nuint value, int offset)
    {
        if (UIntPtr.Size == 8)
        {
            return (nuint)RotateRight((ulong)value, offset);
        }
        else
        {
            return RotateRight((uint)value, offset);
        }
    }

    /// <summary>
    /// Accumulates the CRC (Cyclic redundancy check) checksum.
    /// </summary>
    /// <param name="crc">The base value to calculate checksum on</param>
    /// <param name="data">The data for which to compute the checksum</param>
    /// <returns>The CRC-checksum</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Crc32C(uint crc, byte data)
    {
        return Crc32Fallback.Crc32C(crc, data);
    }

    /// <summary>
    /// Accumulates the CRC (Cyclic redundancy check) checksum.
    /// </summary>
    /// <param name="crc">The base value to calculate checksum on</param>
    /// <param name="data">The data for which to compute the checksum</param>
    /// <returns>The CRC-checksum</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Crc32C(uint crc, ushort data)
    {
        return Crc32Fallback.Crc32C(crc, data);
    }

    /// <summary>
    /// Accumulates the CRC (Cyclic redundancy check) checksum.
    /// </summary>
    /// <param name="crc">The base value to calculate checksum on</param>
    /// <param name="data">The data for which to compute the checksum</param>
    /// <returns>The CRC-checksum</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Crc32C(uint crc, uint data)
    {
        return Crc32Fallback.Crc32C(crc, data);
    }

    /// <summary>
    /// Accumulates the CRC (Cyclic redundancy check) checksum.
    /// </summary>
    /// <param name="crc">The base value to calculate checksum on</param>
    /// <param name="data">The data for which to compute the checksum</param>
    /// <returns>The CRC-checksum</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Crc32C(uint crc, ulong data)
    {
        return Crc32C(Crc32C(crc, (uint)data), (uint)(data >> 32));
    }

    internal static class Crc32ReflectedTable
    {
        internal static uint[] Generate(uint reflectedPolynomial)
        {
            uint[] table = new uint[256];

            for (int i = 0; i < 256; i++)
            {
                uint val = (uint)i;

                for (int j = 0; j < 8; j++)
                {
                    if ((val & 0b0000_0001) == 0)
                    {
                        val >>= 1;
                    }
                    else
                    {
                        val = (val >> 1) ^ reflectedPolynomial;
                    }
                }

                table[i] = val;
            }

            return table;
        }
    }

    private static class Crc32Fallback
    {
        // CRC-32 transition table.
        // While this implementation is based on the Castagnoli CRC-32 polynomial (CRC-32C),
        // x32 + x28 + x27 + x26 + x25 + x23 + x22 + x20 + x19 + x18 + x14 + x13 + x11 + x10 + x9 + x8 + x6 + x0,
        // this version uses reflected bit ordering, so 0x1EDC6F41 becomes 0x82F63B78u.
        // This is computed lazily so as to avoid increasing the assembly size for data that's
        // only needed on a fallback path.
        private static readonly Unchecked.Array<uint> s_crcTable = Crc32ReflectedTable.Generate(0x82F63B78u);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint Crc32C(uint crc, byte data)
        {
            return s_crcTable[(byte)(crc ^ data) ^ (crc >> 8)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint Crc32C(uint crc, ushort data)
        {
            crc = s_crcTable[(byte)(crc ^ (byte)data) ^ (crc >> 8)];
            data >>= 8;
            return s_crcTable[(byte)(crc ^ data) ^ (crc >> 8)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint Crc32C(uint crc, uint data)
        {
            crc = s_crcTable[(byte)(crc ^ (byte)data) ^ (crc >> 8)];
            data >>= 8;
            crc = s_crcTable[(byte)(crc ^ (byte)data) ^ (crc >> 8)];
            data >>= 8;
            crc = s_crcTable[(byte)(crc ^ (byte)data) ^ (crc >> 8)];
            data >>= 8;
            return s_crcTable[(byte)(crc ^ data) ^ (crc >> 8)];
        }

    }

    /// <summary>
    /// Reset the lowest significant bit in the given value
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint ResetLowestSetBit(uint value)
    {
        // It's lowered to BLSR on x86
        return value & (value - 1);
    }

    /// <summary>
    /// Reset specific bit in the given value
    /// Reset the lowest significant bit in the given value
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong ResetLowestSetBit(ulong value)
    {
        // It's lowered to BLSR on x86
        return value & (value - 1);
    }

    /// <summary>
    /// Flip the bit at a specific position in a given value.
    /// Similar in behavior to the x86 instruction BTC (Bit Test and Complement).
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="index">The zero-based index of the bit to flip.
    /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
    /// <returns>The new value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint FlipBit(uint value, int index)
    {
        return value ^ (1u << index);
    }

    /// <summary>
    /// Flip the bit at a specific position in a given value.
    /// Similar in behavior to the x86 instruction BTC (Bit Test and Complement).
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="index">The zero-based index of the bit to flip.
    /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
    /// <returns>The new value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong FlipBit(ulong value, int index)
    {
        return value ^ (1ul << index);
    }
}
#else
global using BitOperations = System.Numerics.BitOperations;
#endif