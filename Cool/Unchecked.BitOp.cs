using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    // And Operation
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FastAnd(ref byte left, ref byte right, nuint length)
    {
        nuint offset = 0;
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            ref Vector<byte> vleft = ref Unsafe.As<byte, Vector<byte>>(ref left);
            ref Vector<byte> vright = ref Unsafe.As<byte, Vector<byte>>(ref right);
            
            for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
            {
                Unsafe.AddByteOffset(ref vleft, offset) &= Unsafe.AddByteOffset(ref vright, offset);
                Unsafe.AddByteOffset(ref vleft, offset + (nuint)Vector<byte>.Count) &= Unsafe.AddByteOffset(ref vright, offset + (nuint)Vector<byte>.Count);
            }
            
            if ((length & (nuint)Vector<byte>.Count) != 0)
            {
                Unsafe.AddByteOffset(ref vleft, offset) &= Unsafe.AddByteOffset(ref vright, offset);
            }

            Unsafe.AddByteOffset(ref vleft, length - (nuint)Vector<byte>.Count) &= Unsafe.AddByteOffset(ref vright, length - (nuint)Vector<byte>.Count);
            return true;
        }
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SlowAnd(ref ulong left, ref ulong right, nuint length, out nuint offset, out nuint remaining)
    {
        offset = 0;
        ref nuint nleft = ref Unsafe.As<ulong, nuint>(ref left);
        ref nuint nright = ref Unsafe.As<ulong, nuint>(ref right);
        
        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            Unsafe.AddByteOffset(ref nleft, offset) &= Unsafe.AddByteOffset(ref nright, offset);
            Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) &= Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint));
        }
        
        remaining = length - offset;
        if ((remaining & sizeof(ulong)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) &= Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(ulong);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowAnd(ref uint left, ref uint right, nuint length, out nuint offset, out nuint remaining)
    {
        SlowAnd(ref Unsafe.As<uint, ulong>(ref left), ref Unsafe.As<uint, ulong>(ref right), length, out offset, out remaining);
        if ((remaining & sizeof(uint)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) &= Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(uint);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowAnd(ref ushort left, ref ushort right, nuint length, out nuint offset, out nuint remaining)
    {
        SlowAnd(ref Unsafe.As<ushort, uint>(ref left), ref Unsafe.As<ushort, uint>(ref right), length, out offset, out remaining);
        if ((remaining & sizeof(ushort)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) &= Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(ushort);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowAnd(ref byte left, ref byte right, nuint length)
    {
        SlowAnd(ref Unsafe.As<byte, ushort>(ref left), ref Unsafe.As<byte, ushort>(ref right), length, out nuint offset, out nuint remaining);
        if ((remaining & sizeof(byte)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) &= Unsafe.AddByteOffset(ref right, offset);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void And(ref ulong left, ref ulong right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left &= right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ulong>();
        if (FastAnd(ref Unsafe.As<ulong, byte>(ref left), ref Unsafe.As<ulong, byte>(ref right), length)) return;
        SlowAnd(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void And(ref uint left, ref uint right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left &= right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<uint>();
        if (FastAnd(ref Unsafe.As<uint, byte>(ref left), ref Unsafe.As<uint, byte>(ref right), length)) return;
        SlowAnd(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void And(ref ushort left, ref ushort right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left &= right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ushort>();
        if (FastAnd(ref Unsafe.As<ushort, byte>(ref left), ref Unsafe.As<ushort, byte>(ref right), length)) return;
        SlowAnd(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void And(ref byte left, ref byte right, nuint length)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (length == 1) { left &= right; return; }
        if (length == 0) return;
        if (FastAnd(ref left, ref right, length)) return;
        SlowAnd(ref left, ref right, length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void And<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() % sizeof(ulong) == 0)
        {
            And(ref Unsafe.As<T, ulong>(ref left), ref Unsafe.As<T, ulong>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ulong)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(uint) == 0)
        {
            And(ref Unsafe.As<T, uint>(ref left), ref Unsafe.As<T, uint>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(uint)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(ushort) == 0)
        {
            And(ref Unsafe.As<T, ushort>(ref left), ref Unsafe.As<T, ushort>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ushort)));
        }
        else
        {
            And(ref Unsafe.As<T, byte>(ref left), ref Unsafe.As<T, byte>(ref right), numElements * (nuint)Unsafe.SizeOf<T>());
        }
    }

    // Or Operation
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FastOr(ref byte left, ref byte right, nuint length)
    {
        nuint offset = 0;
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            ref Vector<byte> vleft = ref Unsafe.As<byte, Vector<byte>>(ref left);
            ref Vector<byte> vright = ref Unsafe.As<byte, Vector<byte>>(ref right);
            
            for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
            {
                Unsafe.AddByteOffset(ref vleft, offset) |= Unsafe.AddByteOffset(ref vright, offset);
                Unsafe.AddByteOffset(ref vleft, offset + (nuint)Vector<byte>.Count) |= Unsafe.AddByteOffset(ref vright, offset + (nuint)Vector<byte>.Count);
            }
            
            if ((length & (nuint)Vector<byte>.Count) != 0)
            {
                Unsafe.AddByteOffset(ref vleft, offset) |= Unsafe.AddByteOffset(ref vright, offset);
            }

            Unsafe.AddByteOffset(ref vleft, length - (nuint)Vector<byte>.Count) |= Unsafe.AddByteOffset(ref vright, length - (nuint)Vector<byte>.Count);
            return true;
        }
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SlowOr(ref ulong left, ref ulong right, nuint length, out nuint offset, out nuint remaining)
    {
        offset = 0;
        ref nuint nleft = ref Unsafe.As<ulong, nuint>(ref left);
        ref nuint nright = ref Unsafe.As<ulong, nuint>(ref right);
        
        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            Unsafe.AddByteOffset(ref nleft, offset) |= Unsafe.AddByteOffset(ref nright, offset);
            Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) |= Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint));
        }
        
        remaining = length - offset;
        if ((remaining & sizeof(ulong)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) |= Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(ulong);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowOr(ref uint left, ref uint right, nuint length, out nuint offset, out nuint remaining)
    {
        SlowOr(ref Unsafe.As<uint, ulong>(ref left), ref Unsafe.As<uint, ulong>(ref right), length, out offset, out remaining);
        if ((remaining & sizeof(uint)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) |= Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(uint);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowOr(ref ushort left, ref ushort right, nuint length, out nuint offset, out nuint remaining)
    {
        SlowOr(ref Unsafe.As<ushort, uint>(ref left), ref Unsafe.As<ushort, uint>(ref right), length, out offset, out remaining);
        if ((remaining & sizeof(ushort)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) |= Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(ushort);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowOr(ref byte left, ref byte right, nuint length)
    {
        SlowOr(ref Unsafe.As<byte, ushort>(ref left), ref Unsafe.As<byte, ushort>(ref right), length, out nuint offset, out nuint remaining);
        if ((remaining & sizeof(byte)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) |= Unsafe.AddByteOffset(ref right, offset);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Or(ref ulong left, ref ulong right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left |= right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ulong>();
        if (FastOr(ref Unsafe.As<ulong, byte>(ref left), ref Unsafe.As<ulong, byte>(ref right), length)) return;
        SlowOr(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Or(ref uint left, ref uint right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left |= right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<uint>();
        if (FastOr(ref Unsafe.As<uint, byte>(ref left), ref Unsafe.As<uint, byte>(ref right), length)) return;
        SlowOr(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Or(ref ushort left, ref ushort right, nuint numElements)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (numElements == 1) { left |= right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ushort>();
        if (FastOr(ref Unsafe.As<ushort, byte>(ref left), ref Unsafe.As<ushort, byte>(ref right), length)) return;
        SlowOr(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Or(ref byte left, ref byte right, nuint length)
    {
        if (Unsafe.AreSame(ref left, ref right)) return;
        if (length == 1) { left |= right; return; }
        if (length == 0) return;
        if (FastOr(ref left, ref right, length)) return;
        SlowOr(ref left, ref right, length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Or<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() % sizeof(ulong) == 0)
        {
            Or(ref Unsafe.As<T, ulong>(ref left), ref Unsafe.As<T, ulong>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ulong)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(uint) == 0)
        {
            Or(ref Unsafe.As<T, uint>(ref left), ref Unsafe.As<T, uint>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(uint)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(ushort) == 0)
        {
            Or(ref Unsafe.As<T, ushort>(ref left), ref Unsafe.As<T, ushort>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ushort)));
        }
        else
        {
            Or(ref Unsafe.As<T, byte>(ref left), ref Unsafe.As<T, byte>(ref right), numElements * (nuint)Unsafe.SizeOf<T>());
        }
    }

    // AndNot Operation
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool FastAndNot(ref byte left, ref byte right, nuint length)
    {
        if (Unsafe.AreSame(ref left, ref right))
        {
            Unsafe.InitBlockUnaligned(ref left, 0, length);
            return true;
        }
        nuint offset = 0;
        if (Vector.IsHardwareAccelerated && length >= (nuint)Vector<byte>.Count)
        {
            ref Vector<byte> vleft = ref Unsafe.As<byte, Vector<byte>>(ref left);
            ref Vector<byte> vright = ref Unsafe.As<byte, Vector<byte>>(ref right);
            
            for (nuint stopLoopAtOffset = length & ~((nuint)Vector<byte>.Count * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)Vector<byte>.Count * 2)
            {
                Unsafe.AddByteOffset(ref vleft, offset) &= ~Unsafe.AddByteOffset(ref vright, offset);
                Unsafe.AddByteOffset(ref vleft, offset + (nuint)Vector<byte>.Count) &= ~Unsafe.AddByteOffset(ref vright, offset + (nuint)Vector<byte>.Count);
            }
            
            if ((length & (nuint)Vector<byte>.Count) != 0)
            {
                Unsafe.AddByteOffset(ref vleft, offset) &= ~Unsafe.AddByteOffset(ref vright, offset);
            }

            Unsafe.AddByteOffset(ref vleft, length - (nuint)Vector<byte>.Count) &= ~Unsafe.AddByteOffset(ref vright, length - (nuint)Vector<byte>.Count);
            return true;
        }
        return false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void SlowAndNot(ref ulong left, ref ulong right, nuint length, out nuint offset, out nuint remaining)
    {
        offset = 0;
        ref nuint nleft = ref Unsafe.As<ulong, nuint>(ref left);
        ref nuint nright = ref Unsafe.As<ulong, nuint>(ref right);
        
        for (nuint stopLoopAtOffset = length & ~((nuint)sizeof(nuint) * 2 - 1); offset < stopLoopAtOffset; offset += (nuint)sizeof(nuint) * 2)
        {
            Unsafe.AddByteOffset(ref nleft, offset) &= ~Unsafe.AddByteOffset(ref nright, offset);
            Unsafe.AddByteOffset(ref nleft, offset + (nuint)sizeof(nuint)) &= ~Unsafe.AddByteOffset(ref nright, offset + (nuint)sizeof(nuint));
        }
        
        remaining = length - offset;
        if ((remaining & sizeof(ulong)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) &= ~Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(ulong);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowAndNot(ref uint left, ref uint right, nuint length, out nuint offset, out nuint remaining)
    {
        SlowAndNot(ref Unsafe.As<uint, ulong>(ref left), ref Unsafe.As<uint, ulong>(ref right), length, out offset, out remaining);
        if ((remaining & sizeof(uint)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) &= ~Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(uint);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowAndNot(ref ushort left, ref ushort right, nuint length, out nuint offset, out nuint remaining)
    {
        SlowAndNot(ref Unsafe.As<ushort, uint>(ref left), ref Unsafe.As<ushort, uint>(ref right), length, out offset, out remaining);
        if ((remaining & sizeof(ushort)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) &= (ushort)~Unsafe.AddByteOffset(ref right, offset);
            offset += sizeof(ushort);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SlowAndNot(ref byte left, ref byte right, nuint length)
    {
        SlowAndNot(ref Unsafe.As<byte, ushort>(ref left), ref Unsafe.As<byte, ushort>(ref right), length, out nuint offset, out nuint remaining);
        if ((remaining & sizeof(byte)) != 0)
        {
            Unsafe.AddByteOffset(ref left, offset) &= (byte)~Unsafe.AddByteOffset(ref right, offset);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AndNot(ref ulong left, ref ulong right, nuint numElements)
    {
        if (numElements == 1) { left &= ~right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ulong>();
        if (FastAndNot(ref Unsafe.As<ulong, byte>(ref left), ref Unsafe.As<ulong, byte>(ref right), length)) return;
        SlowAndNot(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AndNot(ref uint left, ref uint right, nuint numElements)
    {
        if (numElements == 1) { left &= ~right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<uint>();
        if (FastAndNot(ref Unsafe.As<uint, byte>(ref left), ref Unsafe.As<uint, byte>(ref right), length)) return;
        SlowAndNot(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AndNot(ref ushort left, ref ushort right, nuint numElements)
    {
        if (numElements == 1) { left &= (ushort)~right; return; }
        if (numElements == 0) return;
        nuint length = numElements * (nuint)Unsafe.SizeOf<ushort>();
        if (FastAndNot(ref Unsafe.As<ushort, byte>(ref left), ref Unsafe.As<ushort, byte>(ref right), length)) return;
        SlowAndNot(ref left, ref right, length, out _, out _);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AndNot(ref byte left, ref byte right, nuint length)
    {
        if (length == 1) { left &= (byte)~right; return; }
        if (length == 0) return;
        if (FastAndNot(ref left, ref right, length)) return;
        SlowAndNot(ref left, ref right, length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AndNot<T>(ref T left, ref T right, nuint numElements) where T : unmanaged
    {
        if ((nuint)Unsafe.SizeOf<T>() % sizeof(ulong) == 0)
        {
            AndNot(ref Unsafe.As<T, ulong>(ref left), ref Unsafe.As<T, ulong>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ulong)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(uint) == 0)
        {
            AndNot(ref Unsafe.As<T, uint>(ref left), ref Unsafe.As<T, uint>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(uint)));
        }
        else if ((nuint)Unsafe.SizeOf<T>() % sizeof(ushort) == 0)
        {
            AndNot(ref Unsafe.As<T, ushort>(ref left), ref Unsafe.As<T, ushort>(ref right), numElements * ((nuint)Unsafe.SizeOf<T>() / sizeof(ushort)));
        }
        else
        {
            AndNot(ref Unsafe.As<T, byte>(ref left), ref Unsafe.As<T, byte>(ref right), numElements * (nuint)Unsafe.SizeOf<T>());
        }
    }

}
