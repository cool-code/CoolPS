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
            var data = this.data!;
            int length0 = data.GetLength(0);
            int length1 = data.GetLength(1);
            int length2 = data.GetLength(2);
            int length3 = data.GetLength(3);
            int sum = 0;
            for (int i = 0; i < length0; i++)
            {
                for (int j = 0; j < length1; j++)
                {
                    for (int k = 0; k < length2; k++)
                    {
                        for (int l = 0; l < length3; l++)
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
            var data = this.data!;
            uint length0 = (uint)data.GetLength(0);
            uint length1 = (uint)data.GetLength(1);
            uint length2 = (uint)data.GetLength(2);
            uint length3 = (uint)data.GetLength(3);
            int sum = 0;
            for (uint i = 0; i < length0; i++)
            {
                for (uint j = 0; j < length1; j++)
                {
                    for (uint k = 0; k < length2; k++)
                    {
                        for (uint l = 0; l < length3; l++)
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
            var ua = this.ua;
            int length0 = ua.GetLength(0);
            int length1 = ua.GetLength(1);
            int length2 = ua.GetLength(2);
            int length3 = ua.GetLength(3);
            int sum = 0;
            for (int i = 0; i < length0; i++)
            {
                for (int j = 0; j < length1; j++)
                {
                    for (int k = 0; k < length2; k++)
                    {
                        for (int l = 0; l < length3; l++)
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
            var ua = this.ua;
            uint length0 = (uint)ua.GetLength(0);
            uint length1 = (uint)ua.GetLength(1);
            uint length2 = (uint)ua.GetLength(2);
            uint length3 = (uint)ua.GetLength(3);
            int sum = 0;
            for (uint i = 0; i < length0; i++)
            {
                for (uint j = 0; j < length1; j++)
                {
                    for (uint k = 0; k < length2; k++)
                    {
                        for (uint l = 0; l < length3; l++)
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
