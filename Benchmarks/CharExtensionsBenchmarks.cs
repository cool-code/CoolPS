using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class CharExtensionsBenchmarks
    {
        private char[] chars = [];

        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random(42);
            var n = 200_000;
            chars = [.. Enumerable.Range(0, n).Select(_ => (char)rnd.Next(0, 0x10000))];
        }

        [Benchmark]
        public int IsAscii_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsAscii()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsAsciiDigit_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsAsciiDigit()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsAsciiHexDigit_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsAsciiHexDigit()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsAsciiLetter_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsAsciiLetter()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsControl_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsControl()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsC1Control_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsC1Control()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsBetween_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsBetween('a', 'z')) sum++;
            return sum;
        }

        [Benchmark]
        public int IsAsciiHexDigitUpper_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsAsciiHexDigitUpper()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsAsciiHexDigitLower_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsAsciiHexDigitLower()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsAsciiLetterUpper_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsAsciiLetterUpper()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsAsciiLetterLower_Count()
        {
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
                if (chars[i].IsAsciiLetterLower()) sum++;
            return sum;
        }
    }
}
