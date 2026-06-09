#if NETFRAMEWORK
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

// Some routines inspired by the Stanford Bit Twiddling Hacks by Sean Eron Anderson:
// http://graphics.stanford.edu/~seander/bithacks.html

public static class BitOperations
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private unsafe struct TrailingZeroCountDeBruijnTable
    {
        public fixed int Table[32];

        public TrailingZeroCountDeBruijnTable()
        {
            fixed (int* p = Table)
            {
                p[0] = 00; p[1] = 01; p[2] = 28; p[3] = 02; p[4] = 29; p[5] = 14; p[6] = 24; p[7] = 03;
                p[8] = 30; p[9] = 22; p[10] = 20; p[11] = 15; p[12] = 25; p[13] = 17; p[14] = 04; p[15] = 08;
                p[16] = 31; p[17] = 27; p[18] = 13; p[19] = 23; p[20] = 21; p[21] = 19; p[22] = 16; p[23] = 07;
                p[24] = 26; p[25] = 12; p[26] = 18; p[27] = 06; p[28] = 11; p[29] = 05; p[30] = 10; p[31] = 09;
            }
        }
    }
    private static readonly TrailingZeroCountDeBruijnTable TrailingZeroCountDeBruijn = new();

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
    public static unsafe nuint RoundUpToPowerOf2(nuint value)
    {
        if (sizeof(IntPtr) == 8)
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
        return 31 ^ Log2Core(value);
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
    public static unsafe int LeadingZeroCount(nuint value)
    {
        if (sizeof(IntPtr) == 8)
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
        return Log2Core(value);
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
    public static unsafe int Log2(nuint value)
    {
        if (sizeof(IntPtr) == 8)
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
    /// Note that by convention, input value 0 returns -1 since Log(0) is undefined.
    /// Does not directly use any hardware intrinsics, nor does it incur branching.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Log2Core(uint value)
    {
        int c = -1;
        if (value >= 1 << 16) { value >>= 16; c += 16; }
        if (value >= 1 << 8) { value >>= 8; c += 8; }
        if (value >= 1 << 4) { value >>= 4; c += 4; }
        if (value >= 1 << 2) { value >>= 2; c += 2; }
        if (value >= 1 << 1) { value >>= 1; c += 1; }
        return c + (int)value;
    }

    /// <summary>Returns the integer (ceiling) log of the specified value, base 2.</summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2Ceiling(uint value)
    {
        if (value <= 1) return 0;
        return Log2(value - 1) + 1;
    }

    /// <summary>Returns the integer (ceiling) log of the specified value, base 2.</summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2Ceiling(ulong value)
    {
        if (value <= 1) return 0;
        return Log2(value - 1) + 1;
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
    public static unsafe int PopCount(ulong value)
    {
        if (sizeof(IntPtr) == 4)
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
    public static unsafe int PopCount(nuint value)
    {
        if (sizeof(IntPtr) == 8)
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
    public static unsafe int TrailingZeroCount(uint value)
    {
        // Unguarded fallback contract is 0->0, BSF contract is 0->undefined
        if (value == 0)
        {
            return 32;
        }

        // uint.MaxValue >> 27 is always in range [0 - 31]
        return TrailingZeroCountDeBruijn.Table[(uint)((value & -value) * 0x077CB531u) >> 27];
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
    public static unsafe int TrailingZeroCount(nint value)
    {
        if (sizeof(IntPtr) == 8)
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
    public static unsafe int TrailingZeroCount(nuint value)
    {
        if (sizeof(IntPtr) == 8)
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
    public static unsafe nuint RotateLeft(nuint value, int offset)
    {
        if (sizeof(IntPtr) == 8)
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
    public static unsafe nuint RotateRight(nuint value, int offset)
    {
        if (sizeof(IntPtr) == 8)
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
        private static readonly Unchecked.SZArray<uint> s_crcTable = Crc32ReflectedTable.Generate(0x82F63B78u);

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
#endif