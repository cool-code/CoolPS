using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Ptr<T>(T* pointer) where T : unmanaged
    {
        public void* Pointer = pointer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ref T GetReference() => ref Unsafe.AsRef<T>(Pointer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Ptr<T>(T* pointer) => new(pointer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T*(Ptr<T> ptr) => (T*)ptr.Pointer;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ptr<T> operator +(Ptr<T> ptr, nint offset) => new((T*)Unsafe.Add<T>(ptr.Pointer, offset));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ptr<T> operator +(Ptr<T> ptr, nuint offset) => new((T*)Unsafe.Add<T>(ptr.Pointer, offset));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ptr<T> operator -(Ptr<T> ptr, nint offset) => new((T*)Unsafe.Subtract<T>(ptr.Pointer, offset));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ptr<T> operator -(Ptr<T> ptr, nuint offset) => new((T*)Unsafe.Subtract<T>(ptr.Pointer, offset));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator -(Ptr<T> left, Ptr<T> right) => (nint)Unsafe.ByteOffset(ref Unsafe.AsRef<T>(right.Pointer), ref Unsafe.AsRef<T>(left.Pointer)) / Unsafe.SizeOf<T>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ptr<T> operator ++(Ptr<T> ptr) => new((T*)Unsafe.Add<T>(ptr.Pointer, 1));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ptr<T> operator --(Ptr<T> ptr) => new((T*)Unsafe.Subtract<T>(ptr.Pointer, 1));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ptr<T> left, Ptr<T> right) => left.Pointer == right.Pointer;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ptr<T> left, Ptr<T> right) => left.Pointer != right.Pointer;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Ptr<T> obj) => this == obj;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? obj) => obj is Ptr<T> ptr && this == ptr;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => ((UIntPtr)Pointer).GetHashCode();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() => ((UIntPtr)Pointer).ToString();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly IntPtr ToIntPtr() => (IntPtr)Pointer;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly UIntPtr ToUIntPtr() => (UIntPtr)Pointer;
        public readonly ref T this[nint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Unsafe.AsRef<T>(Pointer), index);
        }
        public readonly ref T this[nuint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Unsafe.AsRef<T>(Pointer), index);
        }
    }
}
