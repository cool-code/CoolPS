using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class UncheckedArray1DNonZeroBenchmarks
    {
        [Params(1024, 8192)]
        public int Size;

        private Array? data;
        private Unchecked.Array<int> ua;

        [GlobalSetup]
        public void Setup()
        {
            int lb = 1;
            data = Array.CreateInstance(typeof(int), new int[] { Size }, new int[] { lb });
            for (int i = lb; i < lb + Size; i++) data.SetValue(i + 1, i);
            ua = data!;
        }

        [Benchmark(Baseline = true)]
        public int Foreach_SystemArray1D()
        {
            int sum = 0;
            foreach (var v in data!) sum += (int)v!;
            return sum;
        }

        [Benchmark]
        public int Foreach_UncheckedArray1D()
        {
            int sum = 0;
            foreach (var v in ua) sum += v;
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_SystemArray1D()
        {
            var data = this.data!;
            int lb = data.GetLowerBound(0);
            int length = data.GetLength(0);
            int sum = 0;
            for (int i = lb; i < lb + length; i++) sum += (int)data.GetValue(i)!;
            return sum;
        }

        [Benchmark]
        public int For_UintIndex_SystemArray1D()
        {
            var data = this.data!;
            uint length = (uint)data.GetLength(0);
            uint lb = (uint)data.GetLowerBound(0);
            int sum = 0;
            for (uint i = 0; i < length; i++) sum += (int)data.GetValue((int)(i + lb))!;
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_UncheckedArray1D()
        {
            var ua = this.ua;
            int lb = ua.GetLowerBound(0);
            int length = ua.GetLength(0);
            int sum = 0;
            for (int i = lb; i < lb + length; i++) sum += ua[i];
            return sum;
        }

        [Benchmark]
        public int For_UintIndex_UncheckedArray1D()
        {
            var ua = this.ua;
            uint length = (uint)ua.GetLength(0);
            uint lb = (uint)ua.GetLowerBound(0);
            int sum = 0;
            for (uint i = 0; i < length; i++) sum += ua[i + lb];
            return sum;
        }
    }

    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class UncheckedArray5DBenchmarks
    {
        [Params(2, 4, 8)]
        public int Size;

        private int[,,,,]? data;
        private Unchecked.Array<int> ua;

        [GlobalSetup]
        public void Setup()
        {
            data = new int[Size, Size, Size, Size, Size];
            for (int i0 = 0; i0 < Size; i0++)
                for (int i1 = 0; i1 < Size; i1++)
                    for (int i2 = 0; i2 < Size; i2++)
                        for (int i3 = 0; i3 < Size; i3++)
                            for (int i4 = 0; i4 < Size; i4++)
                                data[i0, i1, i2, i3, i4] = ((i0 * Size + i1) * Size + i2) * Size * Size + (i3 * Size + i4) + 1;
            ua = data!;
        }

        [Benchmark(Baseline = true)]
        public int Foreach_SystemArray5D()
        {
            int sum = 0;
            foreach (var v in data!) sum += v;
            return sum;
        }

        [Benchmark]
        public int Foreach_UncheckedArray5D()
        {
            int sum = 0;
            foreach (var v in ua) sum += v;
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_SystemArray5D()
        {
            var data = this.data!;
            int l0 = data.GetLength(0);
            int l1 = data.GetLength(1);
            int l2 = data.GetLength(2);
            int l3 = data.GetLength(3);
            int l4 = data.GetLength(4);
            int sum = 0;
            for (int i0 = 0; i0 < l0; i0++)
                for (int i1 = 0; i1 < l1; i1++)
                    for (int i2 = 0; i2 < l2; i2++)
                        for (int i3 = 0; i3 < l3; i3++)
                            for (int i4 = 0; i4 < l4; i4++)
                                sum += data[i0, i1, i2, i3, i4];
            return sum;
        }

        [Benchmark]
        public int For_UintIndex_SystemArray5D()
        {
            var data = this.data!;
            uint l0 = (uint)data.GetLength(0);
            uint l1 = (uint)data.GetLength(1);
            uint l2 = (uint)data.GetLength(2);
            uint l3 = (uint)data.GetLength(3);
            uint l4 = (uint)data.GetLength(4);
            int sum = 0;
            for (uint i0 = 0; i0 < l0; i0++)
                for (uint i1 = 0; i1 < l1; i1++)
                    for (uint i2 = 0; i2 < l2; i2++)
                        for (uint i3 = 0; i3 < l3; i3++)
                            for (uint i4 = 0; i4 < l4; i4++)
                                sum += data[i0, i1, i2, i3, i4];
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_UncheckedArray5D()
        {
            var ua = this.ua;
            int l0 = ua.GetLength(0u);
            int l1 = ua.GetLength(1u);
            int l2 = ua.GetLength(2u);
            int l3 = ua.GetLength(3u);
            int l4 = ua.GetLength(4u);
            int sum = 0;
            for (int i0 = 0; i0 < l0; i0++)
                for (int i1 = 0; i1 < l1; i1++)
                    for (int i2 = 0; i2 < l2; i2++)
                        for (int i3 = 0; i3 < l3; i3++)
                            for (int i4 = 0; i4 < l4; i4++)
                                sum += ua[i0, i1, i2, i3, i4];
            return sum;
        }

        [Benchmark]
        public int For_UintIndex_UncheckedArray5D()
        {
            var ua = this.ua;
            uint l0 = (uint)ua.GetLength(0u);
            uint l1 = (uint)ua.GetLength(1u);
            uint l2 = (uint)ua.GetLength(2u);
            uint l3 = (uint)ua.GetLength(3u);
            uint l4 = (uint)ua.GetLength(4u);
            int sum = 0;
            for (uint i0 = 0; i0 < l0; i0++)
                for (uint i1 = 0; i1 < l1; i1++)
                    for (uint i2 = 0; i2 < l2; i2++)
                        for (uint i3 = 0; i3 < l3; i3++)
                            for (uint i4 = 0; i4 < l4; i4++)
                                sum += ua[i0, i1, i2, i3, i4];
            return sum;
        }
    }

    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class UncheckedArray10DBenchmarks
    {
        [Params(2, 3)]
        public int Size;

        private int[,,,,,,,,,]? data10;
        private Unchecked.Array10D<int> ua;
        private int[] dims = new int[10];

        [GlobalSetup]
        public void Setup()
        {
            int s = Size;
            for (int d = 0; d < 10; d++) dims[d] = s;
            data10 = new int[s, s, s, s, s, s, s, s, s, s];

            int total = 1;
            for (int i = 0; i < 10; i++) total *= dims[i];

            // fill using linear -> indices mapping to avoid deeply nested loops
            int val = 1;
            int[] indices = new int[10];
            for (int linear = 0; linear < total; linear++)
            {
                int rem = linear;
                for (int d = 0; d < 10; d++)
                {
                    int stride = 1;
                    for (int k = d + 1; k < 10; k++) stride *= dims[k];
                    indices[d] = rem / stride;
                    rem = rem % stride;
                }
                data10[indices[0], indices[1], indices[2], indices[3], indices[4], indices[5], indices[6], indices[7], indices[8], indices[9]] = val++;
            }
            ua = data10!;
        }

        [Benchmark(Baseline = true)]
        public int Foreach_SystemArray10D()
        {
            int sum = 0;
            foreach (var v in data10!) sum += (int)v!;
            return sum;
        }

        [Benchmark]
        public int Foreach_UncheckedArray10D()
        {
            int sum = 0;
            foreach (var v in ua) sum += v;
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_SystemArray10D()
        {
            var data = this.data10!;
            int l0 = data.GetLength(0);
            int l1 = data.GetLength(1);
            int l2 = data.GetLength(2);
            int l3 = data.GetLength(3);
            int l4 = data.GetLength(4);
            int l5 = data.GetLength(5);
            int l6 = data.GetLength(6);
            int l7 = data.GetLength(7);
            int l8 = data.GetLength(8);
            int l9 = data.GetLength(9);
            int sum = 0;
            for (int i0 = 0; i0 < l0; i0++)
                for (int i1 = 0; i1 < l1; i1++)
                    for (int i2 = 0; i2 < l2; i2++)
                        for (int i3 = 0; i3 < l3; i3++)
                            for (int i4 = 0; i4 < l4; i4++)
                                for (int i5 = 0; i5 < l5; i5++)
                                    for (int i6 = 0; i6 < l6; i6++)
                                        for (int i7 = 0; i7 < l7; i7++)
                                            for (int i8 = 0; i8 < l8; i8++)
                                                for (int i9 = 0; i9 < l9; i9++)
                                                    sum += data[i0, i1, i2, i3, i4, i5, i6, i7, i8, i9];
            return sum;
        }

        [Benchmark]
        public int For_IntIndex_UncheckedArray10D()
        {
            var ua = this.ua;
            int l0 = ua.GetLength(0);
            int l1 = ua.GetLength(1);
            int l2 = ua.GetLength(2);
            int l3 = ua.GetLength(3);
            int l4 = ua.GetLength(4);
            int l5 = ua.GetLength(5);
            int l6 = ua.GetLength(6);
            int l7 = ua.GetLength(7);
            int l8 = ua.GetLength(8);
            int l9 = ua.GetLength(9);
            int sum = 0;
            for (int i0 = 0; i0 < l0; i0++)
                for (int i1 = 0; i1 < l1; i1++)
                    for (int i2 = 0; i2 < l2; i2++)
                        for (int i3 = 0; i3 < l3; i3++)
                            for (int i4 = 0; i4 < l4; i4++)
                                for (int i5 = 0; i5 < l5; i5++)
                                    for (int i6 = 0; i6 < l6; i6++)
                                        for (int i7 = 0; i7 < l7; i7++)
                                            for (int i8 = 0; i8 < l8; i8++)
                                                for (int i9 = 0; i9 < l9; i9++)
                                                    sum += ua[i0, i1, i2, i3, i4, i5, i6, i7, i8, i9];
            return sum;
        }
    }
}
