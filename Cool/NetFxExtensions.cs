#if NETFRAMEWORK
using System;
using System.Runtime.CompilerServices;

namespace Cool;

public static class NetFxExtensions
{
    extension(Array)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(T[] array, T value) => Unchecked.Fill(ref array.GetReference(), (nuint)array.Length, in value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(T[] array, T value, int startIndex, int count) => Unchecked.Fill(ref Unsafe.Add(ref array.GetReference(), startIndex), (nuint)count, in value);
    }
    extension(UIntPtr)
    {
        public static nuint MaxValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ~(nuint)0;
        }
    }

    extension(Math)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe UIntPtr Min(UIntPtr left, UIntPtr right)
        {
            if (sizeof(UIntPtr) == 8)
            {
                return (UIntPtr)Math.Min((ulong)left, (ulong)right);
            }
            else
            {
                return (UIntPtr)Math.Min((uint)left, (uint)right);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe UIntPtr Max(UIntPtr left, UIntPtr right)
        {
            if (sizeof(UIntPtr) == 8)
            {
                return (UIntPtr)Math.Max((ulong)left, (ulong)right);
            }
            else
            {
                return (UIntPtr)Math.Max((uint)left, (uint)right);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe IntPtr Min(IntPtr left, IntPtr right)
        {
            if (sizeof(IntPtr) == 8)
            {
                return (IntPtr)Math.Min((long)left, (long)right);
            }
            else
            {
                return (IntPtr)Math.Min((int)left, (int)right);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe IntPtr Max(IntPtr left, IntPtr right)
        {
            if (sizeof(IntPtr) == 8)
            {
                return (IntPtr)Math.Max((long)left, (long)right);
            }
            else
            {
                return (IntPtr)Math.Max((int)left, (int)right);
            }
        }
    }
}

#endif