using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class CodePointBenchmarks
    {
        private CodePoint cpAscii;
        private CodePoint cpBmp;
        private CodePoint cpEmoji;
        private CodePoint cpInvalid;
        private CodePoint cpHigh;
        private CodePoint cpLow;
        private CodePoint[] cpSamples = null!;
        private StringBuilder sb = null!;

        [GlobalSetup]
        public void Setup()
        {
            cpAscii = 'A';
            cpBmp = (CodePoint)0x4F60; // 你
            cpEmoji = (CodePoint)0x1F600; // 😀
            cpInvalid = (CodePoint)0x110000; // invalid
            cpHigh = (CodePoint)0xD83D;
            cpLow = (CodePoint)0xDE00;

            int n = 200_000;
            var rnd = new Random(42);
            var set = new CodePoint[] { cpAscii, cpBmp, cpEmoji, cpInvalid, cpHigh, cpLow };
            cpSamples = new CodePoint[n];
            for (int i = 0; i < n; i++) cpSamples[i] = set[rnd.Next(set.Length)];

            sb = new StringBuilder(64);
        }

        [Benchmark]
        public int ToString_All()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) sum += cpSamples[i].ToString().Length;
            return sum;
        }

        [Benchmark]
        public int ToUnicode_All()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) sum += cpSamples[i].ToUnicode().Length;
            return sum;
        }

        [Benchmark]
        public int Append_Extension_Single()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++)
            {
                sb.Clear();
                CodePointExtensions.Append(sb, cpSamples[i]);
                sum += sb.Length;
            }
            return sum;
        }

        [Benchmark]
        public int Append_Extension_Params()
        {
            int sum = 0;
            var batch = new CodePoint[] { cpAscii, cpBmp, cpEmoji };
            for (int i = 0; i < 100_000; i++)
            {
                sb.Clear();
                CodePointExtensions.Append(sb, batch);
                sum += sb.Length;
            }
            return sum;
        }

        [Benchmark]
        public int AppendUnicode_Extension_Single()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++)
            {
                sb.Clear();
                CodePointExtensions.AppendUnicode(sb, cpSamples[i]);
                sum += sb.Length;
            }
            return sum;
        }

        [Benchmark]
        public int AppendUnicode_Extension_Params()
        {
            int sum = 0;
            var batch = new CodePoint[] { cpAscii, cpBmp, cpEmoji };
            for (int i = 0; i < 100_000; i++)
            {
                sb.Clear();
                CodePointExtensions.AppendUnicode(sb, batch);
                sum += sb.Length;
            }
            return sum;
        }

        [Benchmark]
        public int CharCount_Property()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) sum += cpSamples[i].CharCount;
            return sum;
        }

        [Benchmark]
        public int IsAscii_Property()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) if (cpSamples[i].IsAscii()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsValid_Property()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) if (cpSamples[i].IsValid()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsControl_Property()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) if (cpSamples[i].IsControl()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsWideWidth_Property()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) if (cpSamples[i].IsWideWidth()) sum++;
            return sum;
        }

        [Benchmark]
        public int IsEmoji_Property()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) if (cpSamples[i].IsEmoji()) sum++;
            return sum;
        }

        [Benchmark]
        public int Operators_AddInt()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) sum += (int)(cpSamples[i] + 1);
            return sum;
        }

        [Benchmark]
        public int Operators_SubInt()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) sum += (int)(cpSamples[i] - 1);
            return sum;
        }

        [Benchmark]
        public int Operators_SubtractCp()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++) sum += (cpSamples[i] - cpAscii);
            return sum;
        }

        [Benchmark]
        public int Operators_Increment()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++)
            {
                var x = cpSamples[i];
                x++;
                sum += (int)x;
            }
            return sum;
        }

        [Benchmark]
        public int Operators_PairAdd_FromSurrogate()
        {
            int sum = 0;
            for (int i = 0; i < 100_000; i++)
            {
                var cp = CodePoint.FromSurrogatePair(cpHigh, cpLow);
                sum += (int)cp;
            }
            return sum;
        }

        [Benchmark]
        public int Operators_StringConcat()
        {
            int sum = 0;
            string s = "X";
            for (int i = 0; i < 100_000; i++) sum += (s + cpSamples[i]).Length + (cpSamples[i] + s).Length;
            return sum;
        }

        [Benchmark]
        public int Operator_Multiply10_Count()
        {
            int sum = 0;
            for (int i = 0; i < 50_000; i++) sum += (cpEmoji * 10).Length;
            return sum;
        }

        [Benchmark]
        public int Operator_Multiply1000_Count()
        {
            int sum = 0;
            for (int i = 0; i < 50_000; i++) sum += (cpEmoji * 1000).Length;
            return sum;
        }
        [Benchmark]
        public int Conversions_Implicit_Explicit()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++)
            {
                CodePoint cp = (uint)cpSamples[i]; // implicit from uint
                uint u = (uint)cp; // explicit to uint
                int it = (int)cp; // explicit to int
                sum += (int)u + it;
            }
            return sum;
        }

        [Benchmark]
        public int EqualsAndCompare()
        {
            int sum = 0;
            for (int i = 0; i < cpSamples.Length; i++)
            {
                if (cpSamples[i].Equals(cpAscii)) sum++;
                if (cpSamples[i] == cpAscii) sum++;
                sum += cpSamples[i].CompareTo(cpAscii);
            }
            return sum;
        }
    }
}
