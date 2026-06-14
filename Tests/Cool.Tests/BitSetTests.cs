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

        const string wideRange = "1100~115F,231A~231B,2329~232A,23E9~23EC,23F0,23F3,25FD~25FE,2614~2615,2630~2637,2648~2653,267F,268A~268F,2693,26A1,26AA~26AB,26BD~26BE,26C4~26C5,26CE,26D4,26EA,26F2~26F3,26F5,26FA,26FD,2705,270A~270B,2728,274C,274E,2753~2755,2757,2795~2797,27B0,27BF,2B1B~2B1C,2B50,2B55,2E80~2E99,2E9B~2EF3,2F00~2FD5,2FF0~303E,3041~3096,3099~30FF,3105~312F,3131~318E,3190~31E5,31EF~321E,3220~3247,3250~A48C,A490~A4C6,A960~A97C,AC00~D7A3,F900~FAFF,FE10~FE19,FE30~FE52,FE54~FE66,FE68~FE6B,FF01~FF60,FFE0~FFE6,16FE0~16FE4,16FF0~16FF6,17000~18CD5,18CFF~18D1E,18D80~18DF2,1AFF0~1AFF3,1AFF5~1AFFB,1AFFD~1AFFE,1B000~1B122,1B132,1B150~1B152,1B155,1B164~1B167,1B170~1B2FB,1D300~1D356,1D360~1D376,1F004,1F0CF,1F18E,1F191~1F19A,1F200~1F202,1F210~1F23B,1F240~1F248,1F250~1F251,1F260~1F265,1F300~1F320,1F32D~1F335,1F337~1F37C,1F37E~1F393,1F3A0~1F3CA,1F3CF~1F3D3,1F3E0~1F3F0,1F3F4,1F3F8~1F43E,1F440,1F442~1F4FC,1F4FF~1F53D,1F54B~1F54E,1F550~1F567,1F57A,1F595~1F596,1F5A4,1F5FB~1F64F,1F680~1F6C5,1F6CC,1F6D0~1F6D2,1F6D5~1F6D8,1F6DC~1F6DF,1F6EB~1F6EC,1F6F4~1F6FC,1F7E0~1F7EB,1F7F0,1F90C~1F93A,1F93C~1F945,1F947~1F9FF,1FA70~1FA7C,1FA80~1FA8A,1FA8E~1FAC6,1FAC8,1FACD~1FADC,1FADF~1FAEA,1FAEF~1FAF8";

        [Fact]
        public void SetRangeAndToString()
        {
            using var bs = new BitSet<Native>(18, "0~3,5,7~F");
            string s = bs.ToString();
            Assert.Equal("0~3,5,7~F", s);
            using var bs2 = new BitSet<Fixed16KB>(0x1FFFF, wideRange);
            s = bs2.ToString();
            Assert.Equal(wideRange, s);
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
