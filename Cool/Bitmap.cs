using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public unsafe readonly struct Bitmap : IDisposable
{
    private readonly uint* _pbitmap;
    private readonly uint _bitHighLimit;

    public Bitmap(uint bitHighLimit, string ranage)
    {
        _bitHighLimit = bitHighLimit;
        int size = ((int)(bitHighLimit + 1) >> 5) * sizeof(uint);
        _pbitmap = (uint*)Marshal.AllocHGlobal(size);
        Marshal.Copy(new byte[size], 0, (IntPtr)_pbitmap, size);
        string[] parts = ranage.Split(',');
        foreach (var part in parts)
        {
            int dashIndex = part.IndexOf('-');
            if (dashIndex > 0)
            {
                uint start = uint.Parse(part.Substring(0, dashIndex), NumberStyles.HexNumber);
                uint end = Math.Min(bitHighLimit, uint.Parse(part.Substring(dashIndex + 1), NumberStyles.HexNumber));
                for (uint i = start; i <= end; i++) SetBit(i);
            }
            else
            {
                uint pos = uint.Parse(part, NumberStyles.HexNumber);
                SetBit(pos);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void SetBit(uint pos) { if (pos <= _bitHighLimit) { _pbitmap[pos >> 5] |= 1u << (int)(pos & 31); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool GetBit(uint pos) => (pos <= _bitHighLimit) && ((_pbitmap[pos >> 5] & (1u << (int)(pos & 31))) != 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void ClearBit(uint pos) { if (pos <= _bitHighLimit) { _pbitmap[pos >> 5] &= ~(1u << (int)(pos & 31)); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Dispose() => Marshal.FreeHGlobal((IntPtr)_pbitmap);
}
