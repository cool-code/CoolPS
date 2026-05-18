using System;
using System.Collections;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Cool;

namespace Cool.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class BitSetBenchmarks
    {
        private const int BitHighLimit = 65535;
        private const int N = 200_000;
        private int[] indexes = null!;
        private BitSet? protoA;
        private BitSet? protoB;
        private BitArray? protoArrayA;
        private BitArray? protoArrayB;

        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random(42);
            indexes = new int[N];
            for (int i = 0; i < N; i++) indexes[i] = rnd.Next(BitHighLimit + 1);

            protoA = new BitSet(BitHighLimit);
            protoB = new BitSet(BitHighLimit);
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
            protoA = null;
            protoB = null;
            protoArrayA = null;
            protoArrayB = null;
        }

        [Benchmark]
        public void Set_BitSet()
        {
            var bs = new BitSet(BitHighLimit);
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
            var bs = new BitSet(BitHighLimit);
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
            var bs = new BitSet(BitHighLimit);
            for (int i = 0; i < indexes.Length; i++) bs.Set((uint)indexes[i]);
            return bs.Cardinality();
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
            var a = protoA!.Clone();
            var b = protoB!.Clone();
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
            var a = protoA!.Clone();
            var b = protoB!.Clone();
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
            var a = protoA!.Clone();
            var b = protoB!.Clone();
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
            var a = protoA!.Clone();
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
