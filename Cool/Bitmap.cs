using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public unsafe readonly struct Bitmap : IDisposable
{
    private readonly uint* _pbitmap;
    private readonly int _bitHighLimit;

    public Bitmap(int bitHighLimit, string ranage)
    {
        _bitHighLimit = bitHighLimit;
        int size = ((bitHighLimit + 1) >> 5) * sizeof(uint);
        _pbitmap = (uint*)Marshal.AllocHGlobal(size);
        Marshal.Copy(new byte[size], 0, (IntPtr)_pbitmap, size);
        string[] parts = ranage.Split(',');
        foreach (var part in parts)
        {
            int dashIndex = part.IndexOf('-');
            if (dashIndex > 0)
            {
                int start = int.Parse(part.Substring(0, dashIndex), NumberStyles.HexNumber);
                int end = Math.Min(bitHighLimit, int.Parse(part.Substring(dashIndex + 1), NumberStyles.HexNumber));
                for (int i = start; i <= end; i++) SetBit(i);
            }
            else
            {
                int pos = int.Parse(part, NumberStyles.HexNumber);
                SetBit(pos);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void SetBit(int pos) { if (pos <= _bitHighLimit) { _pbitmap[pos >> 5] |= 1u << (pos & 31); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool GetBit(int pos) => (pos <= _bitHighLimit) && ((_pbitmap[pos >> 5] & (1u << (pos & 31))) != 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void ClearBit(int pos) { if (pos <= _bitHighLimit) { _pbitmap[pos >> 5] &= ~(1u << (pos & 31)); } }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Dispose() => Marshal.FreeHGlobal((IntPtr)_pbitmap);
}
