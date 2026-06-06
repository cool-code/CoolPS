using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedIsSubsetByteBenchmarks
    {
        [Params(8, 200, 8192)]
        public int ByteCount;

        [Params(0, 1)]
        public int ByteOffset;

        private byte[]? leftBuffer;
        private byte[]? rightBuffer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            leftBuffer = new byte[ByteCount + ByteOffset];
            rightBuffer = new byte[leftBuffer.Length];

            for (int i = 0; i < leftBuffer.Length; i++)
            {
                leftBuffer[i] = 0xAA;
                rightBuffer[i] = 0xFF;
            }
        }

        [Benchmark]
        public bool IsSubset_Bytes()
        {
            ref byte l = ref Unsafe.Add(ref leftBuffer!.GetReference(), ByteOffset);
            ref byte r = ref Unsafe.Add(ref rightBuffer!.GetReference(), ByteOffset);
            return Unchecked.IsSubset(ref l, ref r, (nuint)ByteCount);
        }

        [Benchmark(Baseline = true)]
        public bool Manual_IsSubset_Bytes()
        {
            for (int i = 0; i < ByteCount; i++)
            {
                int idx = i + ByteOffset;
                if ((leftBuffer![idx] & (byte)~rightBuffer![idx]) != 0) return false;
            }
            return true;
        }
    }

    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedIsSubsetUlongBenchmarks
    {
        [Params(1, 25, 1024)]
        public int ElementCount;

        private ulong[]? leftBuffer;
        private ulong[]? rightBuffer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            leftBuffer = new ulong[ElementCount];
            rightBuffer = new ulong[ElementCount];

            for (int i = 0; i < leftBuffer.Length; i++)
            {
                leftBuffer[i] = 0xAAAAAAAAAAAAAAAA;
                rightBuffer[i] = 0xFFFFFFFFFFFFFFFF;
            }
        }

        [Benchmark]
        public bool IsSubset_Ulongs()
        {
            ref ulong l = ref leftBuffer!.GetReference();
            ref ulong r = ref rightBuffer!.GetReference();
            return Unchecked.IsSubset(ref l, ref r, (nuint)ElementCount);
        }

        [Benchmark(Baseline = true)]
        public bool Manual_IsSubset_Ulongs()
        {
            for (int i = 0; i < ElementCount; i++)
            {
                if ((leftBuffer![i] & ~rightBuffer![i]) != 0) return false;
            }
            return true;
        }
    }
}
