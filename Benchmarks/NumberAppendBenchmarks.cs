using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class NumberAppendBenchmarks
    {
        private ulong[] ulongs = null!;
        private long[] longs = null!;
        private int[] ints = null!;
        private int n = 200_000;

        [GlobalSetup]
        public void Setup()
        {
            ulongs = new ulong[n];
            for (int i = 0; i < n; i++)
            {
                // deterministic pseudo-random 64-bit values
                ulongs[i] = ((ulong)(uint)i * 6364136223846793005UL) ^ 0x9E3779B97F4A7C15UL;
            }
            longs = new long[n];
            ints = new int[n];
            for (int i = 0; i < n; i++)
            {
                longs[i] = unchecked((long)ulongs[i]);
                ints[i] = unchecked((int)ulongs[i]);
            }
        }

        [Benchmark]
        public int SB_Append_Ulong()
        {
            int sum = 0;
            StringBuilder sb = StringBuilderPool.Shared.Rent(32);
            for (int i = 0; i < ulongs.Length; i++)
            {
                sb.Append(ulongs[i]);
                sum += sb.Length;
                sb.Clear();
            }
            StringBuilderPool.Shared.Return(sb);
            return sum;
        }

        [Benchmark]
        public int VSB_Append_Ulong()
        {
            int sum = 0;
            ValueStringBuilder sb = new(32);
            for (int i = 0; i < ulongs.Length; i++)
            {
                sb.Append(ulongs[i]);
                sum += sb.Length;
                sb.Clear();
            }
            sb.Dispose();
            return sum;
        }

        [Benchmark]
        public int SB_Append_Long()
        {
            int sum = 0;
            StringBuilder sb = StringBuilderPool.Shared.Rent(32);
            for (int i = 0; i < longs.Length; i++)
            {
                sb.Append(longs[i]);
                sum += sb.Length;
                sb.Clear();
            }
            StringBuilderPool.Shared.Return(sb);
            return sum;
        }

        [Benchmark]
        public int VSB_Append_Long()
        {
            int sum = 0;
            ValueStringBuilder sb = new(32);
            for (int i = 0; i < longs.Length; i++)
            {
                sb.Append(longs[i]);
                sum += sb.Length;
                sb.Clear();
            }
            sb.Dispose();
            return sum;
        }

        [Benchmark]
        public int SB_Append_Int()
        {
            int sum = 0;
            StringBuilder sb = StringBuilderPool.Shared.Rent(16);
            for (int i = 0; i < ints.Length; i++)
            {
                sb.Append(ints[i]);
                sum += sb.Length;
                sb.Clear();
            }
            StringBuilderPool.Shared.Return(sb);
            return sum;
        }

        [Benchmark]
        public int VSB_Append_Int()
        {
            int sum = 0;
            ValueStringBuilder sb = new(16);
            for (int i = 0; i < ints.Length; i++)
            {
                sb.Append(ints[i]);
                sum += sb.Length;
                sb.Clear();
            }
            sb.Dispose();
            return sum;
        }
    }
}
