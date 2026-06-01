#if !NETFRAMEWORK
using System;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    public ref struct ArrayEnumerator<T>
    {
        private ref T _current;
        private readonly ref T _end;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ArrayEnumerator(Array array, uint offset)
        {
            ref T start = ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref array.GetRawArrayData(), (nint)offset));
            nint len = (nint)array.GetLength();
            _current = ref Unsafe.Subtract(ref start, 1);
            _end = ref Unsafe.Add(ref start, len);
        }

        public readonly ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            _current = ref Unsafe.Add(ref _current, 1);
            return !Unsafe.AreSame(ref _current, ref _end);
        }
    }
}
#endif