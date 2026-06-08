#if NETFRAMEWORK
using System.Runtime.CompilerServices;

namespace Cool;

public static class ArrayExtensions
{
    extension(System.Array)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(T[] array, T value) => Unchecked.Fill(ref array.GetReference(), (nuint)array.Length, in value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fill<T>(T[] array, T value, int startIndex, int count) => Unchecked.Fill(ref Unsafe.Add(ref array.GetReference(), startIndex), (nuint)count, in value);
    }
}
#endif