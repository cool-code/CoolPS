using System;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class AnsiInliningBenchmarks
    {
        private int[] colors256;
        private int[] colors16;
        private (int r, int g, int b)[] rgbs;

        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random(42);
            var n = 200_000;
            colors256 = Enumerable.Range(0, n).Select(_ => rnd.Next(0, 256)).ToArray();
            colors16 = Enumerable.Range(0, n).Select(_ => rnd.Next(0, 16)).ToArray();
            rgbs = Enumerable.Range(0, n).Select(_ => (rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256))).ToArray();
        }

        [Benchmark]
        public int Foreground_Xterm256_ReturnLen()
        {
            int sum = 0;
            for (int i = 0; i < colors256.Length; i++)
            {
                sum += Ansi.Foreground((Xterm256)colors256[i]).Length;
            }
            return sum;
        }

        [Benchmark]
        public int Foreground_Xterm16_ReturnLen()
        {
            int sum = 0;
            for (int i = 0; i < colors16.Length; i++)
            {
                sum += Ansi.Foreground((Xterm16)colors16[i]).Length;
            }
            return sum;
        }

        [Benchmark]
        public int Foreground_RGB_ReturnLen()
        {
            int sum = 0;
            for (int i = 0; i < rgbs.Length; i++)
            {
                var t = rgbs[i];
                sum += Ansi.Foreground(t.r, t.g, t.b).Length;
            }
            return sum;
        }

        [Benchmark]
        public int EscapeSGR_ReturnLen()
        {
            int sum = 0;
            for (int i = 0; i < colors16.Length; i++)
                sum += Ansi.EscapeSGR("0;1").Length;
            return sum;
        }

        [Benchmark]
        public int Bold_ReturnLen()
        {
            int sum = 0;
            for (int i = 0; i < 100_000; i++)
                sum += Ansi.Bold("text").Length;
            return sum;
        }
    }
}
