using System;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class BitSet
{
    public static partial class Allocator
    {
        public struct Pooled : IAllocator, IDisposable
        {
            private nuint _bitHighLimit;
            private nuint[] _poolArray;
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = bitHighLimit;
                _poolArray = ArrayPool<nuint>.Shared.Rent((int)WordCount(bitHighLimit));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference() => ref _poolArray.GetReference();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            {
                if (_poolArray != null)
                {
                    ArrayPool<nuint>.Shared.Return(_poolArray);
                    _poolArray = null!;
                }
            }
        }
    }
}