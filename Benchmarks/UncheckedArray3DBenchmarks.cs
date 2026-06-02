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
        [Params(16, 64, 512)]
        public int Size;

        private int[,,]? data;
        private Unchecked.Array3D<int> ua;

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
            var data = this.data!;
            int length0 = data.GetLength(0);
            int length1 = data.GetLength(1);
            int length2 = data.GetLength(2);
            int sum = 0;
            for (int i = 0; i < length0; i++)
            {
                for (int j = 0; j < length1; j++)
                {
                    for (int k = 0; k < length2; k++)
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
            var data = this.data!;
            uint length0 = (uint)data.GetLength(0);
            uint length1 = (uint)data.GetLength(1);
            uint length2 = (uint)data.GetLength(2);
            int sum = 0;
            for (uint i = 0; i < length0; i++)
            {
                for (uint j = 0; j < length1; j++)
                {
                    for (uint k = 0; k < length2; k++)
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
            var ua = this.ua;
            int length0 = ua.GetLength(0);
            int length1 = ua.GetLength(1);
            int length2 = ua.GetLength(2);
            int sum = 0;
            for (int i = 0; i < length0; i++)
            {
                for (int j = 0; j < length1; j++)
                {
                    for (int k = 0; k < length2; k++)
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
            var ua = this.ua;
            uint length0 = (uint)ua.GetLength(0);
            uint length1 = (uint)ua.GetLength(1);
            uint length2 = (uint)ua.GetLength(2);
            int sum = 0;
            for (uint i = 0; i < length0; i++)
            {
                for (uint j = 0; j < length1; j++)
                {
                    for (uint k = 0; k < length2; k++)
                    {
                        sum += ua[i, j, k];
                    }
                }
            }
            return sum;
        }

    }
}
