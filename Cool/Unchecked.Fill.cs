using System.Numerics;
using System.Runtime.CompilerServices;
#if DOTNET8_OR_GREATER
using System.Runtime.Intrinsics;
#endif

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FastFill<T>(ref T reference, nuint numElements, in T value)
    {
        // We have enough data for at least one vectorized write.
        Vector<byte> vector;
        int elementSize = Unsafe.SizeOf<T>();

        if (elementSize == 1)
        {
            vector = new Vector<byte>(Unsafe.BitCast<T, byte>(value));
        }
        else if (elementSize == 2)
        {
            vector = (Vector<byte>)new Vector<ushort>(Unsafe.BitCast<T, ushort>(value));
        }
        else if (elementSize == 4)
        {
            // special-case float since it's already passed in a SIMD reg
            vector = (typeof(T) == typeof(float))
                ? (Vector<byte>)new Vector<float>(Unsafe.BitCast<T, float>(value))
                : (Vector<byte>)new Vector<uint>(Unsafe.BitCast<T, uint>(value));
        }
        else if (elementSize == 8)
        {
            // special-case double since it's already passed in a SIMD reg
            vector = (typeof(T) == typeof(double))
                ? (Vector<byte>)new Vector<double>(Unsafe.BitCast<T, double>(value))
                : (Vector<byte>)new Vector<ulong>(Unsafe.BitCast<T, ulong>(value));
        }
        else if (elementSize == Vector<byte>.Count)
        {
            vector = Unsafe.BitCast<T, Vector<byte>>(value);
        }
#if DOTNET8_OR_GREATER
        else if (elementSize == 16)
        {
            if (Vector<byte>.Count == 32)
            {
                vector = Vector256.Create(Unsafe.BitCast<T, Vector128<byte>>(value)).AsVector();
            }
            else if (Vector<byte>.Count == 64)
            {
                vector = Vector512.Create(Unsafe.BitCast<T, Vector128<byte>>(value)).AsVector();
            }
            else
            {
                return false;
            }
        }
        else if (elementSize == 32)
        {
            if (Vector<byte>.Count == 64)
            {
                vector = Vector512.Create(Unsafe.BitCast<T, Vector256<byte>>(value)).AsVector();
            }
            else
            {
                return false;
            }
        }
#endif
        else
        {
            return false;
        }

        ref Vector<byte> dest = ref Unsafe.As<T, Vector<byte>>(ref reference);
        nuint length = numElements * (nuint)elementSize;
        nuint offset = 0;
        for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
        {
            Unsafe.AddByteOffset(ref dest, offset) = vector;
            Unsafe.AddByteOffset(ref dest, offset + (nuint)Vector<byte>.Count) = vector;
        }
        if ((length & (nuint)Vector<byte>.Count) != 0)
        {
            Unsafe.AddByteOffset(ref dest, offset) = vector;
        }
        Unsafe.AddByteOffset(ref dest, length - (nuint)Vector<byte>.Count) = vector;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowFill<T>(ref T reference, nuint numElements, T value)
    {
        nuint i = 0;
        for (nuint stopLoopAtOffset = numElements & ~(nuint)1; i < stopLoopAtOffset; i += 2)
        {
            Unsafe.Add(ref reference, (nint)i + 0) = value;
            Unsafe.Add(ref reference, (nint)i + 1) = value;
        }
        if ((numElements & 1) != 0)
        {
            Unsafe.Add(ref reference, (nint)i) = value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Fill<T>(ref T reference, nuint numElements, in T value)
    {
        if (numElements == 1) { reference = value; return; }
        if (numElements == 0) return;
        if (Unsafe.SizeOf<T>() == 1)
        {
            Unsafe.InitBlockUnaligned(ref Unsafe.As<T, byte>(ref reference), Unsafe.AsRef<T, byte>(value), numElements);
            return;
        }
        if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>() &&
            Vector.IsHardwareAccelerated &&
            Unsafe.SizeOf<T>() <= Vector<byte>.Count &&
            BitOperations.IsPow2(Unsafe.SizeOf<T>()) &&
            numElements >= (uint)(Vector<byte>.Count / Unsafe.SizeOf<T>()))
        {
            if (FastFill(ref reference, numElements, value)) return;
        }
        SlowFill(ref reference, numElements, value);
    }
}