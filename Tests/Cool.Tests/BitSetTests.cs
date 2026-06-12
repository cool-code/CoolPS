using System;
using Xunit;
using Cool;
using static Cool.BitSet.Allocator;

namespace Cool.Tests
{
    public class BitSetTests
    {
        [Fact]
        public void BasicSetClearContains()
        {
            var bs = new BitSet<Fixed32B>(100);
            Assert.True(bs.IsEmpty());

            bs.Set(1);
            bs.Set(50);
            bs.Set(100);

            Assert.True(bs.Contains(1));
            Assert.True(bs.Contains(50));
            Assert.True(bs.Contains(100));

            bs.Clear(50);
            Assert.False(bs.Contains(50));
        }

        [Fact]
        public void SetRangeAndToString()
        {
            using var bs = new BitSet<Native>(18, "0~3,5,7~F");
            string s = bs.ToString();
            Assert.Equal("0~3,5,7~F", s);
        }

        [Fact]
        public void SetAllClearCardinalityCloneEquals()
        {
            using var bs = new BitSet<Pooled>(63);
            bs.SetAll();
            Assert.Equal(64u, bs.Cardinality());

            using var clone = bs.Clone();
            Assert.True(bs.SetEquals(clone));

            bs.Clear();
            Assert.True(bs.IsEmpty());
        }

        [Fact]
        public void SetOperations_Union_Intersect_Difference_Symmetric()
        {
            using var a = new BitSet<Native>(31);
            using var b = new BitSet<Native>(31);

            a.Set(1);
            a.Set(3);
            b.Set(3);
            b.Set(4);

            var x = a.Clone<Fixed8B>();
            x.Union(b);
            Assert.True(x.Contains(1));
            Assert.True(x.Contains(3));
            Assert.True(x.Contains(4));

            x = a.Clone<Fixed8B>();
            x.Intersect(b);
            Assert.False(x.Contains(1));
            Assert.True(x.Contains(3));

            x = a.Clone<Fixed8B>();
            x.Difference(b);
            Assert.True(x.Contains(1));
            Assert.False(x.Contains(3));

            x = a.Clone<Fixed8B>();
            x.SymmetricDifference(b);
            Assert.True(x.Contains(1));
            Assert.False(x.Contains(3));
            Assert.True(x.Contains(4));

        }

        [Fact]
        public void Comparisons_Subset_Superset_SetEquals()
        {
            using var a = new BitSet<Native>(15);
            using var b = new BitSet<Pooled>(15);

            a.Set(1);
            a.Set(2);
            b.Set(1);
            b.Set(2);

            Assert.True(a.SetEquals(b));
            Assert.True(a == b);

            b.Set(3);
            Assert.True(a.IsSubsetOf(b));
            Assert.True(b.IsSupersetOf(a));
            Assert.True(a < b || a <= b);
        }

        [Fact]
        public void Invert_TogglesBits()
        {
            using var bs = new BitSet<Native>(7);
            bs.Set(1);
            bs.Set(3);
            bs.Invert(3);
            Assert.False(bs.Contains(3));
            bs.Invert(3);
            Assert.True(bs.Contains(3));
        }
    }
}
