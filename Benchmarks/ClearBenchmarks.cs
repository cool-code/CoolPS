using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class ClearBenchmarks
    {
        private byte[] a = null!;
        private byte[] b = null!;

        [Params(1, 64, 256, 1024, 65536, 1048576, 4194304)]
        public int N;

        [GlobalSetup]
        public void GlobalSetup()
        {
            a = new byte[N];
            b = new byte[N];
            for (int i = 0; i < N; i++)
            {
                byte v = (byte)(i & 0xFF);
                a[i] = v;
                b[i] = v;
            }
        }

        [Benchmark(Baseline = true)]
        public void Unchecked_Clear()
        {
            Unchecked.Clear(ref a[0], N);
        }

        [Benchmark]
        public void Unsafe_InitBlockUnaligned()
        {
            Unsafe.InitBlockUnaligned(ref b[0], 0, (nuint)N);
        }
    }
}
