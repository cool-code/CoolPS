#if NETFRAMEWORK
using System;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    public ref struct ArrayEnumerator<T>
    {
        private readonly Array _array;
        private readonly nint _offset;
        private readonly nint _length;
        private nint _index;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ArrayEnumerator(Array array, uint offset)
        {
            _array = array;
            _offset = (nint)offset;
            _length = (nint)array.GetLength();
            _index = -1;
        }
        public readonly ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref _array.GetRawArrayData(), _offset)), _index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => ++_index < _length;
    }
}
#endif