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
        private byte[] dst1 = null!;
        private byte[] dst2 = null!;

        [Params(1, 64, 256, 1024, 2048, 65536, 1048576, 4194304)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            src = new byte[N];
            dst1 = new byte[N];
            dst2 = new byte[N];
            for (int i = 0; i < N; i++) src[i] = (byte)(i & 0xFF);
        }


        [Benchmark(Baseline = true)]
        public void Unchecked_Copy()
        {
            Unchecked.Copy(ref dst1[0], ref src[0], N);
        }

        [Benchmark]
        public void Unchecked_CopyBackword()
        {
            if (N < 32) return;
            Unchecked.Copy(ref dst1[32], ref dst1[0], N - 32);
        }

        [Benchmark]
        public void Unsafe_CopyBlockUnaligned()
        {
            Unsafe.CopyBlockUnaligned(ref dst2[0], ref src[0], (nuint)N);
        }

        [Benchmark]
        public void Unsafe_CopyBackword()
        {
            if (N < 32) return;
#if NETFRAMEWORK
            Unsafe.CopyBlockUnaligned(ref dst2[32], ref dst2[0], (nuint)N - 32);
#else
            MemoryMarshal.CreateSpan(ref dst2[0], N - 32).CopyTo(MemoryMarshal.CreateSpan(ref dst2[32], N - 32));
#endif
        }
    }
}
