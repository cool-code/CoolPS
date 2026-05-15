using System.Runtime.CompilerServices;

namespace Cool.NumberDrivers;

internal static class Hex
{
    private static readonly byte[] _hexTable = CreateHexTable();
    private static byte[] CreateHexTable()
    {
        var table = new byte[128];
        for (char c = '0'; c <= '9'; c++)
            table[c] = (byte)(c - '0');
        for (char c = 'A'; c <= 'F'; c++)
            table[c] = (byte)(c - 'A' + 10);
        for (char c = 'a'; c <= 'f'; c++)
            table[c] = (byte)(c - 'a' + 10);
        return table;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Lookup(char c)
    {
        return c.IsAsciiHexDigit() ? Unsafe.ReadNoBoundsCheck(_hexTable, c) : (byte)0;
    }
}