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
    private static readonly Unchecked.SZArray<ulong> UlongTensComplementTable = new ulong[20] {
        0UL, 9UL, 99UL, 999UL, 9999UL, 99999UL, 999999UL, 9999999UL, 99999999UL,
        999999999UL, 9999999999UL, 99999999999UL, 999999999999UL, 9999999999999UL,
        99999999999999UL, 999999999999999UL, 9999999999999999UL, 99999999999999999UL,
        999999999999999999UL, 9999999999999999999UL
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetDecLength(ulong value)
    {
        uint guess = (uint)((19 * BitOperations.Log2(value)) >> 6) + 1;
        return (int)(guess + ((UlongTensComplementTable[guess] - value) >> 63));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(ulong value)
    {
        int n = GetDecLength(value);
        ref char sr = ref AppendRef(n);
        ref uint pairTable = ref Unsafe.As<char, uint>(ref DigitPairs.GetReference());
        while (value >= 100)
        {
            ulong quotient = value / 100;
            ulong remainder = value - (quotient * 100);
            Unsafe.As<char, uint>(ref Unsafe.Add(ref sr, n -= 2)) = Unsafe.Add(ref pairTable, (nuint)remainder);
            value = quotient;
        }
        switch (n)
        {
            case 2: Unsafe.As<char, uint>(ref sr) = Unsafe.Add(ref pairTable, (nuint)value); break;
            case 1: sr = (char)(value + '0'); break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(long value)
    {
        ulong v;
        if (value < 0)
        {
            Append('-');
            v = value == long.MinValue ? 9223372036854775808UL : (ulong)-value;
        }
        else
        {
            v = (ulong)value;
        }
        Append(v);
    }

    private static readonly Unchecked.SZArray<uint> UintTensComplementTable = new uint[11] {
        0U, 9U, 99U, 999U, 9999U, 99999U, 999999U, 9999999U, 99999999U,
        999999999U, uint.MaxValue
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetDecLength(uint value)
    {
        uint guess = (uint)((19 * BitOperations.Log2(value)) >> 6) + 1;
        return (int)(guess + ((UintTensComplementTable[guess] - value) >> 31));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(uint value)
    {
        int n = GetDecLength(value);
        ref char sr = ref AppendRef(n);
        ref uint pairTable = ref Unsafe.As<char, uint>(ref DigitPairs.GetReference());
        while (value >= 100)
        {
            uint quotient = value / 100;
            uint remainder = value - (quotient * 100);
            Unsafe.As<char, uint>(ref Unsafe.Add(ref sr, n -= 2)) = Unsafe.Add(ref pairTable, remainder);
            value = quotient;
        }
        switch (n)
        {
            case 2: Unsafe.As<char, uint>(ref sr) = Unsafe.Add(ref pairTable, value); break;
            case 1: sr = (char)(value + '0'); break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(int value)
    {
        uint v;
        if (value < 0)
        {
            Append('-');
            v = value == int.MinValue ? 2147483648 : (uint)-value;
        }
        else
        {
            v = (uint)value;
        }
        Append(v);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(ushort value) => Append((uint)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(short value) => Append((int)value);

#if NETFRAMEWORK
    private static readonly Unchecked.SZArray<string> ByteTable = new string[256] {
        "0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20",
        "21","22","23","24","25","26","27","28","29","30","31","32","33","34","35","36","37","38","39",
        "40","41","42","43","44","45","46","47","48","49","50","51","52","53","54","55","56","57","58",
        "59","60","61","62","63","64","65","66","67","68","69","70","71","72","73","74","75","76","77",
        "78","79","80","81","82","83","84","85","86","87","88","89","90","91","92","93","94","95","96",
        "97","98","99","100","101","102","103","104","105","106","107","108","109","110","111","112",
        "113","114","115","116","117","118","119","120","121","122","123","124","125","126","127",
        "128","129","130","131","132","133","134","135","136","137","138","139","140","141","142","143",
        "144","145","146","147","148","149","150","151","152","153","154","155","156","157","158","159",
        "160","161","162","163","164","165","166","167","168","169","170","171","172","173","174","175",
        "176","177","178","179","180","181","182","183","184","185","186","187","188","189","190","191",
        "192","193","194","195","196","197","198","199","200","201","202","203","204","205","206","207",
        "208","209","210","211","212","213","214","215","216","217","218","219","220","221","222","223",
        "224","225","226","227","228","229","230","231","232","233","234","235","236","237","238","239",
        "240","241","242","243","244","245","246","247","248","249","250","251","252","253","254","255"
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(byte value) => Append(ByteTable[value]);

    private static readonly Unchecked.SZArray<string> SByteTable = new string[256] {
        "0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20",
        "21","22","23","24","25","26","27","28","29","30","31","32","33","34","35","36","37","38","39",
        "40","41","42","43","44","45","46","47","48","49","50","51","52","53","54","55","56","57","58",
        "59","60","61","62","63","64","65","66","67","68","69","70","71","72","73","74","75","76","77",
        "78","79","80","81","82","83","84","85","86","87","88","89","90","91","92","93","94","95","96",
        "97","98","99","100","101","102","103","104","105","106","107","108","109","110","111","112",
        "113","114","115","116","117","118","119","120","121","122","123","124","125","126","127",
        "-128","-127","-126","-125","-124","-123","-122","-121","-120","-119","-118","-117","-116",
        "-115","-114","-113","-112","-111","-110","-109","-108","-107","-106","-105","-104","-103",
        "-102","-101","-100","-99","-98","-97","-96","-95","-94","-93","-92","-91","-90","-89","-88",
        "-87","-86","-85","-84","-83","-82","-81","-80","-79","-78","-77","-76","-75","-74","-73","-72",
        "-71","-70","-69","-68","-67","-66","-65","-64","-63","-62","-61","-60","-59","-58","-57","-56",
        "-55","-54","-53","-52","-51","-50","-49","-48","-47","-46","-45","-44","-43","-42","-41","-40",
        "-39","-38","-37","-36","-35","-34","-33","-32","-31","-30","-29","-28","-27","-26","-25","-24",
        "-23","-22","-21","-20","-19","-18","-17","-16","-15","-14","-13","-12","-11","-10","-9","-8",
        "-7","-6","-5","-4","-3","-2","-1"
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(sbyte value) => Append(SByteTable[(byte)value]);
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(byte value) => Append((uint)value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(sbyte value) => Append((int)value);
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Append(nuint value)
    {
        if (sizeof(nuint) == 8)
        {
            Append((ulong)value);
        }
        else
        {
            Append((uint)value);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Append(nint value)
    {
        if (sizeof(nint) == 8)
        {
            Append((long)value);
        }
        else
        {
            Append((int)value);
        }
    }
}