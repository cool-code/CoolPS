using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class UncheckedArray4DBenchmarks
    {
        [Params(4, 16, 128)]
        public int Size;

        private int[,,,]? data;
        private Unchecked.Array4D<int> ua;

        [GlobalSetup]
        public void Setup()
        {
            data = new int[Size, Size, Size, Size];
            for (int i = 0; i < Size; i++) for (int j = 0; j < Size; j++) for (int k = 0; k < Size; k++) for (int l = 0; l < Size; l++) data[i, j, k, l] = i * Size * Size * Size + j * Size * Size + k * Size + l + 1;
            ua = data;
        }

        [Benchmark(Baseline = true)]
        public int Foreach_SystemArray4D()
        {
            int sum = 0;
            foreach (var v in data!) sum += v;
            return sum;
        }

        [Benchmark]
        public int Foreach_UncheckedArray4D()
        {
            int sum = 0;
            foreach (var v in ua) sum += v;
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_SystemArray4D()
        {
            int sum = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    for (int k = 0; k < Size; k++)
                    {
                        for (int l = 0; l < Size; l++)
                        {
                            sum += data![i, j, k, l];
                        }
                    }
                }
            }
            return sum;
        }

        [Benchmark]
        public int For_UintIndex_SystemArray4D()
        {
            int sum = 0;
            for (uint i = 0; i < Size; i++)
            {
                for (uint j = 0; j < Size; j++)
                {
                    for (uint k = 0; k < Size; k++)
                    {
                        for (uint l = 0; l < Size; l++)
                        {
                            sum += data![i, j, k, l];
                        }
                    }
                }
            }
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_UncheckedArray4D()
        {
            int sum = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    for (int k = 0; k < Size; k++)
                    {
                        for (int l = 0; l < Size; l++)
                        {
                            sum += ua[i, j, k, l];
                        }
                    }
                }
            }
            return sum;
        }

        [Benchmark]
        public int For_UintIndex_UncheckedArray4D()
        {
            int sum = 0;
            for (uint i = 0; i < Size; i++)
            {
                for (uint j = 0; j < Size; j++)
                {
                    for (uint k = 0; k < Size; k++)
                    {
                        for (uint l = 0; l < Size; l++)
                        {
                            sum += ua[i, j, k, l];
                        }
                    }
                }
            }
            return sum;
        }

    }
}
