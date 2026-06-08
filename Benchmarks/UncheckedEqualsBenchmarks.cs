using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedEqualsByteBenchmarks
    {
        [Params(8, 200, 8192)]
        public int ByteCount;

        [Params(0, 1)]
        public int ByteOffset;

        [Params("Equal", "Start", "Middle", "End", "Random")]
        public string Scenario = "Equal";

        private byte[]? leftBuffer;
        private byte[]? rightBuffer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            // allocate a little extra to avoid edge issues with offsets
            leftBuffer = new byte[ByteCount + ByteOffset + 4];
            rightBuffer = new byte[ByteCount + ByteOffset + 4];
            for (int i = 0; i < leftBuffer.Length; i++) leftBuffer[i] = (byte)((i * 31) + 7);

            // start with right == left, then introduce a mismatch depending on Scenario
            Array.Copy(leftBuffer, 0, rightBuffer, 0, rightBuffer.Length);

            if (ByteCount > 0 && Scenario != "Equal")
            {
                int start = ByteOffset;
                int idx;
                switch (Scenario)
                {
                    case "Start": idx = start; break;
                    case "Middle": idx = start + (ByteCount / 2); break;
                    case "End": idx = start + (ByteCount - 1); break;
                    case "Random":
                        var rnd = new Random(42);
                        idx = start + rnd.Next(0, ByteCount);
                        break;
                    default: idx = -1; break;
                }
                if (idx >= 0 && idx < rightBuffer.Length) rightBuffer[idx] ^= 0xFF;
            }
        }

        [Benchmark]
        public bool Equals_Bytes_Unchecked()
        {
            ref byte l = ref Unsafe.Add(ref leftBuffer!.GetReference(), ByteOffset);
            ref byte r = ref Unsafe.Add(ref rightBuffer!.GetReference(), ByteOffset);
            return Unchecked.Equals(ref l, ref r, (nuint)ByteCount);
        }

        [Benchmark(Baseline = true)]
        public bool Manual_Equals_Bytes()
        {
            int start = ByteOffset;
            bool eq = true;
            for (int i = 0; i < ByteCount; i++)
            {
                if (leftBuffer![start + i] != rightBuffer![start + i]) { eq = false; break; }
            }
            return eq;
        }
    }

    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedEqualsUlongBenchmarks
    {
        [Params(1, 25, 1024)]
        public int ElementCount;

        [Params("Equal", "Start", "Middle", "End", "Random")]
        public string Scenario = "Equal";

        private ulong[]? leftBuffer;
        private ulong[]? rightBuffer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            leftBuffer = new ulong[Math.Max(1, ElementCount)];
            rightBuffer = new ulong[leftBuffer.Length];
            for (int i = 0; i < leftBuffer.Length; i++) leftBuffer[i] = (ulong)((uint)i * 0x9E3779B9u + 0xCAFEBABEu);
            Array.Copy(leftBuffer, 0, rightBuffer, 0, rightBuffer.Length);

            if (ElementCount > 0 && Scenario != "Equal")
            {
                int idx;
                switch (Scenario)
                {
                    case "Start": idx = 0; break;
                    case "Middle": idx = ElementCount / 2; break;
                    case "End": idx = ElementCount - 1; break;
                    case "Random": idx = (new Random(42)).Next(0, ElementCount); break;
                    default: idx = -1; break;
                }
                if (idx >= 0 && idx < rightBuffer.Length) rightBuffer[idx] = rightBuffer[idx] ^ 0xDEADBEEFDEADBEEFUL;
            }
        }

        [Benchmark]
        public bool Equals_Ulongs_Unchecked()
        {
            ref ulong l = ref leftBuffer!.GetReference();
            ref ulong r = ref rightBuffer!.GetReference();
            return Unchecked.Equals(ref l, ref r, (nuint)ElementCount);
        }

        [Benchmark(Baseline = true)]
        public bool Manual_Equals_Ulongs()
        {
            bool eq = true;
            for (int i = 0; i < ElementCount; i++)
            {
                if (leftBuffer![i] != rightBuffer![i]) { eq = false; break; }
            }
            return eq;
        }
    }
}
