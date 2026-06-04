using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    public readonly partial struct Array<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[](Array<T> value) => (T[])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator SZArray<T>(Array<T> value) => (SZArray<T>)(T[])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,](Array<T> value) => (T[,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array2D<T>(Array<T> value) => (Array2D<T>)(T[,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,](Array<T> value) => (T[,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array3D<T>(Array<T> value) => (Array3D<T>)(T[,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,](Array<T> value) => (T[,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array4D<T>(Array<T> value) => (Array4D<T>)(T[,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,](Array<T> value) => (T[,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array5D<T>(Array<T> value) => (Array5D<T>)(T[,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,](Array<T> value) => (T[,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array6D<T>(Array<T> value) => (Array6D<T>)(T[,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,](Array<T> value) => (T[,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array7D<T>(Array<T> value) => (Array7D<T>)(T[,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,](Array<T> value) => (T[,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array8D<T>(Array<T> value) => (Array8D<T>)(T[,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,](Array<T> value) => (T[,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array9D<T>(Array<T> value) => (Array9D<T>)(T[,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array10D<T>(Array<T> value) => (Array10D<T>)(T[,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array11D<T>(Array<T> value) => (Array11D<T>)(T[,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array12D<T>(Array<T> value) => (Array12D<T>)(T[,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array13D<T>(Array<T> value) => (Array13D<T>)(T[,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array14D<T>(Array<T> value) => (Array14D<T>)(T[,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array15D<T>(Array<T> value) => (Array15D<T>)(T[,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array16D<T>(Array<T> value) => (Array16D<T>)(T[,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array17D<T>(Array<T> value) => (Array17D<T>)(T[,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array18D<T>(Array<T> value) => (Array18D<T>)(T[,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array19D<T>(Array<T> value) => (Array19D<T>)(T[,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array20D<T>(Array<T> value) => (Array20D<T>)(T[,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array21D<T>(Array<T> value) => (Array21D<T>)(T[,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array22D<T>(Array<T> value) => (Array22D<T>)(T[,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array23D<T>(Array<T> value) => (Array23D<T>)(T[,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array24D<T>(Array<T> value) => (Array24D<T>)(T[,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array25D<T>(Array<T> value) => (Array25D<T>)(T[,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array26D<T>(Array<T> value) => (Array26D<T>)(T[,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array27D<T>(Array<T> value) => (Array27D<T>)(T[,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array28D<T>(Array<T> value) => (Array28D<T>)(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array29D<T>(Array<T> value) => (Array29D<T>)(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array30D<T>(Array<T> value) => (Array30D<T>)(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array31D<T>(Array<T> value) => (Array31D<T>)(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,](Array<T> value) => (T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Array32D<T>(Array<T> value) => (Array32D<T>)(T[,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,])value._array;
    }
}
