using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe nint Add<T>(this nint start, nint index) => (nint)(((byte*)start) + (index * Unsafe.SizeOf<T>()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe nint Add<T>(this nint start, nuint index) => (nint)(((byte*)start) + (index * (nuint)Unsafe.SizeOf<T>()));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void* ToPointer(this nint ptr) => (void*)ptr;

    [StructLayout(LayoutKind.Sequential)]
    internal sealed class BufferData<T> where T : unmanaged
    {
        public T PlaceHolder; // PlaceHolder for the start of the data;
    }
    public readonly struct Buffer<T> where T : unmanaged
    {
        private readonly BufferData<T>? _data;
        private readonly nint _byteOffset;
        private readonly nuint _length;

        private static readonly nint _arrayDataOffset = IntPtr.Size;

        public uint Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (uint)_length;
        }

        public ulong LongLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer(T[] array)
        {
            if (array == null)
            {
                this = default;
                return;
            }
            ref var data = ref Unsafe.As<T[], BufferData<T>>(ref array);
            _length = (nuint)array.Length;
            _byteOffset = _arrayDataOffset;
            _data = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer(T[] array, int start)
        {
            if (array == null)
            {
                this = default;
                return;
            }
            ref var data = ref Unsafe.As<T[], BufferData<T>>(ref array);
            _length = (nuint)(array.Length - start);
            _byteOffset = _arrayDataOffset.Add<T>(start);
            _data = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer(T[] array, int start, int length)
        {
            if (array == null)
            {
                this = default;
                return;
            }
            ref var data = ref Unsafe.As<T[], BufferData<T>>(ref array);
            _length = (nuint)length;
            _byteOffset = _arrayDataOffset.Add<T>(start);
            _data = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer(Array array)
        {
            if (array == null)
            {
                this = default;
                return;
            }
            ref var data = ref Unsafe.As<Array, BufferData<T>>(ref array);
            _length = (nuint)array.LongLength;
            _byteOffset = Unsafe.ByteOffset(ref data.PlaceHolder, ref array.GetReference<T>());
            _data = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer(Array array, nint start)
        {
            if (array == null)
            {
                this = default;
                return;
            }
            ref var data = ref Unsafe.As<Array, BufferData<T>>(ref array);
            _length = (nuint)((nint)array.LongLength - start);
            _byteOffset = Unsafe.ByteOffset(ref data.PlaceHolder, ref array.GetReference<T>()).Add<T>(start);
            _data = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer(Array array, nint start, uint length)
        {
            if (array == null)
            {
                this = default;
                return;
            }
            ref var data = ref Unsafe.As<Array, BufferData<T>>(ref array);
            _length = length;
            _byteOffset = Unsafe.ByteOffset(ref data.PlaceHolder, ref array.GetReference<T>()).Add<T>(start);
            _data = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Buffer(T* ptr, nuint length)
        {
            _data = null;
            _length = length;
            _byteOffset = (nint)ptr;
        }

        internal Buffer(BufferData<T>? data, nint byteOffset, nuint length)
        {
            _data = data;
            _length = length;
            _byteOffset = byteOffset;
        }

        public ref T this[nuint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_data == null)
                    unsafe { return ref Unsafe.Add(ref Unsafe.AsRef<T>(_byteOffset.ToPointer()), index); }
                else
                    return ref Unsafe.Add(ref Unsafe.AddByteOffset(ref _data.PlaceHolder, _byteOffset), index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference()
        {
            if (_data == null)
            {
                unsafe { return ref Unsafe.AsRef<T>(_byteOffset.ToPointer()); }
            }
            return ref Unsafe.AddByteOffset(ref _data.PlaceHolder, _byteOffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public unsafe ref T GetPinnableReference()
        {
            if (_length != 0) return ref GetReference();
            return ref Unsafe.AsRef<T>(null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer<T> Slice(nint start) => new(_data, _byteOffset.Add<T>(start), _length - (nuint)start);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Buffer<T> Slice(nint start, nuint length) => new(_data, _byteOffset.Add<T>(start), length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            nuint length = _length;
            if (length == 0) return;
            nuint byteLength = length * (nuint)Unsafe.SizeOf<T>();
            Unsafe.InitBlockUnaligned(ref Unsafe.As<T, byte>(ref GetReference()), 0, byteLength);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fill(in T value) => Unchecked.Fill(ref GetReference(), _length, in value);
    }
}