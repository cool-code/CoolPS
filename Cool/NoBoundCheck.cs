using System;
using System.Runtime.CompilerServices;
using InlineIL;
using static InlineIL.IL.Emit;

namespace Cool;

public static partial class NoBoundCheck
{
    #region String and Array Accessors
    // 32 bit and 64 bit systems have different sizes for the string object header,
    // so different offsets are needed to access the string data.
    // Note:
    //   The string layout is the same for all strings, so we only need one set of offsets for all strings.
    //   The offsets are relative to the start of the method table pointer, which is located immediately after the object header.
    //   The first character of the string is located immediately after the length field.

    // 32 bit system string layout (12 bytes total):
    // ┌───────────────────────────────┐
    // │ Object Header 4B              │  ← Object Header (4 bytes, for GC and type information)
    // ├───────────────────────────────┤
    // │ Method Table Pointer (4B)     │  ← Relative Method Table Offset 0
    // ├───────────────────────────────┤
    // │ Length Field int (4B)         │  ← Relative Method Table Offset 4
    // ├───────────────────────────────┤
    // │ First Character char (2B)     │  ← Relative Method Table Offset 8  ✅
    // │ Subsequent Characters...      │
    // │ Null Terminator '\0'          │
    // └───────────────────────────────┘
    private const int StringOffset32 = 8;   // 32 bit system string data start offset

    // 64 bit system string layout (20 bytes total):
    // ┌───────────────────────────────┐
    // │ Object Header 8B              │  ← Object Header (8 bytes, for GC and type information)
    // ├───────────────────────────────┤
    // │ Method Table Pointer (8B)     │  ← Relative Method Table Offset 0
    // ├───────────────────────────────┤
    // │ Length Field int (4B)         │  ← Relative Method Table Offset 8
    // ├───────────────────────────────┤
    // │ First Character char (2B)     │  ← Relative Method Table Offset 12 ✅
    // │ Subsequent Characters...      │
    // │ Null Terminator '\0'          │
    // └───────────────────────────────┘
    private const int StringOffset64 = 12;  // 64 bit system string data start offset

    // The Read and Write methods for strings are provided as examples of how to use the IL.Emit API to read and write data at specific offsets within an object. They are not intended to be used as general-purpose string accessors, and they do not perform any bounds checking or null reference checking. Use with caution.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char Read(string str, int index)
    {
        Ldarg(nameof(str));
        Conv_U();
        if (IntPtr.Size == 8)
        {
            Ldc_I4(StringOffset64);
        }
        else
        {
            Ldc_I4(StringOffset32);
        }
        IL.Emit.Add();

        Ldarg(nameof(index));
        Conv_I();
        Ldc_I4_2();
        Mul();
        IL.Emit.Add();

        Ldind_U2();
        return IL.Return<char>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(string str, int index, char value)
    {
        Ldarg(nameof(str));
        Conv_U();
        if (IntPtr.Size == 8)
        {
            Ldc_I4(StringOffset64);
        }
        else
        {
            Ldc_I4(StringOffset32);
        }
        IL.Emit.Add();

        Ldarg(nameof(index));
        Conv_I();
        Ldc_I4_2();
        Mul();
        IL.Emit.Add();

        Ldarg(nameof(value));
        Stind_I2();
    }

    // 32 bit and 64 bit systems have different sizes for the array object header,
    // so different offsets are needed to access the array data.
    // Note:
    //   The array layout is the same for all array types (e.g. T[]), so we only need one set of offsets for all arrays.
    //   The offsets are the same for all array types because the array header layout is determined by the CLR and is consistent across all array types.
    //   The offsets are relative to the start of the method table pointer, which is located immediately after the object header.

    // 32 bit system array layout (12 bytes total):
    // ┌───────────────────────────────┐
    // │ Object Header 4B              │  ← Object Header (4 bytes, for GC and type information)
    // ├───────────────────────────────┤
    // │ Method Table Pointer (4B)     │  ← Relative Method Table Offset 0
    // ├───────────────────────────────┤
    // │ Length Field int (4B)         │  ← Relative Method Table Offset 4
    // ├───────────────────────────────┤
    // │ First Element data[0]         │  ← Relative Method Table Offset 8  ✅
    // │ data[1] ...                   │
    // └───────────────────────────────┘
    private const int ArrayOffset32 = 8;   // 32 bit system array data start offset

    // 64 bit system array layout (24 bytes total):
    // ┌───────────────────────────────┐
    // │ Object Header 8B              │  ← Object Header (8 bytes, for GC and type information)
    // ├───────────────────────────────┤
    // │ Method Table Pointer (8B)     │  ← Relative Method Table Offset 0
    // ├───────────────────────────────┤
    // │ Length Field int (4B)         │  ← Relative Method Table Offset 8
    // ├───────────────────────────────┤
    // │ Padding for alignment (4B)    │  ← Relative Method Table Offset 12 (not used, just padding to align the data on an 8-byte boundary)
    // ├───────────────────────────────┤
    // │ First Element data[0]         │  ← Relative Method Table Offset 16 ✅
    // │ data[1] ...                   │
    // └───────────────────────────────┘
    private const int ArrayOffset64 = 16;  // 64 bit system array data start offset

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Read<T>(T[] array, int index)
    {
        Ldarg(nameof(array));
        Conv_U();
        if (IntPtr.Size == 8)
        {
            Ldc_I4(ArrayOffset64);
        }
        else
        {
            Ldc_I4(ArrayOffset32);
        }
        IL.Emit.Add();
        Ldarg(nameof(index));
        Conv_I();
        Sizeof(typeof(T));
        Mul();
        IL.Emit.Add();
        Ldobj(typeof(T));
        return IL.Return<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write<T>(T[] array, int index, T value)
    {
        Ldarg(nameof(array));
        Conv_U();
        if (IntPtr.Size == 8)
        {
            Ldc_I4(ArrayOffset64);
        }
        else
        {
            Ldc_I4(ArrayOffset32);
        }
        IL.Emit.Add();
        Ldarg(nameof(index));
        Conv_I();
        Sizeof(typeof(T));
        Mul();
        IL.Emit.Add();
        Ldarg(nameof(value));
        Stobj(typeof(T));
    }
    #endregion
}