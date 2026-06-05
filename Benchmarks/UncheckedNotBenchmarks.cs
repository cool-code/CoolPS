using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedNotByteBenchmarks
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
        public void Not_Bytes()
        {
            ref byte r = ref Unsafe.Add(ref buffer!.GetReference(), ByteOffset);
            Unchecked.Not(ref r, (nuint)ByteCount);
            Unchecked.Not(ref r, (nuint)ByteCount);
        }

        [Benchmark(Baseline = true)]
        public void Manual_Not_Bytes()
        {
            for (int i = 0; i < ByteCount; i++)
            {
                int idx = i + ByteOffset;
                buffer![idx] = (byte)~buffer[idx];
            }
            for (int i = 0; i < ByteCount; i++)
            {
                int idx = i + ByteOffset;
                buffer![idx] = (byte)~buffer[idx];
            }
        }
    }

    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedNotUlongBenchmarks
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
        public void Not_Ulongs()
        {
            ref ulong r = ref buffer!.GetReference();
            Unchecked.Not(ref r, (nuint)ElementCount);
            Unchecked.Not(ref r, (nuint)ElementCount);
        }

        [Benchmark(Baseline = true)]
        public void Manual_Not_Ulongs()
        {
            for (int i = 0; i < ElementCount; i++) buffer![i] = ~buffer![i];
            for (int i = 0; i < ElementCount; i++) buffer![i] = ~buffer![i];
        }
    }
}