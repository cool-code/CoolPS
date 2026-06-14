using System;
using System.Collections;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;
using static Cool.BitSet.Allocator;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class BitSetBenchmarks
    {
        private const int BitHighLimit = 65535;
        private const int N = 200_000;
        private int[] indexes = null!;
        private BitSet<Native>? protoA;
        private BitSet<Native>? protoB;
        private BitArray? protoArrayA;
        private BitArray? protoArrayB;

        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random(42);
            indexes = new int[N];
            for (int i = 0; i < N; i++) indexes[i] = rnd.Next(BitHighLimit + 1);

            protoA = new BitSet<Native>(BitHighLimit);
            protoB = new BitSet<Native>(BitHighLimit);
            protoArrayA = new BitArray(BitHighLimit + 1);
            protoArrayB = new BitArray(BitHighLimit + 1);

            for (int i = 0; i <= BitHighLimit; i += 3)
            {
                protoA.Set((uint)i);
                protoArrayA[i] = true;
            }

            for (int i = 0; i <= BitHighLimit; i += 5)
            {
                protoB.Set((uint)i);
                protoArrayB[i] = true;
            }
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            protoA!.Dispose();
            protoB!.Dispose();
            protoArrayA = null;
            protoArrayB = null;
        }

        [Benchmark]
        public void Set_BitSet()
        {
            using var bs = new BitSet<Pooled>(BitHighLimit);
            for (int i = 0; i < indexes.Length; i++) bs.Set((uint)indexes[i]);
        }

        [Benchmark]
        public void Set_BitArray()
        {
            var ba = new BitArray(BitHighLimit + 1);
            for (int i = 0; i < indexes.Length; i++) ba.Set(indexes[i], true);
        }

        [Benchmark]
        public int Contains_BitSet()
        {
            using var bs = new BitSet<Pooled>(BitHighLimit);
            for (int i = 0; i < indexes.Length; i++) bs.Set((uint)indexes[i]);
            int sum = 0;
            for (int i = 0; i < indexes.Length; i++) if (bs.Contains((uint)indexes[i])) sum++;
            return sum;
        }

        [Benchmark]
        public int Contains_BitArray()
        {
            var ba = new BitArray(BitHighLimit + 1);
            for (int i = 0; i < indexes.Length; i++) ba.Set(indexes[i], true);
            int sum = 0;
            for (int i = 0; i < indexes.Length; i++) if (ba[indexes[i]]) sum++;
            return sum;
        }

        [Benchmark]
        public int Cardinality_BitSet()
        {
            using var bs = new BitSet<Pooled>(BitHighLimit);
            for (int i = 0; i < indexes.Length; i++) bs.Set((uint)indexes[i]);
            return (int)bs.Cardinality();
        }

        [Benchmark]
        public int Cardinality_BitArray()
        {
            var ba = new BitArray(BitHighLimit + 1);
            for (int i = 0; i < indexes.Length; i++) ba.Set(indexes[i], true);
            int sum = 0;
            for (int i = 0; i < ba.Length; i++) if (ba[i]) sum++;
            return sum;
        }

        [Benchmark]
        public void Union_BitSet()
        {
            using var a = protoA!.Clone<Pooled>();
            using var b = protoB!.Clone<Pooled>();
            a.Union(b);
        }

        [Benchmark]
        public void Union_BitArray()
        {
            var a = (BitArray)protoArrayA!.Clone();
            var b = (BitArray)protoArrayB!.Clone();
            a.Or(b);
        }

        [Benchmark]
        public void Intersect_BitSet()
        {
            using var a = protoA!.Clone<Pooled>();
            using var b = protoB!.Clone<Pooled>();
            a.Intersect(b);
        }

        [Benchmark]
        public void Intersect_BitArray()
        {
            var a = (BitArray)protoArrayA!.Clone();
            var b = (BitArray)protoArrayB!.Clone();
            a.And(b);
        }

        [Benchmark]
        public void Symmetric_BitSet()
        {
            using var a = protoA!.Clone<Pooled>();
            using var b = protoB!.Clone<Pooled>();
            a.SymmetricDifference(b);
        }

        [Benchmark]
        public void Symmetric_BitArray()
        {
            var a = (BitArray)protoArrayA!.Clone();
            var b = (BitArray)protoArrayB!.Clone();
            a.Xor(b);
        }

        [Benchmark]
        public void Invert_BitSet()
        {
            using var a = protoA!.Clone();
            a.Invert();
        }

        [Benchmark]
        public void Invert_BitArray()
        {
            var a = (BitArray)protoArrayA!.Clone();
            a.Not();
        }
    }
}
