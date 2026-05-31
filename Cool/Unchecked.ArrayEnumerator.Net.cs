#if !NETFRAMEWORK
using System;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    public ref struct ArrayEnumerator<T>
    {
        private readonly ref T _ref;
        private readonly nint _length;
        private nint _index;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ArrayEnumerator(Array array, nint offset)
        {
            var a = Unsafe.As<RawArray>(array);
            _ref = ref a.GetReference<T>(offset);
            _length = (nint)a.Length;
            _index = -1;
        }
        public readonly ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref _ref, _index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => ++_index < _length;
    }
}
#endif