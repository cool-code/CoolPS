using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class UncheckedArray2DBenchmarks
    {
        [Params(16, 64, 1024, 8192)]
        public int Size;

        private int[,]? data;
        private Unchecked.Array2D<int> ua;

        [GlobalSetup]
        public void Setup()
        {
            data = new int[Size, Size];
            for (int i = 0; i < Size; i++) for (int j = 0; j < Size; j++) data[i, j] = i * Size + j + 1;
            ua = data;
        }

        [Benchmark(Baseline = true)]
        public int Foreach_SystemArray2D()
        {
            int sum = 0;
            foreach (var v in data) sum += v;
            return sum;
        }

        [Benchmark]
        public int Foreach_UncheckedArray2D()
        {
            int sum = 0;
            foreach (var v in ua) sum += v;
            return sum;
        }
    }
}
