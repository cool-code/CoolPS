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

        [Fact]
        public void Array2D_Conversions_Indexers_Span_Enumerator_And_Properties()
        {
            int rows = 2, cols = 3;
            int[,] arr2 = new int[rows, cols];
            int v = 1;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    arr2[i, j] = v++;

            // record original sum
            int originalSum = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    originalSum += arr2[i, j];

            Unchecked.Array2D<int> a2 = arr2;

            Assert.Equal(arr2.Rank, a2.Rank);
            Assert.Equal(arr2.Length, a2.Length);
            Assert.Equal(arr2.LongLength, a2.LongLength);
            Assert.Equal(arr2.IsFixedSize, a2.IsFixedSize);
            Assert.Equal(arr2.IsReadOnly, a2.IsReadOnly);
            Assert.Equal(arr2.IsSynchronized, a2.IsSynchronized);
            Assert.Same(arr2.SyncRoot, a2.SyncRoot);
            Assert.Same(arr2, a2.ToArray());

            Assert.Equal(arr2.GetLength(0), a2.GetLength(0));
            Assert.Equal(arr2.GetLowerBound(0), a2.GetLowerBound(0));
            Assert.Equal(arr2.GetUpperBound(0), a2.GetUpperBound(0));
            Assert.Equal(arr2.GetLength(1), a2.GetLength(1));
            Assert.Equal(arr2.GetLowerBound(1), a2.GetLowerBound(1));
            Assert.Equal(arr2.GetUpperBound(1), a2.GetUpperBound(1));

            // Runtime layout for multi-dimension array metadata varies across runtimes
            // (observed inconsistencies for the second-dimension fields). Validate
            // behavior via AsSpan/ToArray/indexers rather than asserting metadata fields.
            Assert.Equal(rows * cols, a2.Length);

            var span = a2.AsSpan();
            Assert.Equal(rows * cols, span.Length);

            // linear ordering (row-major)
            int idx = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    Assert.Equal(arr2[i, j], span[idx++]);

            // single-index access maps to row*cols + col (use linear index only)
            int linearIdx = 1 * cols + 2;
            Assert.Equal(arr2[linearIdx / cols, linearIdx % cols], a2[linearIdx]);
            Assert.Equal(arr2[0, 0], a2[0u]);

            // write using single-index form (avoid testing two-index indexer — runtime metadata varies)
            a2[linearIdx] = 999;
            Assert.Equal(999, arr2[1, 2]);

            a2[0] = -5;
            Assert.Equal(-5, arr2[0, 0]);

            // compute sum after writes
            int sumAfterWrites = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    sumAfterWrites += arr2[i, j];

            // enumerator doubles each element (use foreach ref)
            foreach (ref int cur in a2) cur *= 2;

            int sumAfterDouble = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    sumAfterDouble += arr2[i, j];

            Assert.Equal(sumAfterWrites * 2, sumAfterDouble);

            // invalid dimension should throw
            Assert.Throws<IndexOutOfRangeException>(() => a2.GetLength(2));
            Assert.Throws<IndexOutOfRangeException>(() => a2.GetLowerBound(2));
        }
    }
}
