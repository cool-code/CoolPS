using System;
using System.Collections.Generic;
using Xunit;
using Cool;
using System.Reflection;

namespace Cool.Tests
{
    public class RangeTests
    {
        [Fact]
        public void EmptyRange_YieldsNothing()
        {
            var r = new Range<int>("", 0xFF);
            var items = new List<int>();
            foreach (int v in r) items.Add(v);
            Assert.Empty(items);
        }

        [Fact]
        public void SingleValue_Hex()
        {
            var r = new Range<int>("A", 0xFF);
            var items = new List<int>();
            foreach (int v in r) items.Add(v);
            Assert.Single(items);
            Assert.Equal(10, items[0]);
        }

        [Fact]
        public void MultipleValues_Hex()
        {
            var r = new Range<int>("1,3~5", 0xFF);
            var items = new List<int>();
            foreach (int v in r) items.Add(v);
            Assert.Equal(new[] { 1, 3, 4, 5 }, items.ToArray());
        }

        [Fact]
        public void InclusiveRange_Hex()
        {
            var r = new Range<int>("1~3", 0xFF);
            var items = new List<int>();
            foreach (int v in r) items.Add(v);
            Assert.Equal(new[] { 1, 2, 3 }, items.ToArray());
        }

        [Fact]
        public void EndTruncatedByHighLimit()
        {
            // end value FF (255) is truncated to highLimit 0xF (15)
            var r = new Range<int>("1~FF", 0xF);
            var items = new List<int>();
            foreach (int v in r) items.Add(v);
            Assert.Equal(15, items.Count); // 1..0xF inclusive
            Assert.Equal(1, items[0]);
        }

        [Fact]
        public void TrailingTilde_NoEnd_YieldsSingle()
        {
            var r = new Range<byte>("5~", 0xFF);
            var items = new List<byte>();
            foreach (byte v in r) items.Add(v);
            Assert.Single(items);
            Assert.Equal(5, items[0]);
        }

        [Fact]
        public void NegativeAndPlusParsing()
        {
            var r = new Range<int>("-A,+3", 0xFF);
            var items = new List<int>();
            foreach (int v in r) items.Add(v);
            Assert.Equal(new[] { -10, 3 }, items.ToArray());
        }
#pragma warning disable CS8625

        [Fact]
        public void ToStringTest()
        {
            var r = new Range<int>("1~3,5,7~F", 0xFF);
            var s = r.ToString();
            Assert.Equal("1~3,5,7~F", s);
            r = new Range<int>(null, 0xFF);
            var items = new List<int>();
            foreach (int v in r) items.Add(v);
            Assert.Empty(items);
            s = r.ToString();
            Assert.Null(s);
        }
#pragma warning restore CS8625

    }
}

