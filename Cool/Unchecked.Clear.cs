using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void ClearInternal(ref Block64 reference, nint length)
    {
        if (length == 0) return;
        if ((length & 64) != 0)
        {
            reference = default;
            reference = ref Unsafe.Add(ref reference, 1);
            length -= 64;
        }
        if (length < 2048)
        {
            for (; length > 0; length -= 128)
            {
                reference = default;
                reference = ref Unsafe.Add(ref reference, 1);
                reference = default;
                reference = ref Unsafe.Add(ref reference, 1);
            }
            return;
        }
        nint misaligned = (nint)Unsafe.AsPointer(ref reference) & (64 - 1);
        nint alignmentOffset = 64 - misaligned;
        reference = default;
        reference = ref Unsafe.AddByteOffset(ref reference, alignmentOffset);
        length -= alignmentOffset;
        for (; length > 64; length -= 64)
        {
            reference = default;
            reference = ref Unsafe.Add(ref reference, 1);
        }
        Unsafe.AddByteOffset(ref reference, length - 64) = default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ClearInternal(ref Block16 reference, nint length)
    {
        if (length == 0) return;
        if ((length & 16) != 0)
        {
            reference = default;
            reference = ref Unsafe.Add(ref reference, 1);
            length -= 16;
        }
        if ((length & 32) != 0)
        {
            reference = default;
            reference = ref Unsafe.Add(ref reference, 1);
            reference = default;
            reference = ref Unsafe.Add(ref reference, 1);
            length -= 32;
        }
        ClearInternal(ref Unsafe.As<Block16, Block64>(ref reference), length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ClearInternal(ref ulong reference, nint length)
    {
        if (length == 0) return;
        if ((length & 8) != 0)
        {
            reference = 0;
            reference = ref Unsafe.Add(ref reference, 1);
            length -= 8;
        }
        ClearInternal(ref Unsafe.As<ulong, Block16>(ref reference), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ClearInternal(ref uint reference, nint length)
    {
        if (length == 0) return;
        if ((length & 4) != 0)
        {
            reference = 0;
            reference = ref Unsafe.Add(ref reference, 1);
            length -= 4;
        }
        ClearInternal(ref Unsafe.As<uint, ulong>(ref reference), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ClearInternal(ref ushort reference, nint length)
    {
        if (length == 0) return;
        if ((length & 2) != 0)
        {
            reference = 0;
            reference = ref Unsafe.Add(ref reference, 1);
            length -= 2;
        }
        ClearInternal(ref Unsafe.As<ushort, uint>(ref reference), length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ClearInternal(ref byte reference, nint length)
    {
        if ((length & 1) != 0)
        {
            reference = 0;
            reference = ref Unsafe.Add(ref reference, 1);
            length -= 1;
        }
        ClearInternal(ref Unsafe.As<byte, ushort>(ref reference), length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowClear<T>(ref T reference, nint numElements)
    {
        int i = 0;
        if ((numElements & 1) != 0)
        {
            Unsafe.Add(ref reference, i++) = default!;
        }
        for (; i < numElements; i += 2)
        {
            Unsafe.Add(ref reference, i) = default!;
            Unsafe.Add(ref reference, i + 1) = default!;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear<T>(ref T reference, nint numElements)
    {
        if (numElements == 0) return;
        if (numElements == 1) { reference = default!; return; }
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            SlowClear(ref reference, numElements);
            return;
        }
        nint byteCount = Unsafe.ByteOffset(ref reference, ref Unsafe.Add(ref reference, numElements));
        if (Unsafe.SizeOf<T>() % 64 == 0)
        {
            ClearInternal(ref Unsafe.As<T, Block64>(ref reference), byteCount);
        }
        else if (Unsafe.SizeOf<T>() % 16 == 0)
        {
            ClearInternal(ref Unsafe.As<T, Block16>(ref reference), byteCount);
        }
        else if (Unsafe.SizeOf<T>() % 8 == 0)
        {
            ClearInternal(ref Unsafe.As<T, ulong>(ref reference), byteCount);
        }
        else if (Unsafe.SizeOf<T>() % 4 == 0)
        {
            ClearInternal(ref Unsafe.As<T, uint>(ref reference), byteCount);
        }
        else if (Unsafe.SizeOf<T>() % 2 == 0)
        {
            ClearInternal(ref Unsafe.As<T, ushort>(ref reference), byteCount);
        }
        else
        {
            ClearInternal(ref Unsafe.As<T, byte>(ref reference), byteCount);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref ulong reference, nint numElements)
    {
        if (numElements == 1) { reference = 0; return; }
        if (numElements == 0) return;
        ClearInternal(ref reference, numElements * 8);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref uint reference, nint numElements)
    {
        if (numElements == 1) { reference = 0; return; }
        if (numElements == 0) return;
        ClearInternal(ref reference, numElements * 4);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref ushort reference, nint numElements)
    {
        if (numElements == 1) { reference = 0; return; }
        if (numElements == 0) return;
        ClearInternal(ref reference, numElements * 2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref byte reference, nint numElements)
    {
        if (numElements == 1) { reference = 0; return; }
        if (numElements == 0) return;
        ClearInternal(ref reference, numElements);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref long reference, nint numElements)
    {
        Clear(ref Unsafe.As<long, ulong>(ref reference), numElements);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref int reference, nint numElements)
    {
        Clear(ref Unsafe.As<int, uint>(ref reference), numElements);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref short reference, nint numElements)
    {
        Clear(ref Unsafe.As<short, ushort>(ref reference), numElements);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref sbyte reference, nint numElements)
    {
        Clear(ref Unsafe.As<sbyte, byte>(ref reference), numElements);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Clear(ref nuint reference, nint numElements)
    {
        if (sizeof(nuint) == sizeof(ulong))
        {
            Clear(ref Unsafe.As<nuint, ulong>(ref reference), numElements);
        }
        else
        {
            Clear(ref Unsafe.As<nuint, uint>(ref reference), numElements);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Clear(ref nint reference, nint numElements)
    {
        if (sizeof(nint) == sizeof(long))
        {
            Clear(ref Unsafe.As<nint, ulong>(ref reference), numElements);
        }
        else
        {
            Clear(ref Unsafe.As<nint, uint>(ref reference), numElements);
        }
    }
}
