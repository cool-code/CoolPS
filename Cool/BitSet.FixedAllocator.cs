using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;


namespace Cool;

public static partial class BitSet
{
    public static partial class Allocator
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed8B : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[2];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(63u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed16B : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[4];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(127u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed32B : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[8];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(255u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed64B : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[16];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(511u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed128B : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[32];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(1023u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed256B : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[64];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(2047u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed512B : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[128];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(4095u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed1KB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[256];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(8191u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed2KB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[512];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(16383u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed4KB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[1024];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(32767u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed8KB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[2048];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(65535u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed16KB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[4096];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(131071u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed32KB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[8192];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(262143u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed64KB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[16384];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(524287u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed128KB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[32768];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(1048575u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed256KB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[65536];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(2097151u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed512KB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[131072];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(4194303u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed1MB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[262144];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(8388607u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed2MB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[524288];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(16777215u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed4MB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[1048576];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(33554431u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed8MB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[2097152];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(67108863u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed16MB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[4194304];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(134217727u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed32MB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[8388608];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(268435455u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed64MB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[16777216];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(536870911u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed128MB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[33554432];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(1073741823u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed256MB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[67108864];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(2147483647u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Fixed512MB : IAllocator
        {
            private nuint _bitHighLimit;
            private fixed uint _holder[134217728];
            public readonly nuint BitHighLimit
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _bitHighLimit;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Init(nuint bitHighLimit)
            {
                _bitHighLimit = Math.Min((UIntPtr)bitHighLimit, (UIntPtr)(4294967295u));
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly ref nuint GetReference()
            {
                return ref Unsafe.AsRef<uint, nuint>(in _holder[0]);
            }
        }
    }
}