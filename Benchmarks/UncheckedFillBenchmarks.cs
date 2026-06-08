using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedFillByteBenchmarks
    {
        [Params(8, 200, 8192, 1024 * 1024 * 800)]
        public int ByteCount;

        [Params(0, 1)]
        public int ByteOffset;

        private byte[]? buffer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            buffer = new byte[ByteCount + ByteOffset];
        }

        [Benchmark]
        public void Fill_Bytes()
        {
            byte fill = 0xAB;
            ref byte r = ref Unsafe.Add(ref buffer!.GetReference(), ByteOffset);
            Unchecked.Fill(ref r, (nuint)ByteCount, in fill);
        }

        [Benchmark]
        public void Array_Fill_Bytes()
        {
            byte fill = 0xAB;
            Array.Fill(buffer!, fill);
        }

        [Benchmark(Baseline = true)]
        public void Manual_Fill_Bytes()
        {
            byte fill = 0xAB;
            for (int i = 0; i < ByteCount; i++) buffer![i + ByteOffset] = fill;
        }
    }

    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedFillUIntBenchmarks
    {
        [Params(1, 25, 1024, 1024 * 1024 * 100)]
        public int ElementCount;

        private uint[]? buffer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            buffer = new uint[ElementCount];
        }

        [Benchmark]
        public void Fill_UInts()
        {
            uint value = 0x12345678u;
            ref uint r = ref buffer!.GetReference();
            Unchecked.Fill(ref r, (nuint)ElementCount, in value);
        }

        [Benchmark]
        public void Array_Fill_UInts()
        {
            uint value = 0x12345678u;
            Array.Fill(buffer!, value);
        }

        [Benchmark(Baseline = true)]
        public void Manual_Fill_UInts()
        {
            uint value = 0x12345678u;
            for (int i = 0; i < ElementCount; i++) buffer![i] = value;
        }
    }

    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedFillUlongBenchmarks
    {
        [Params(1, 25, 1024, 1024 * 1024 * 100)]
        public int ElementCount;

        private ulong[]? buffer;

        [GlobalSetup]
        public void GlobalSetup()
        {
            buffer = new ulong[ElementCount];
        }

        [Benchmark]
        public void Fill_Ulongs()
        {
            ulong value = 0xCAFEBABECAFEBABEu;
            ref ulong r = ref buffer!.GetReference();
            Unchecked.Fill(ref r, (nuint)ElementCount, in value);
        }

        [Benchmark]
        public void Array_Fill_Ulongs()
        {
            ulong value = 0xCAFEBABECAFEBABEu;
            Array.Fill(buffer!, value);
        }

        [Benchmark(Baseline = true)]
        public void Manual_Fill_Ulongs()
        {
            ulong value = 0xCAFEBABECAFEBABEu;
            for (int i = 0; i < ElementCount; i++) buffer![i] = value;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ThreeBytes
    {
        public byte A;
        public byte B;
        public byte C;
    }

    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 5, printSource: true)]
    public class UncheckedFillStructBenchmarks
    {
        [Params(1, 128, 4096, 1024 * 1024 * 100)]
        public int ElementCount;

        private ThreeBytes[]? buffer;
        private ThreeBytes value = new() { A = 1, B = 2, C = 3 };

        [GlobalSetup]
        public void GlobalSetup()
        {
            buffer = new ThreeBytes[ElementCount];
        }

        [Benchmark(Baseline = true)]
        public void Manual_Fill_ThreeBytes()
        {
            for (int i = 0; i < ElementCount; i++) buffer![i] = value;
        }

        [Benchmark]
        public void Fill_ThreeBytes()
        {
            Unchecked.Fill(ref buffer!.GetReference(), (nuint)ElementCount, in value);
        }

        [Benchmark]
        public void Array_Fill_ThreeBytes()
        {
            Array.Fill(buffer!, value);
        }
    }
}
