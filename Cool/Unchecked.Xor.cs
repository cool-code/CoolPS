using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Xor(ref Vector<byte> left, ref Vector<byte> right, nint length, nint offset)
    {
        if (length < Vector<byte>.Count) return;
        if ((length & Vector<byte>.Count) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
            offset += Vector<byte>.Count;
        }
        for (; offset < length; offset += Vector<byte>.Count * 2)
        {
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
            Unsafe.AddByteOffset(ref left, offset + Vector<byte>.Count) ^= Unsafe.AddByteOffset(ref right, offset + Vector<byte>.Count);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Xor(ref ulong left, ref ulong right, nint length, nint offset)
    {
        if (length < 8) return;
        if ((length & 8) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
            offset += 8;
        }
        if (!Vector.IsHardwareAccelerated)
        {
            for (; offset < length; offset += 16)
            {
                Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
                Unsafe.AddByteOffset(ref left, offset + 8) ^= Unsafe.AddByteOffset(ref right, offset + 8);
            }
        }
        else
        {
            if (Vector<byte>.Count > 16)
            {
                if ((length & 16) != 0)
                {
                    Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
                    Unsafe.AddByteOffset(ref left, offset + 8) ^= Unsafe.AddByteOffset(ref right, offset + 8);
                    offset += 16;
                }
            }
            if (Vector<byte>.Count > 32)
            {
                if ((length & 32) != 0)
                {
                    Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
                    Unsafe.AddByteOffset(ref left, offset + 8) ^= Unsafe.AddByteOffset(ref right, offset + 8);
                    Unsafe.AddByteOffset(ref left, offset + 16) ^= Unsafe.AddByteOffset(ref right, offset + 16);
                    Unsafe.AddByteOffset(ref left, offset + 24) ^= Unsafe.AddByteOffset(ref right, offset + 24);
                    offset += 32;
                }
            }
            if (Vector<byte>.Count > 64)
            {
                if ((length & 64) != 0)
                {
                    Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
                    Unsafe.AddByteOffset(ref left, offset + 8) ^= Unsafe.AddByteOffset(ref right, offset + 8);
                    Unsafe.AddByteOffset(ref left, offset + 16) ^= Unsafe.AddByteOffset(ref right, offset + 16);
                    Unsafe.AddByteOffset(ref left, offset + 24) ^= Unsafe.AddByteOffset(ref right, offset + 24);
                    Unsafe.AddByteOffset(ref left, offset + 32) ^= Unsafe.AddByteOffset(ref right, offset + 32);
                    Unsafe.AddByteOffset(ref left, offset + 40) ^= Unsafe.AddByteOffset(ref right, offset + 40);
                    Unsafe.AddByteOffset(ref left, offset + 48) ^= Unsafe.AddByteOffset(ref right, offset + 48);
                    Unsafe.AddByteOffset(ref left, offset + 56) ^= Unsafe.AddByteOffset(ref right, offset + 56);
                    offset += 64;
                }
            }
            Xor(ref Unsafe.As<ulong, Vector<byte>>(ref left), ref Unsafe.As<ulong, Vector<byte>>(ref right), length, offset);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Xor(ref uint left, ref uint right, nint length, nint offset)
    {
        if (length < 4) return;
        if ((length & 4) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
            offset += 4;
        }
        Xor(ref Unsafe.As<uint, ulong>(ref left), ref Unsafe.As<uint, ulong>(ref right), length, offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Xor(ref ushort left, ref ushort right, nint length, nint offset)
    {
        if (length < 2) return;
        if ((length & 2) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
            offset += 2;
        }
        Xor(ref Unsafe.As<ushort, uint>(ref left), ref Unsafe.As<ushort, uint>(ref right), length, offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Xor(ref byte left, ref byte right, nint length, nint offset)
    {
        if ((length & 1) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
            offset += 1;
        }
        Xor(ref Unsafe.As<byte, ushort>(ref left), ref Unsafe.As<byte, ushort>(ref right), length, offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void XorBackward(ref Vector<byte> left, ref Vector<byte> right, nint length, nint offset)
    {
        if (length < Vector<byte>.Count) return;
        if ((length & Vector<byte>.Count) != 0)
        {
            offset -= Vector<byte>.Count;
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
        }
        for (offset -= Vector<byte>.Count * 2; offset >= 0; offset -= Vector<byte>.Count * 2)
        {
            Unsafe.AddByteOffset(ref left, offset + Vector<byte>.Count) ^= Unsafe.AddByteOffset(ref right, offset + Vector<byte>.Count);
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void XorBackward(ref ulong left, ref ulong right, nint length, nint offset)
    {
        if (length < 8) return;
        if ((length & 8) != 0)
        {
            offset -= 8;
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
        }
        if (!Vector.IsHardwareAccelerated)
        {
            for (offset -= 16; offset >= 0; offset -= 16)
            {
                Unsafe.AddByteOffset(ref left, offset + 8) ^= Unsafe.AddByteOffset(ref right, offset + 8);
                Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
            }
        }
        else
        {
            if (Vector<byte>.Count > 16)
            {
                if ((length & 16) != 0)
                {
                    offset -= 16;
                    Unsafe.AddByteOffset(ref left, offset + 8) ^= Unsafe.AddByteOffset(ref right, offset + 8);
                    Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
                }
            }
            if (Vector<byte>.Count > 32)
            {
                if ((length & 32) != 0)
                {
                    offset -= 32;
                    Unsafe.AddByteOffset(ref left, offset + 24) ^= Unsafe.AddByteOffset(ref right, offset + 24);
                    Unsafe.AddByteOffset(ref left, offset + 16) ^= Unsafe.AddByteOffset(ref right, offset + 16);
                    Unsafe.AddByteOffset(ref left, offset + 8) ^= Unsafe.AddByteOffset(ref right, offset + 8);
                    Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
                }
            }
            if (Vector<byte>.Count > 64)
            {
                if ((length & 64) != 0)
                {
                    offset -= 64;
                    Unsafe.AddByteOffset(ref left, offset + 56) ^= Unsafe.AddByteOffset(ref right, offset + 56);
                    Unsafe.AddByteOffset(ref left, offset + 48) ^= Unsafe.AddByteOffset(ref right, offset + 48);
                    Unsafe.AddByteOffset(ref left, offset + 40) ^= Unsafe.AddByteOffset(ref right, offset + 40);
                    Unsafe.AddByteOffset(ref left, offset + 32) ^= Unsafe.AddByteOffset(ref right, offset + 32);
                    Unsafe.AddByteOffset(ref left, offset + 24) ^= Unsafe.AddByteOffset(ref right, offset + 24);
                    Unsafe.AddByteOffset(ref left, offset + 16) ^= Unsafe.AddByteOffset(ref right, offset + 16);
                    Unsafe.AddByteOffset(ref left, offset + 8) ^= Unsafe.AddByteOffset(ref right, offset + 8);
                    Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
                }
            }
            XorBackward(ref Unsafe.As<ulong, Vector<byte>>(ref left), ref Unsafe.As<ulong, Vector<byte>>(ref right), length, offset);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void XorBackward(ref uint left, ref uint right, nint length, nint offset)
    {
        if (length < 4) return;
        if ((length & 4) != 0)
        {
            offset -= 4;
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
        }
        XorBackward(ref Unsafe.As<uint, ulong>(ref left), ref Unsafe.As<uint, ulong>(ref right), length, offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void XorBackward(ref ushort left, ref ushort right, nint length, nint offset)
    {
        if (length < 2) return;
        if ((length & 2) != 0)
        {
            offset -= 2;
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
        }
        XorBackward(ref Unsafe.As<ushort, uint>(ref left), ref Unsafe.As<ushort, uint>(ref right), length, offset);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void XorBackward(ref byte left, ref byte right, nint length, nint offset)
    {
        if ((length & 1) != 0)
        {
            offset -= 1;
            Unsafe.AddByteOffset(ref left, offset) ^= Unsafe.AddByteOffset(ref right, offset);
        }
        XorBackward(ref Unsafe.As<byte, ushort>(ref left), ref Unsafe.As<byte, ushort>(ref right), length, offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Xor<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        if (numElements == 0) return;
        nint byteCount = Unsafe.ByteOffset(ref right, ref Unsafe.Add(ref right, numElements));
        ref byte leftBytes = ref Unsafe.As<T, byte>(ref left);
        ref byte rightBytes = ref Unsafe.As<T, byte>(ref right);
        if (Unsafe.AreSame(ref left, ref right))
        {
            Unsafe.InitBlockUnaligned(ref leftBytes, 0, (nuint)byteCount);
            return;
        }
        if ((nuint)Unsafe.ByteOffset(ref right, ref left) < (nuint)byteCount)
        {
            if (Unsafe.SizeOf<T>() % Vector<byte>.Count == 0 && Vector.IsHardwareAccelerated)
            {
                XorBackward(ref Unsafe.As<byte, Vector<byte>>(ref leftBytes), ref Unsafe.As<byte, Vector<byte>>(ref rightBytes), byteCount, byteCount);
            }
            else if (Unsafe.SizeOf<T>() % 8 == 0)
            {
                XorBackward(ref Unsafe.As<byte, ulong>(ref leftBytes), ref Unsafe.As<byte, ulong>(ref rightBytes), byteCount, byteCount);
            }
            else if (Unsafe.SizeOf<T>() % 4 == 0)
            {
                XorBackward(ref Unsafe.As<byte, uint>(ref leftBytes), ref Unsafe.As<byte, uint>(ref rightBytes), byteCount, byteCount);
            }
            else if (Unsafe.SizeOf<T>() % 2 == 0)
            {
                XorBackward(ref Unsafe.As<byte, ushort>(ref leftBytes), ref Unsafe.As<byte, ushort>(ref rightBytes), byteCount, byteCount);
            }
            else
            {
                XorBackward(ref leftBytes, ref rightBytes, byteCount, byteCount);
            }
            return;
        }
        if (Unsafe.SizeOf<T>() % Vector<byte>.Count == 0 && Vector.IsHardwareAccelerated)
        {
            Xor(ref Unsafe.As<byte, Vector<byte>>(ref leftBytes), ref Unsafe.As<byte, Vector<byte>>(ref rightBytes), byteCount, 0);
        }
        else if (Unsafe.SizeOf<T>() % 8 == 0)
        {
            Xor(ref Unsafe.As<byte, ulong>(ref leftBytes), ref Unsafe.As<byte, ulong>(ref rightBytes), byteCount, 0);
        }
        else if (Unsafe.SizeOf<T>() % 4 == 0)
        {
            Xor(ref Unsafe.As<byte, uint>(ref leftBytes), ref Unsafe.As<byte, uint>(ref rightBytes), byteCount, 0);
        }
        else if (Unsafe.SizeOf<T>() % 2 == 0)
        {
            Xor(ref Unsafe.As<byte, ushort>(ref leftBytes), ref Unsafe.As<byte, ushort>(ref rightBytes), byteCount, 0);
        }
        else
        {
            Xor(ref leftBytes, ref rightBytes, byteCount, 0);
        }
    }
}