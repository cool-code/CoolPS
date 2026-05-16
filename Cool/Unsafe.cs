using System;
using System.Runtime.CompilerServices;
using InlineIL;

namespace Cool
{
    public static unsafe class Unsafe
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* AsPointer<T>(ref T value)
        {
            IL.Emit.Ldarg(nameof(value));
            IL.Emit.Conv_U();
            return IL.ReturnPointer();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T As<T>(object o)
            where T : class
        {
            IL.Emit.Ldarg(nameof(o));
            return IL.Return<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TTo As<TFrom, TTo>(ref TFrom source)
        {
            IL.Emit.Ldarg(nameof(source));
            return ref IL.ReturnRef<TTo>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Add<T>(ref T source, int elementOffset)
        {
            IL.Emit.Ldarg(nameof(source));
            IL.Emit.Ldarg(nameof(elementOffset));
            IL.Emit.Sizeof(typeof(T));
            IL.Emit.Conv_I();
            IL.Emit.Mul();
            IL.Emit.Add();
            return ref IL.ReturnRef<T>();
        }

        // 32 bit and 64 bit systems have different sizes for the string object header,
        // so different offsets are needed to access the string data

        private const int StringOffset32 = 8;   // 32 bit system string data start offset
        private const int StringOffset64 = 12;  // 64 bit system string data start offset

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ReadNoBoundsCheck(string str, int index)
            => (IntPtr.Size == 8)
                ? ReadNoBoundsCheck64(str, index)
                : ReadNoBoundsCheck32(str, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ReadNoBoundsCheck32(string str, int index)
        {
            IL.Emit.Ldarg(nameof(str));
            IL.Emit.Conv_U();
            IL.Emit.Ldc_I4(StringOffset32);
            IL.Emit.Add();

            IL.Emit.Ldarg(nameof(index));
            IL.Emit.Conv_I();
            IL.Emit.Ldc_I4_2();
            IL.Emit.Mul();
            IL.Emit.Add();

            IL.Emit.Ldind_U2();
            return IL.Return<char>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ReadNoBoundsCheck64(string str, int index)
        {
            IL.Emit.Ldarg(nameof(str));
            IL.Emit.Conv_U();
            IL.Emit.Ldc_I4(StringOffset64);
            IL.Emit.Add();

            IL.Emit.Ldarg(nameof(index));
            IL.Emit.Conv_I();
            IL.Emit.Ldc_I4_2();
            IL.Emit.Mul();
            IL.Emit.Add();

            IL.Emit.Ldind_U2();
            return IL.Return<char>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteNoBoundsCheck(string str, int index, char value)
        {
            if (IntPtr.Size == 8)
                WriteNoBoundsCheck64(str, index, value);
            else
                WriteNoBoundsCheck32(str, index, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteNoBoundsCheck64(string str, int index, char value)
        {
            IL.Emit.Ldarg(nameof(str));
            IL.Emit.Conv_U();
            IL.Emit.Ldc_I4(StringOffset64);
            IL.Emit.Add();

            IL.Emit.Ldarg(nameof(index));
            IL.Emit.Conv_I();
            IL.Emit.Ldc_I4_2();
            IL.Emit.Mul();
            IL.Emit.Add();

            IL.Emit.Ldarg(nameof(value));
            IL.Emit.Stind_I2();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteNoBoundsCheck32(string str, int index, char value)
        {
            IL.Emit.Ldarg(nameof(str));
            IL.Emit.Conv_U();
            IL.Emit.Ldc_I4(StringOffset32);
            IL.Emit.Add();

            IL.Emit.Ldarg(nameof(index));
            IL.Emit.Conv_I();
            IL.Emit.Ldc_I4_2();
            IL.Emit.Mul();
            IL.Emit.Add();

            IL.Emit.Ldarg(nameof(value));
            IL.Emit.Stind_I2();
        }

        // 32 bit and 64 bit systems have different sizes for the array object header,
        // so different offsets are needed to access the array data

        private const int ArrayOffset32 = 8;   // 32 bit system array data start offset
        private const int ArrayOffset64 = 16;  // 64 bit system array data start offset

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadNoBoundsCheck<T>(T[] array, int index)
            => (IntPtr.Size == 8)
                ? ReadNoBoundsCheck64(array, index)
                : ReadNoBoundsCheck32(array, index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ReadNoBoundsCheck32<T>(T[] array, int index)
        {
            IL.Emit.Ldarg(nameof(array));
            IL.Emit.Conv_U();
            IL.Emit.Ldc_I4(ArrayOffset32);
            IL.Emit.Add();
            IL.Emit.Ldarg(nameof(index));
            IL.Emit.Sizeof(typeof(T));
            IL.Emit.Mul();
            IL.Emit.Add();
            IL.Emit.Ldobj(typeof(T));
            return IL.Return<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ReadNoBoundsCheck64<T>(T[] array, int index)
        {
            IL.Emit.Ldarg(nameof(array));
            IL.Emit.Conv_U();
            IL.Emit.Ldc_I4(ArrayOffset64);
            IL.Emit.Add();
            IL.Emit.Ldarg(nameof(index));
            IL.Emit.Sizeof(typeof(T));
            IL.Emit.Mul();
            IL.Emit.Add();
            IL.Emit.Ldobj(typeof(T));
            return IL.Return<T>();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteNoBoundsCheck<T>(T[] array, int index, T value)
        {
            if (IntPtr.Size == 8)
                WriteNoBoundsCheck64(array, index, value);
            else
                WriteNoBoundsCheck32(array, index, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteNoBoundsCheck32<T>(T[] array, int index, T value)
        {
            IL.Emit.Ldarg(nameof(array));
            IL.Emit.Conv_U();
            IL.Emit.Ldc_I4(ArrayOffset32);
            IL.Emit.Add();
            IL.Emit.Ldarg(nameof(index));
            IL.Emit.Sizeof(typeof(T));
            IL.Emit.Mul();
            IL.Emit.Add();
            IL.Emit.Ldarg(nameof(value));
            IL.Emit.Stobj(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteNoBoundsCheck64<T>(T[] array, int index, T value)
        {
            IL.Emit.Ldarg(nameof(array));
            IL.Emit.Conv_U();
            IL.Emit.Ldc_I4(ArrayOffset64);
            IL.Emit.Add();
            IL.Emit.Ldarg(nameof(index));
            IL.Emit.Sizeof(typeof(T));
            IL.Emit.Mul();
            IL.Emit.Add();
            IL.Emit.Ldarg(nameof(value));
            IL.Emit.Stobj(typeof(T));
        }

    }
}
