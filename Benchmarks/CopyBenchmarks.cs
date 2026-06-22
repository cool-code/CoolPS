using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class CopyBenchmarks
    {
        private byte[] src = null!;
        private byte[] dst = null!;

        [Params(1, 16, 64, 128, 256, 512, 800, 1024, 1600, 2048, 4096, 8192, 16384, 32768, 65536, 1048576, 4194304, 16777216, 33554432, 50331648, 67108864, 134217728, 536870912)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            src = new byte[N];
            dst = new byte[N];
            for (int i = 0; i < N; i++) src[i] = (byte)(i & 0xFF);
        }


        [Benchmark(Baseline = true)]
        public void Unchecked_Copy()
        {
            Unchecked.Copy(src, dst, N);
        }

        [Benchmark]
        public void Unchecked_CopyBackword()
        {
            if (N < 32) return;
            Unchecked.Copy(dst, 0, dst, 32, N - 32);
        }

        [Benchmark]
        public void System_Copy()
        {
#if NETFRAMEWORK
            Array.Copy(src, 0, dst, 0, N);
#else
            MemoryMarshal.CreateSpan(ref src[0], N).CopyTo(MemoryMarshal.CreateSpan(ref dst[0], N));
#endif
        }

        [Benchmark]
        public void System_CopyBackword()
        {
            if (N < 32) return;
#if NETFRAMEWORK
            Array.Copy(dst, 0, dst, 32, N - 32);
#else
            MemoryMarshal.CreateSpan(ref dst[0], N - 32).CopyTo(MemoryMarshal.CreateSpan(ref dst[32], N - 32));
#endif
        }
    }


    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class CopyObjectBenchmarks
    {
        private object[] src = null!;
        private object[] dst = null!;

        [Params(1, 16, 64, 256, 1024, 2048, 65536, 1048576, 4194304)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            src = new object[N];
            dst = new object[N];
            for (int i = 0; i < N; i++) src[i] = new object();
        }


        [Benchmark(Baseline = true)]
        public void Unchecked_Copy()
        {
            Unchecked.Copy(src, dst, N);
        }

        [Benchmark]
        public void Unchecked_CopyBackword()
        {
            if (N < 32) return;
            Unchecked.Copy(dst, 0, dst, 32, N - 32);
        }

        [Benchmark]
        public void System_Copy()
        {
#if NETFRAMEWORK
            Array.Copy(src, 0, dst, 0, N);
#else
            MemoryMarshal.CreateSpan(ref src[0], N).CopyTo(MemoryMarshal.CreateSpan(ref dst[0], N));
#endif
        }

        [Benchmark]
        public void System_CopyBackword()
        {
            if (N < 32) return;
#if NETFRAMEWORK
            Array.Copy(dst, 0, dst, 32, N - 32);
#else
            MemoryMarshal.CreateSpan(ref dst[0], N - 32).CopyTo(MemoryMarshal.CreateSpan(ref dst[32], N - 32));
#endif
        }
    }

}
