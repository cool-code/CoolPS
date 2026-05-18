using System;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class BitSetTests
    {
        [Fact]
        public void BasicSetClearContains()
        {
            var bs = new BitSet(100);
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
            var bs = new BitSet(255, "0~3,5,7~F");
            string s = bs.ToString();
            Assert.Equal("0~3,5,7~F", s);
        }

        [Fact]
        public void SetAllClearCardinalityCloneEquals()
        {
            var bs = new BitSet(63);
            bs.SetAll();
            Assert.Equal(64, bs.Cardinality());

            var clone = bs.Clone();
            Assert.True(bs.SetEquals(clone));

            bs.Clear();
            Assert.True(bs.IsEmpty());
        }

        [Fact]
        public void SetOperations_Union_Intersect_Difference_Symmetric()
        {
            var a = new BitSet(31);
            var b = new BitSet(31);

            a.Set(1);
            a.Set(3);
            b.Set(3);
            b.Set(4);

            var x = a.Clone();
            x.Union(b);
            Assert.True(x.Contains(1));
            Assert.True(x.Contains(3));
            Assert.True(x.Contains(4));

            x = a.Clone();
            x.Intersect(b);
            Assert.False(x.Contains(1));
            Assert.True(x.Contains(3));

            x = a.Clone();
            x.Difference(b);
            Assert.True(x.Contains(1));
            Assert.False(x.Contains(3));

            x = a.Clone();
            x.SymmetricDifference(b);
            Assert.True(x.Contains(1));
            Assert.False(x.Contains(3));
            Assert.True(x.Contains(4));

        }

        [Fact]
        public void Comparisons_Subset_Superset_SetEquals()
        {
            var a = new BitSet(15);
            var b = new BitSet(15);

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
            var bs = new BitSet(7);
            bs.Set(1);
            bs.Set(3);
            bs.Invert(3);
            Assert.False(bs.Contains(3));
            bs.Invert(3);
            Assert.True(bs.Contains(3));
        }
    }
}
