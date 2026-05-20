#if NETFRAMEWORK
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InlineIL;
using static InlineIL.IL.Emit;

namespace Cool;

public static partial class NoBoundCheck
{
    private static readonly IntPtr StringOffset = MeasureStringOffset();
    private static readonly IntPtr ArrayOffset = MeasureArrayOffset();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void* GetObjectAddress(object obj)
    {
        IL.Push(obj);
        Conv_U();
        return IL.ReturnPointer();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe IntPtr MeasureStringOffset()
    {
        string s = "X";
        fixed (char* pChar = s) return (IntPtr)((byte*)pChar - (byte*)GetObjectAddress(s));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe IntPtr MeasureArrayOffset()
    {
        byte[] array = new byte[1];
        fixed (byte* pByte = array) return (IntPtr)(pByte - (byte*)GetObjectAddress(array));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref char GetReference(this string str)
    {
        Ldarg(nameof(str));
        Conv_U();
        Ldsfld(new FieldRef(typeof(NoBoundCheck), nameof(StringOffset)));
        IL.Emit.Add();
        return ref IL.ReturnRef<char>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref T GetReference<T>(this T[] array)
    {
        Ldarg(nameof(array));
        Conv_U();
        Ldsfld(new FieldRef(typeof(NoBoundCheck), nameof(ArrayOffset)));
        IL.Emit.Add();
        return ref IL.ReturnRef<T>();
    }
}
#endif