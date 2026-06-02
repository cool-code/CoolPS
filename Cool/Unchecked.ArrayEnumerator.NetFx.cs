#if NETFRAMEWORK
using System;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    public ref struct ArrayEnumerator<T>
    {
        private readonly RawArray _array;
        private readonly nint _offset;
        private readonly uint _length;
        private uint _index;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ArrayEnumerator(Array array, uint offset)
        {
            _array = Unsafe.As<Array, RawArray>(ref array);
            _offset = (nint)offset;
            _length = array.GetLength();
            _index = uint.MaxValue;
        }
        public readonly ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                nuint index = _index;
                return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref _array.Data, _offset)), index);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => _length > ++_index;
    }

    public ref struct SZArrayEnumerator<T>
    {
        private readonly T[] _array;
        private readonly uint _length;
        private uint _index;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal SZArrayEnumerator(T[] array)
        {
            _array = array;
            _length = (uint)array.Length;
            _index = uint.MaxValue;
        }
        public readonly ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                nuint index = _index;
                return ref Unsafe.Add(ref _array.GetReference(), index);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => _length > ++_index;
    }
}
#endif