using System;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class BitSet
{
    public static partial class Allocator
    {
        public struct Native : IAllocator, IDisposable
        {
            private nuint _bitHighLimit;
            private NativeMemoryManager _manager;
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = bitHighLimit;
                _manager = new NativeMemoryManager(ByteCount(bitHighLimit));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly unsafe ref nuint GetReference()
            {
                return ref Unsafe.AsRef<nuint>((void*)_manager.GetPointer());
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly void Dispose()
            {
                _manager?.Dispose();
            }
        }
    }
}