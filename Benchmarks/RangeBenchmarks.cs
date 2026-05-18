using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class RangeBenchmarks
    {
        private const uint HighLimit = 0xFFFFu;
        private string longRange = null!;

        [GlobalSetup]
        public void Setup()
        {
            var sb = new StringBuilder(32 * 1024);
            const int blocks = 2048;
            for (int i = 0; i < blocks; i++)
            {
                int start = i * 3;
                int end = start + 255;

                sb.Append(start.ToString("X"));
                sb.Append(',');
                sb.Append((start + 1).ToString("X"));
                sb.Append('~');
                sb.Append(end.ToString("X"));

                if (i < blocks - 1) sb.Append(',');
            }

            longRange = sb.ToString();
        }

        [Benchmark]
        public int EnumerateCount_UInt()
        {
            int count = 0;
            foreach (var v in Range.Create(longRange, HighLimit))
            {
                count++;
            }
            return count;
        }

        [Benchmark]
        public uint EnumerateSum_UInt()
        {
            uint sum = 0;
            foreach (var v in Range.Create(longRange, HighLimit))
            {
                sum += v;
            }
            return sum;
        }
    }
}
