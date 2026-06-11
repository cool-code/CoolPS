using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


namespace Cool;

public static partial class BitSet
{
    private sealed class NativeMemoryHolder : SafeHandle
    {
        // Core design: 64-byte alignment (perfect for AVX-512 and Cache Line performance of modern CPUs)
        private const nuint Alignment = 64;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ref nuint GetReference() => ref Unsafe.AsRef<nuint>((void*)handle);

        // Uniformly set invalid handles to IntPtr.Zero
        public unsafe NativeMemoryHolder(nuint bitHighLimit) : base(invalidHandleValue: IntPtr.Zero, ownsHandle: true)
        {
            nuint byteCount = ByteCount(bitHighLimit);
            nuint padding = (nuint)sizeof(IntPtr) + (Alignment - 1);
            IntPtr rawBlock = Marshal.AllocCoTaskMem((int)(byteCount + padding));

            // Calculate the aligned address: (rawBlock + sizeof(IntPtr) + 63) & ~63
            nuint rawAddr = (nuint)(void*)rawBlock;
            nuint alignedAddr = (rawAddr + (nuint)sizeof(IntPtr) + (Alignment - 1)) & ~(Alignment - 1);

            // The key step: save the original rawBlock pointer in the hole immediately before alignedAddr
            *(((IntPtr*)alignedAddr) - 1) = rawBlock;

            // Use the aligned pointer as the handle managed by this SafeHandle
            SetHandle((IntPtr)(void*)alignedAddr);
        }

        public override bool IsInvalid => handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            if (handle != IntPtr.Zero)
            {
                unsafe
                {
                    IntPtr rawBlock = *(((IntPtr*)handle) - 1);
                    Marshal.FreeCoTaskMem(rawBlock);
                }
                handle = IntPtr.Zero;
                return true;
            }
            return false;
        }
    }

    public static partial class Allocator
    {
        public struct Native : IAllocator, IDisposable
        {
            private nuint _bitHighLimit;
            private NativeMemoryHolder _holder;
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = bitHighLimit;
                _holder = new NativeMemoryHolder(bitHighLimit);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref _holder.GetReference();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly void Dispose()
            {
                _holder?.Dispose();
            }
        }
    }
}