using System;
#if !NETFRAMEWORK
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace Cool;

// Some routines inspired by the Stanford Bit Twiddling Hacks by Sean Eron Anderson:
// http://graphics.stanford.edu/~seander/bithacks.html

public static class BitOperations
{
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
    public static uint RoundUpToPowerOf2(uint value) => System.Numerics.BitOperations.RoundUpToPowerOf2(value);

    /// <summary>
    /// Round the given integral value up to a power of 2.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The smallest power of 2 which is greater than or equal to <paramref name="value"/>.
    /// If <paramref name="value"/> is 0 or the result overflows, returns 0.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong RoundUpToPowerOf2(ulong value) => System.Numerics.BitOperations.RoundUpToPowerOf2(value);

    /// <summary>
    /// Round the given integral value up to a power of 2.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// The smallest power of 2 which is greater than or equal to <paramref name="value"/>.
    /// If <paramref name="value"/> is 0 or the result overflows, returns 0.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint RoundUpToPowerOf2(nuint value) => System.Numerics.BitOperations.RoundUpToPowerOf2(value);

    /// <summary>
    /// Count the number of leading zero bits in a mask.
    /// Similar in behavior to the x86 instruction LZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeadingZeroCount(uint value) => System.Numerics.BitOperations.LeadingZeroCount(value);

    /// <summary>
    /// Count the number of leading zero bits in a mask.
    /// Similar in behavior to the x86 instruction LZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeadingZeroCount(ulong value) => System.Numerics.BitOperations.LeadingZeroCount(value);

    /// <summary>
    /// Count the number of leading zero bits in a mask.
    /// Similar in behavior to the x86 instruction LZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeadingZeroCount(nuint value) => System.Numerics.BitOperations.LeadingZeroCount(value);

    /// <summary>
    /// Returns the integer (floor) log of the specified value, base 2.
    /// Note that by convention, input value 0 returns 0 since log(0) is undefined.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2(uint value) => System.Numerics.BitOperations.Log2(value);

    /// <summary>
    /// Returns the integer (floor) log of the specified value, base 2.
    /// Note that by convention, input value 0 returns 0 since log(0) is undefined.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2(ulong value) => System.Numerics.BitOperations.Log2(value);

    /// <summary>
    /// Returns the integer (floor) log of the specified value, base 2.
    /// Note that by convention, input value 0 returns 0 since log(0) is undefined.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Log2(nuint value) => System.Numerics.BitOperations.Log2(value);

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
    public static int PopCount(uint value) => System.Numerics.BitOperations.PopCount(value);

    /// <summary>
    /// Returns the population count (number of bits set) of a mask.
    /// Similar in behavior to the x86 instruction POPCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PopCount(ulong value) => System.Numerics.BitOperations.PopCount(value);

    /// <summary>
    /// Returns the population count (number of bits set) of a mask.
    /// Similar in behavior to the x86 instruction POPCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int PopCount(nuint value) => System.Numerics.BitOperations.PopCount(value);

    /// <summary>
    /// Count the number of trailing zero bits in an integer value.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(int value) => System.Numerics.BitOperations.TrailingZeroCount(value);

    /// <summary>
    /// Count the number of trailing zero bits in an integer value.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(uint value) => System.Numerics.BitOperations.TrailingZeroCount(value);

    /// <summary>
    /// Count the number of trailing zero bits in a mask.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(long value) => System.Numerics.BitOperations.TrailingZeroCount(value);

    /// <summary>
    /// Count the number of trailing zero bits in a mask.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(ulong value) => System.Numerics.BitOperations.TrailingZeroCount(value);

    /// <summary>
    /// Count the number of trailing zero bits in a mask.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(nint value) => System.Numerics.BitOperations.TrailingZeroCount(value);

    /// <summary>
    /// Count the number of trailing zero bits in a mask.
    /// Similar in behavior to the x86 instruction TZCNT.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int TrailingZeroCount(nuint value) => System.Numerics.BitOperations.TrailingZeroCount(value);

    /// <summary>
    /// Rotates the specified value left by the specified number of bits.
    /// Similar in behavior to the x86 instruction ROL.
    /// </summary>
    /// <param name="value">The value to rotate.</param>
    /// <param name="offset">The number of bits to rotate by.
    /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint RotateLeft(uint value, int offset) => System.Numerics.BitOperations.RotateLeft(value, offset);

    /// <summary>
    /// Rotates the specified value left by the specified number of bits.
    /// Similar in behavior to the x86 instruction ROL.
    /// </summary>
    /// <param name="value">The value to rotate.</param>
    /// <param name="offset">The number of bits to rotate by.
    /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong RotateLeft(ulong value, int offset) => System.Numerics.BitOperations.RotateLeft(value, offset);

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
    public static nuint RotateLeft(nuint value, int offset) => System.Numerics.BitOperations.RotateLeft(value, offset);

    /// <summary>
    /// Rotates the specified value right by the specified number of bits.
    /// Similar in behavior to the x86 instruction ROR.
    /// </summary>
    /// <param name="value">The value to rotate.</param>
    /// <param name="offset">The number of bits to rotate by.
    /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint RotateRight(uint value, int offset) => System.Numerics.BitOperations.RotateRight(value, offset);

    /// <summary>
    /// Rotates the specified value right by the specified number of bits.
    /// Similar in behavior to the x86 instruction ROR.
    /// </summary>
    /// <param name="value">The value to rotate.</param>
    /// <param name="offset">The number of bits to rotate by.
    /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
    /// <returns>The rotated value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong RotateRight(ulong value, int offset) => System.Numerics.BitOperations.RotateRight(value, offset);

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
    public static nuint RotateRight(nuint value, int offset) => System.Numerics.BitOperations.RotateRight(value, offset);

    /// <summary>
    /// Accumulates the CRC (Cyclic redundancy check) checksum.
    /// </summary>
    /// <param name="crc">The base value to calculate checksum on</param>
    /// <param name="data">The data for which to compute the checksum</param>
    /// <returns>The CRC-checksum</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Crc32C(uint crc, byte data)
    {
        if (Sse42.IsSupported)
        {
            return Sse42.Crc32(crc, data);
        }

        if (Crc32.IsSupported)
        {
            return Crc32.ComputeCrc32C(crc, data);
        }

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
        if (Sse42.IsSupported)
        {
            return Sse42.Crc32(crc, data);
        }

        if (Crc32.IsSupported)
        {
            return Crc32.ComputeCrc32C(crc, data);
        }

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
        if (Sse42.IsSupported)
        {
            return Sse42.Crc32(crc, data);
        }

        if (Crc32.IsSupported)
        {
            return Crc32.ComputeCrc32C(crc, data);
        }

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
        if (Sse42.X64.IsSupported)
        {
            // This intrinsic returns a 64-bit register with the upper 32-bits set to 0.
            return (uint)Sse42.X64.Crc32(crc, data);
        }

        if (Crc32.Arm64.IsSupported)
        {
            return Crc32.Arm64.ComputeCrc32C(crc, data);
        }

        return Crc32C(Crc32C(crc, (uint)(data)), (uint)(data >> 32));
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