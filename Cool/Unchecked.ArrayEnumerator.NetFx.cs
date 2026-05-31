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
        private readonly nint _length;
        private nint _index;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ArrayEnumerator(Array array, nint offset)
        {
            _array = Unsafe.As<RawArray>(array);
            _offset = offset;
            _length = (nint)_array.Length;
            _index = -1;
        }
        public readonly ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _array.GetElement<T>(_offset, _index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => ++_index < _length;
    }
}
#endif