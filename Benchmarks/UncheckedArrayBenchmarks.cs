using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class UncheckedArrayBenchmarks
    {
        [Params(16, 64, 1024, 8192)]
        public int Size;

        private int[]? data;
        private Unchecked.Array<int>? ua;

        [GlobalSetup]
        public void Setup()
        {
            data = new int[Size];
            for (int i = 0; i < Size; i++) data[i] = i + 1;
            ua = data;
        }

        [Benchmark(Baseline = true)]
        public int Foreach_SystemArray()
        {
            int sum = 0;
            foreach (var v in data!) sum += v;
            return sum;
        }

        [Benchmark]
        public int Foreach_UncheckedArray()
        {
            int sum = 0;
            foreach (var v in ua!) sum += v;
            return sum;
        }
    }
}
