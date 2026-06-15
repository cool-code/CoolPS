using System.Runtime.CompilerServices;

namespace Cool;

public ref partial struct ValueStringBuilder
{
    private static readonly Unchecked.String HexDigits = "0123456789ABCDEF";
    private static readonly Unchecked.String HexTable =
     "000102030405060708090A0B0C0D0E0F" +
     "101112131415161718191A1B1C1D1E1F" +
     "202122232425262728292A2B2C2D2E2F" +
     "303132333435363738393A3B3C3D3E3F" +
     "404142434445464748494A4B4C4D4E4F" +
     "505152535455565758595A5B5C5D5E5F" +
     "606162636465666768696A6B6C6D6E6F" +
     "707172737475767778797A7B7C7D7E7F" +
     "808182838485868788898A8B8C8D8E8F" +
     "909192939495969798999A9B9C9D9E9F" +
     "A0A1A2A3A4A5A6A7A8A9AAABACADAEAF" +
     "B0B1B2B3B4B5B6B7B8B9BABBBCBDBEBF" +
     "C0C1C2C3C4C5C6C7C8C9CACBCCCDCECF" +
     "D0D1D2D3D4D5D6D7D8D9DADBDCDDDEDF" +
     "E0E1E2E3E4E5E6E7E8E9EAEBECEDEEEF" +
     "F0F1F2F3F4F5F6F7F8F9FAFBFCFDFEFF";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetHexLength(nuint value) => (BitOperations.Log2(value) + 4) >> 2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendHex(nuint value)
    {
        int n = GetHexLength(value);
        ref char sr = ref AppendRef(n);
        ref uint hexTable = ref Unsafe.As<char, uint>(ref HexTable.GetReference());
        while (value >= 0x10u)
        {
            Unsafe.As<char, uint>(ref Unsafe.Add(ref sr, n -= 2)) = Unsafe.Add(ref hexTable, value & 0xFFu);
            value >>= 8;
        }
        if (n > 0) sr = HexDigits[(int)value];
    }

    private static readonly Unchecked.String DigitPairs =
        "0001020304050607080910111213141516171819" +
        "2021222324252627282930313233343536373839" +
        "4041424344454647484950515253545556575859" +
        "6061626364656667686970717273747576777879" +
        "8081828384858687888990919293949596979899";

    private static readonly Unchecked.String Digits = "0123456789";

    private static readonly Unchecked.SZArray<ulong> TensComplementTable = new ulong[20] {
        0UL, 9UL, 99UL, 999UL, 9999UL, 99999UL, 999999UL, 9999999UL, 99999999UL,
        999999999UL, 9999999999UL, 99999999999UL, 999999999999UL, 9999999999999UL,
        99999999999999UL, 999999999999999UL, 9999999999999999UL, 99999999999999999UL,
        999999999999999999UL, 9999999999999999999UL
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetDecLength(ulong value)
    {
        int guess = ((19 * BitOperations.Log2(value)) >> 6) + 1;
        return (int)(guess - ((long)(TensComplementTable[guess] - value) >> 63));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(ulong value)
    {
        int n = GetDecLength(value);
        ref char sr = ref AppendRef(n);
        ref uint pairTable = ref Unsafe.As<char, uint>(ref DigitPairs.GetReference());
        while (value >= 10)
        {
            Unsafe.As<char, uint>(ref Unsafe.Add(ref sr, n -= 2)) = Unsafe.Add(ref pairTable, (nuint)(value % 100));
            value /= 100;
        }
        if (n > 0) sr = Digits[(uint)value];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(long value)
    {
        if (value == long.MinValue)
        {
            Append("-9223372036854775808");
            return;
        }
        if (value < 0)
        {
            Append('-');
            value = -value;
        }
        Append((ulong)value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(uint value) => Append((ulong)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(int value) => Append((long)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(ushort value) => Append((ulong)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(short value) => Append((long)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(byte value) => Append((ulong)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(sbyte value) => Append((long)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(nuint value) => Append((ulong)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(nint value) => Append((long)value);
}