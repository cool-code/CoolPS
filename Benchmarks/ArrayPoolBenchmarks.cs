using BenchmarkDotNet.Attributes;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    public class ArrayPoolBenchmarks
    {
        [Params(16, 64, 1024, 8192)]
        public int Size;

        [Benchmark(Baseline = true)]
        public void SystemArrayPool_RentReturn()
        {
            var a = System.Buffers.ArrayPool<char>.Shared.Rent(Size);
            System.Buffers.ArrayPool<char>.Shared.Return(a);
        }
        [Benchmark]
        public void ArrayPool_RentReturn()
        {
            var a = Cool.ArrayPool<char>.Shared.Rent(Size);
            Cool.ArrayPool<char>.Shared.Return(a);
        }
    }
}
