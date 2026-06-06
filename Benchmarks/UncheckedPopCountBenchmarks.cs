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
    public class UncheckedPopCountByteBenchmarks
    {
        [Params(8, 200, 8192)]
        public int ByteCount;

        [Params(0, 1)]
        public int ByteOffset;

        private byte[]? buffer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            buffer = new byte[ByteCount + ByteOffset + 64];
            for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)((i * 31) + 7);
        }

        [Benchmark]
        public nuint PopCount_Bytes()
        {
            ref byte r = ref Unsafe.Add(ref buffer!.GetReference(), ByteOffset);
            return Unchecked.PopCount(ref r, (nuint)ByteCount);
        }

        [Benchmark(Baseline = true)]
        public nuint Manual_PopCount_Bytes()
        {
            nuint count = 0;
            for (int i = 0; i < ByteCount; i++)
            {
                int idx = i + ByteOffset;
                count += (nuint)BitOperations.PopCount(buffer![idx]);
            }
            return count;
        }
    }

    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedPopCountUlongBenchmarks
    {
        [Params(1, 25, 1024)]
        public int ElementCount;

        private ulong[]? buffer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            buffer = new ulong[ElementCount + 8];
            for (int i = 0; i < buffer.Length; i++) buffer[i] = (ulong)((uint)i * 0x9E3779B9u + 0xCAFEBABEu);
        }

        [Benchmark]
        public nuint PopCount_Ulongs()
        {
            ref ulong r = ref buffer!.GetReference();
            return Unchecked.PopCount(ref r, (nuint)ElementCount);
        }

        [Benchmark(Baseline = true)]
        public nuint Manual_PopCount_Ulongs()
        {
            nuint count = 0;
            for (int i = 0; i < ElementCount; i++)
            {
                count += (nuint)BitOperations.PopCount(buffer![i]);
            }
            return count;
        }
    }
}