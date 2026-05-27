using System.Numerics;
using System.Runtime.CompilerServices;
#if !NETFRAMEWORK
using System.Runtime.Intrinsics;
#endif

namespace Cool;

public static partial class Unchecked
{
    #region ref T Accessors
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Read<T>(ref T reference, int index) => Unsafe.Add(ref reference, index);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T>(ref T reference, int index, in T value) => Unsafe.Add(ref reference, index) = value;
    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowFill<T>(ref T reference, nuint numElements, in T value)
    {
        nuint i = 0;

        // Write 8 elements at a time

        if (numElements >= 8)
        {
            nuint stopLoopAtOffset = numElements & ~(nuint)7;
            do
            {
                Unsafe.Add(ref reference, (nint)i + 0) = value;
                Unsafe.Add(ref reference, (nint)i + 1) = value;
                Unsafe.Add(ref reference, (nint)i + 2) = value;
                Unsafe.Add(ref reference, (nint)i + 3) = value;
                Unsafe.Add(ref reference, (nint)i + 4) = value;
                Unsafe.Add(ref reference, (nint)i + 5) = value;
                Unsafe.Add(ref reference, (nint)i + 6) = value;
                Unsafe.Add(ref reference, (nint)i + 7) = value;
            } while ((i += 8) < stopLoopAtOffset);
        }

        // Write next 4 elements if needed

        if ((numElements & 4) != 0)
        {
            Unsafe.Add(ref reference, (nint)i + 0) = value;
            Unsafe.Add(ref reference, (nint)i + 1) = value;
            Unsafe.Add(ref reference, (nint)i + 2) = value;
            Unsafe.Add(ref reference, (nint)i + 3) = value;
            i += 4;
        }

        // Write next 2 elements if needed

        if ((numElements & 2) != 0)
        {
            Unsafe.Add(ref reference, (nint)i + 0) = value;
            Unsafe.Add(ref reference, (nint)i + 1) = value;
            i += 2;
        }

        // Write final element if needed

        if ((numElements & 1) != 0)
        {
            Unsafe.Add(ref reference, (nint)i) = value;
        }
    }
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

        ref byte referenceAsBytes = ref Unsafe.As<T, byte>(ref reference);
        nuint totalByteLength = numElements * (nuint)elementSize; // get this calculation ready ahead of time
        nuint stopLoopAtOffset = totalByteLength & (nuint)(nint)(2 * -Vector<byte>.Count); // intentional sign extension carries the negative bit
        nuint offset = 0;

        // Loop, writing 2 vectors at a time.
        // Compare 'numElements' rather than 'stopLoopAtOffset' because we don't want a dependency
        // on the very recently calculated 'stopLoopAtOffset' value.

        if (numElements >= (uint)(2 * Vector<byte>.Count / elementSize))
        {
            do
            {
                Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref referenceAsBytes, offset), vector);
                Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref referenceAsBytes, offset + (nuint)Vector<byte>.Count), vector);
                offset += (uint)(2 * Vector<byte>.Count);
            } while (offset < stopLoopAtOffset);
        }

        // At this point, if any data remains to be written, it's strictly less than
        // 2 * sizeof(Vector) bytes. The loop above had us write an even number of vectors.
        // If the total byte length instead involves us writing an odd number of vectors, write
        // one additional vector now. The bit check below tells us if we're in an "odd vector
        // count" situation.

        if ((totalByteLength & (nuint)Vector<byte>.Count) != 0)
        {
            Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref referenceAsBytes, offset), vector);
        }

        // It's possible that some small buffer remains to be populated - something that won't
        // fit an entire vector's worth of data. Instead of falling back to a loop, we'll write
        // a vector at the very end of the buffer. This may involve overwriting previously
        // populated data, which is fine since we're splatting the same value for all entries.
        // There's no need to perform a length check here because we already performed this
        // check before entering the vectorized code path.

        Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref referenceAsBytes, totalByteLength - (nuint)Vector<byte>.Count), vector);

        // And we're done!

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Fill<T>(ref T reference, nuint numElements, in T value)
    {
        if (numElements == 0) return;

        int elementSize = Unsafe.SizeOf<T>();
        if (elementSize == 1)
        {
            Unsafe.InitBlockUnaligned(ref Unsafe.As<T, byte>(ref reference), Unsafe.BitCast<T, byte>(value), numElements);
            return;
        }

        if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>() &&
            Vector.IsHardwareAccelerated &&
            elementSize <= Vector<byte>.Count &&
            BitOperations.IsPow2(elementSize) &&
            numElements >= (uint)(Vector<byte>.Count / elementSize))
        {
            if (FastFill(ref reference, numElements, in value)) return;
        }

        SlowFill(ref reference, numElements, in value);
    }
}