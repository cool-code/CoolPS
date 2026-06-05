using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedAndByteBenchmarks
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
            leftBuffer = new byte[ByteCount + ByteOffset + 64];
            rightBuffer = new byte[ByteCount + ByteOffset + 64];
            for (int i = 0; i < leftBuffer.Length; i++) leftBuffer[i] = (byte)((i * 31) + 7);
            for (int i = 0; i < rightBuffer.Length; i++) rightBuffer[i] = (byte)((i * 17) + 11);
        }

        [Benchmark]
        public void And_Bytes()
        {
            ref byte l = ref Unsafe.Add(ref leftBuffer!.GetReference(), ByteOffset);
            ref byte r = ref Unsafe.Add(ref rightBuffer!.GetReference(), ByteOffset);
            Unchecked.And(ref l, ref r, (nuint)ByteCount);
        }

        [Benchmark(Baseline = true)]
        public void Manual_And_Bytes()
        {
            for (int i = 0; i < ByteCount; i++)
            {
                int idx = i + ByteOffset;
                leftBuffer![idx] &= rightBuffer![idx];
            }
        }
    }

    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedAndUlongBenchmarks
    {
        [Params(1, 25, 1024)]
        public int ElementCount;

        private ulong[]? leftBuffer;
        private ulong[]? rightBuffer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            leftBuffer = new ulong[ElementCount + 8];
            rightBuffer = new ulong[ElementCount + 8];
            for (int i = 0; i < leftBuffer.Length; i++) leftBuffer[i] = (ulong)((uint)i * 0x9E3779B9u + 0xCAFEBABEu);
            for (int i = 0; i < rightBuffer.Length; i++) rightBuffer[i] = (ulong)((uint)i * 0x1234567u + 0xDEADBEEFu);
        }

        [Benchmark]
        public void And_Ulongs()
        {
            ref ulong l = ref leftBuffer!.GetReference();
            ref ulong r = ref rightBuffer!.GetReference();
            Unchecked.And(ref l, ref r, (nuint)ElementCount);
        }

        [Benchmark(Baseline = true)]
        public void Manual_And_Ulongs()
        {
            for (int i = 0; i < ElementCount; i++) leftBuffer![i] &= rightBuffer![i];
        }
    }
}
