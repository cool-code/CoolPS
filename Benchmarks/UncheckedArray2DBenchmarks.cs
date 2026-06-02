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
            foreach (var v in data!) sum += v;
            return sum;
        }

        [Benchmark]
        public int Foreach_UncheckedArray2D()
        {
            int sum = 0;
            foreach (var v in ua) sum += v;
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_SystemArray2D()
        {
            var data = this.data!;
            var length0 = data.GetLength(0);
            var length1 = data.GetLength(1);
            int sum = 0;
            for (int i = 0; i < length0; i++)
            {
                for (int j = 0; j < length1; j++)
                {
                    sum += data![i, j];
                }
            }
            return sum;
        }

        [Benchmark]
        public int For_UintIndex_SystemArray2D()
        {
            var data = this.data!;
            var length0 = (uint)data.GetLength(0);
            var length1 = (uint)data.GetLength(1);
            int sum = 0;
            for (uint i = 0; i < length0; i++)
            {
                for (uint j = 0; j < length1; j++)
                {
                    sum += data![i, j];
                }
            }
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_UncheckedArray2D()
        {
            var ua = this.ua;
            int sum = 0;
            int length0 = ua.GetLength(0);
            int length1 = ua.GetLength(1);
            for (int i = 0; i < length0; i++)
            {
                for (int j = 0; j < length1; j++)
                {
                    sum += ua[i, j];
                }
            }
            return sum;
        }

        [Benchmark]
        public int For_UintIndex_UncheckedArray2D()
        {
            var ua = this.ua;
            uint length0 = (uint)ua.GetLength(0);
            uint length1 = (uint)ua.GetLength(1);
            int sum = 0;
            for (uint i = 0; i < length0; i++)
            {
                for (uint j = 0; j < length1; j++)
                {
                    sum += ua[i, j];
                }
            }
            return sum;
        }

    }
}
