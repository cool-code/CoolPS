using System;
using Xunit;
using Cool;

namespace Cool.Tests
{
    public class UncheckedArrayTests
    {
        [Fact]
        public void Array_Conversions_Indexer_Span_Enumerator_And_Properties()
        {
            int[] arr = [1, 2, 3, 4];
            Unchecked.Array<int> ua = arr;

            Assert.Equal(arr.Rank, ua.Rank);
            Assert.Equal(arr.Length, ua.Length);
            Assert.Equal(arr.LongLength, ua.LongLength);
            Assert.Equal(arr.IsFixedSize, ua.IsFixedSize);
            Assert.Equal(arr.IsReadOnly, ua.IsReadOnly);
            Assert.Equal(arr.IsSynchronized, ua.IsSynchronized);
            Assert.Same(arr.SyncRoot, ua.SyncRoot);

            // ToArray returns the original array reference
            Assert.Same(arr, ua.ToArray());

            Assert.Equal(arr.GetLength(0), ua.GetLength(0));
            Assert.Equal(arr.GetLowerBound(0), ua.GetLowerBound(0));
            Assert.Equal(arr.GetUpperBound(0), ua.GetUpperBound(0));

            var span = ua.AsSpan();
            Assert.Equal(arr.Length, span.Length);
            for (int i = 0; i < arr.Length; i++) Assert.Equal(arr[i], span[i]);

            // indexer read (uint and int)
            for (uint i = 0; i < (uint)arr.Length; i++) Assert.Equal(arr[i], ua[i]);
            for (int i = 0; i < arr.Length; i++) Assert.Equal(arr[i], ua[i]);
            foreach (int x in ua) Assert.Contains(x, arr);

            // indexer write modifies underlying array
            ua[0] = 10;
            ua[1] = 20;
            Assert.Equal(10, arr[0]);
            Assert.Equal(20, arr[1]);

            // enumerator returns ref and can mutate elements
            foreach (ref int x in ua) x += 1000;
            Assert.Equal(new[] { 1010, 1020, 1003, 1004 }, arr);

            // invalid dimension should throw
            Assert.Throws<IndexOutOfRangeException>(() => ua.GetLength(1));
            Assert.Throws<IndexOutOfRangeException>(() => ua.GetLowerBound(1));
        }

    }
}
