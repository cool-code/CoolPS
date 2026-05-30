using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class UncheckedArray3DBenchmarks
    {
        [Params(16, 64, 1024)]
        public int Size;

        private int[,,]? data;
        private Unchecked.Array<int> ua;

        [GlobalSetup]
        public void Setup()
        {
            data = new int[Size, Size, Size];
            for (int i = 0; i < Size; i++) for (int j = 0; j < Size; j++) for (int k = 0; k < Size; k++) data[i, j, k] = i * Size * Size + j * Size + k + 1;
            ua = data;
        }

        [Benchmark(Baseline = true)]
        public int Foreach_SystemArray3D()
        {
            int sum = 0;
            foreach (var v in data!) sum += v;
            return sum;
        }

        [Benchmark]
        public int Foreach_UncheckedArray3D()
        {
            int sum = 0;
            foreach (var v in ua) sum += v;
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_SystemArray3D()
        {
            int sum = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    for (int k = 0; k < Size; k++)
                    {
                        sum += data![i, j, k];
                    }
                }
            }
            return sum;
        }

        [Benchmark]
        public int For_UintIndex_SystemArray3D()
        {
            int sum = 0;
            for (uint i = 0; i < Size; i++)
            {
                for (uint j = 0; j < Size; j++)
                {
                    for (uint k = 0; k < Size; k++)
                    {
                        sum += data![i, j, k];
                    }
                }
            }
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_UncheckedArray3D()
        {
            int sum = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    for (int k = 0; k < Size; k++)
                    {
                        sum += ua[i, j, k];
                    }
                }
            }
            return sum;
        }

        [Benchmark]
        public int For_UintIndex_UncheckedArray3D()
        {
            int sum = 0;
            for (uint i = 0; i < Size; i++)
            {
                for (uint j = 0; j < Size; j++)
                {
                    for (uint k = 0; k < Size; k++)
                    {
                        sum += ua[i, j, k];
                    }
                }
            }
            return sum;
        }

    }
}
