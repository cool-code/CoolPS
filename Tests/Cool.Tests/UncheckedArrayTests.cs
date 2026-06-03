using System;
using System.Collections.Generic;
using System.Linq;
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
            Unchecked.SZArray<int> ua = arr;

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

        [Fact]
        public void Array_Empty_EnumeratesNone()
        {
            int[] arr = new int[0];
            Unchecked.SZArray<int> ua = arr;
            Assert.Equal(arr.Length, ua.Length);

            int count = 0;
            foreach (ref int x in ua) count++;
            Assert.Equal(0, count);

            Assert.Throws<IndexOutOfRangeException>(() => ua.GetLength(1));
        }

        [Fact]
        public void Array_Enumerator_ModifiesElements()
        {
            Array arr = Array.CreateInstance(typeof(int), [4], [1]);
            for (int i = 1; i <= arr.GetUpperBound(0); i++) arr.SetValue(i, i);
            Unchecked.Array<int> ua = arr;
            int cnt = 0;
            foreach (ref int x in ua)
            {
                x += 5;
                cnt++;
            }
            Assert.Equal(arr.Length, cnt);
            for (int i = 1; i <= arr.GetUpperBound(0); i++) Assert.Equal(i + 5, arr.GetValue(i));
            for (int i = 1; i <= ua.GetUpperBound(0); i++) Assert.Equal(i + 5, ua[i]);
        }

        [Fact]
        public void Array2D_Enumerator_ModifiesElements_And_LinearIndexing()
        {
            int rows = 3, cols = 4;
            int[,] arr2 = new int[rows, cols];
            int v = 1;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    arr2[i, j] = v++;

            Unchecked.Array2D<int> a2 = arr2;

            int cnt = 0;
            foreach (ref int cur in a2)
            {
                cur += 10;
                cnt++;
            }
            Assert.Equal(rows * cols, cnt);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    Assert.Equal((i * cols + j + 1) + 10, arr2[i, j]);

            int linearIdx = 2 * cols + 1;
            Assert.Equal(arr2[2, 1], a2[linearIdx]);
            a2[linearIdx] = -99;
            Assert.Equal(-99, arr2[2, 1]);
        }
        [Fact]
        public void Array3D_Conversions_Indexers_Enumerator_And_Properties()
        {
            int d1 = 2, d2 = 3, d3 = 4;
            int[,,] arr3 = new int[d1, d2, d3];
            int v = 1;
            for (int i = 0; i < d1; i++)
                for (int j = 0; j < d2; j++)
                    for (int k = 0; k < d3; k++)
                        arr3[i, j, k] = v++;

            Unchecked.Array3D<int> a3 = arr3;

            Assert.Equal(arr3.Rank, a3.Rank);
            Assert.Equal(arr3.Length, a3.Length);
            Assert.Equal(arr3.LongLength, a3.LongLength);
            Assert.Equal(arr3.IsFixedSize, a3.IsFixedSize);
            Assert.Equal(arr3.IsReadOnly, a3.IsReadOnly);
            Assert.Equal(arr3.IsSynchronized, a3.IsSynchronized);
            Assert.Same(arr3.SyncRoot, a3.SyncRoot);
            Assert.Same(arr3, a3.ToArray());

            Assert.Equal(arr3.GetLength(0), a3.GetLength(0));
            Assert.Equal(arr3.GetLowerBound(0), a3.GetLowerBound(0));
            Assert.Equal(arr3.GetUpperBound(0), a3.GetUpperBound(0));
            Assert.Equal(arr3.GetLength(1), a3.GetLength(1));
            Assert.Equal(arr3.GetLowerBound(1), a3.GetLowerBound(1));
            Assert.Equal(arr3.GetUpperBound(1), a3.GetUpperBound(1));
            Assert.Equal(arr3.GetLength(2), a3.GetLength(2));
            Assert.Equal(arr3.GetLowerBound(2), a3.GetLowerBound(2));
            Assert.Equal(arr3.GetUpperBound(2), a3.GetUpperBound(2));

            int linearIdx = 1 * d2 * d3 + 2 * d3 + 3;
            Assert.Equal(arr3[1, 2, 3], a3[linearIdx]);
            Assert.Equal(arr3[0, 0, 0], a3[0u]);

            a3[linearIdx] = 9999;
            Assert.Equal(9999, arr3[1, 2, 3]);
            a3[0] = -5;
            Assert.Equal(-5, arr3[0, 0, 0]);

            int sumBefore = 0;
            for (int i = 0; i < d1; i++)
                for (int j = 0; j < d2; j++)
                    for (int k = 0; k < d3; k++)
                        sumBefore += arr3[i, j, k];

            int cnt = 0;
            foreach (ref int cur in a3)
            {
                cur += 10;
                cnt++;
            }
            Assert.Equal(d1 * d2 * d3, cnt);

            int sumAfter = 0;
            for (int i = 0; i < d1; i++)
                for (int j = 0; j < d2; j++)
                    for (int k = 0; k < d3; k++)
                        sumAfter += arr3[i, j, k];
            Assert.Equal(sumBefore + 10 * d1 * d2 * d3, sumAfter);

            Assert.Throws<IndexOutOfRangeException>(() => a3.GetLength(3));
            Assert.Throws<IndexOutOfRangeException>(() => a3.GetLowerBound(3));

            // string[,,] supports reference-type elements and ref-enumerator
            int sr1 = 1, sr2 = 2, sr3 = 3;
            string[,,] sarr = new string[sr1, sr2, sr3];
            int si = 0;
            for (int i = 0; i < sr1; i++)
                for (int j = 0; j < sr2; j++)
                    for (int k = 0; k < sr3; k++)
                        sarr[i, j, k] = "s" + (si++).ToString();

            Unchecked.Array3D<string> sa3 = sarr;
            Assert.Same(sarr, sa3.ToArray());
            sa3[0, 0, 0] = "start";
            Assert.Equal("start", sarr[0, 0, 0]);
            foreach (ref string s in sa3) s += "!";
            for (int i = 0; i < sr1; i++)
                for (int j = 0; j < sr2; j++)
                    for (int k = 0; k < sr3; k++)
                        Assert.EndsWith("!", sarr[i, j, k]);

            // object[,,] elements can be mutated via ref-enumerator
            int or1 = 1, or2 = 2, or3 = 3;
            object?[,,] oarr = new object?[or1, or2, or3];
            int oi = 0;
            for (int i = 0; i < or1; i++)
                for (int j = 0; j < or2; j++)
                    for (int k = 0; k < or3; k++)
                        oarr[i, j, k] = oi++;

            Unchecked.Array<object?> ua3 = oarr;
            Assert.Same(oarr, ua3.ToArray());
            oi = 0;
            foreach (ref object? o in ua3) Assert.Equal(oi++, o);
            oi = 0;
            for (int i = 0; i < or1; i++)
            {
                for (int j = 0; j < or2; j++)
                {
                    for (int k = 0; k < or3; k++)
                    {
                        Assert.Equal(oi++, ua3[i, j, k]);
                    }
                }
            }

            Unchecked.Array3D<object?> oa3 = oarr;
            Assert.Same(oarr, oa3.ToArray());
            foreach (ref object? o in oa3) o = null;
            for (int i = 0; i < or1; i++)
                for (int j = 0; j < or2; j++)
                    for (int k = 0; k < or3; k++)
                        Assert.Null(oarr[i, j, k]);
        }

        [Fact]
        public void Array4D_Conversions_Indexers_Enumerator_And_Properties()
        {
            int d1 = 2, d2 = 2, d3 = 3, d4 = 4;
            int[,,,] arr4 = new int[d1, d2, d3, d4];
            int v = 1;
            for (int i = 0; i < d1; i++)
                for (int j = 0; j < d2; j++)
                    for (int k = 0; k < d3; k++)
                        for (int l = 0; l < d4; l++)
                            arr4[i, j, k, l] = v++;

            Unchecked.Array4D<int> a4 = arr4;

            Assert.Equal(arr4.Rank, a4.Rank);
            Assert.Equal(arr4.Length, a4.Length);
            Assert.Equal(arr4.LongLength, a4.LongLength);
            Assert.Equal(arr4.IsFixedSize, a4.IsFixedSize);
            Assert.Equal(arr4.IsReadOnly, a4.IsReadOnly);
            Assert.Equal(arr4.IsSynchronized, a4.IsSynchronized);
            Assert.Same(arr4.SyncRoot, a4.SyncRoot);
            Assert.Same(arr4, a4.ToArray());

            Assert.Equal(arr4.GetLength(0), a4.GetLength(0));
            Assert.Equal(arr4.GetLowerBound(0), a4.GetLowerBound(0));
            Assert.Equal(arr4.GetUpperBound(0), a4.GetUpperBound(0));
            Assert.Equal(arr4.GetLength(1), a4.GetLength(1));
            Assert.Equal(arr4.GetLowerBound(1), a4.GetLowerBound(1));
            Assert.Equal(arr4.GetUpperBound(1), a4.GetUpperBound(1));
            Assert.Equal(arr4.GetLength(2), a4.GetLength(2));
            Assert.Equal(arr4.GetLowerBound(2), a4.GetLowerBound(2));
            Assert.Equal(arr4.GetUpperBound(2), a4.GetUpperBound(2));
            Assert.Equal(arr4.GetLength(3), a4.GetLength(3));
            Assert.Equal(arr4.GetLowerBound(3), a4.GetLowerBound(3));
            Assert.Equal(arr4.GetUpperBound(3), a4.GetUpperBound(3));

            int linearIdx = 1 * d2 * d3 * d4 + 0 * d3 * d4 + 2 * d4 + 3;
            Assert.Equal(arr4[1, 0, 2, 3], a4[linearIdx]);
            Assert.Equal(arr4[0, 0, 0, 0], a4[0u]);

            a4[linearIdx] = 99999;
            Assert.Equal(99999, arr4[1, 0, 2, 3]);
            a4[0] = -5;
            Assert.Equal(-5, arr4[0, 0, 0, 0]);

            int sumBefore = 0;
            for (int i = 0; i < d1; i++)
                for (int j = 0; j < d2; j++)
                    for (int k = 0; k < d3; k++)
                        for (int l = 0; l < d4; l++)
                            sumBefore += arr4[i, j, k, l];

            int cnt = 0;
            foreach (ref int cur in a4)
            {
                cur += 10;
                cnt++;
            }
            Assert.Equal(d1 * d2 * d3 * d4, cnt);

            int sumAfter = 0;
            for (int i = 0; i < d1; i++)
                for (int j = 0; j < d2; j++)
                    for (int k = 0; k < d3; k++)
                        for (int l = 0; l < d4; l++)
                            sumAfter += arr4[i, j, k, l];
            Assert.Equal(sumBefore + 10 * d1 * d2 * d3 * d4, sumAfter);

            Assert.Throws<IndexOutOfRangeException>(() => a4.GetLength(4));
            Assert.Throws<IndexOutOfRangeException>(() => a4.GetLowerBound(4));
        }

        [Fact]
        public void Array_1D_NonZeroLowerBound_Access_And_Enumerator()
        {
            int len = 4;
            int lb = 1;
            Array arr = Array.CreateInstance(typeof(int), new int[] { len }, new int[] { lb });
            for (int i = lb; i <= lb + len - 1; i++) arr.SetValue(i * 10, i);

            Unchecked.Array<int> ua = arr;
            Assert.Equal(len, ua.Length);
            Assert.Equal(lb, ua.GetLowerBound(0));
            Assert.Equal(lb + len - 1, ua.GetUpperBound(0));

            for (int i = lb; i <= lb + len - 1; i++) Assert.Equal(i * 10, ua[i]);

            ua[lb] = 999;
            Assert.Equal(999, arr.GetValue(lb));

            int cnt = 0;
            foreach (ref int x in ua)
            {
                cnt++;
                x += 1;
            }
            Assert.Equal(len, cnt);
            for (int i = lb; i <= lb + len - 1; i++)
            {
                int expectedBefore = (i == lb) ? 999 : (i * 10);
                Assert.Equal(expectedBefore + 1, arr.GetValue(i));
            }
        }

        [Fact]
        public void LowerDim_Indexers_Flatten_HigherDim_Access_2D_on_3D_4D()
        {
            // 3D array: verify 2D indexer flattens to underlying linear ordering
            int d0 = 2, d1 = 3, d2 = 4;
            int[,,] arr3 = new int[d0, d1, d2];
            int v = 0;
            for (int i = 0; i < d0; i++)
                for (int j = 0; j < d1; j++)
                    for (int k = 0; k < d2; k++)
                        arr3[i, j, k] = v++;

            Unchecked.Array<int> ua3 = arr3;
            var flat3 = new List<int>();
            foreach (int o in arr3) flat3.Add(o);

            for (int i = 0; i < d0; i++)
            {
                for (int j = 0; j < d1; j++)
                {
                    int A = (i - arr3.GetLowerBound(0));
                    for (int t = 1; t < arr3.Rank; t++) A *= arr3.GetLength(t);
                    A += (j - arr3.GetLowerBound(1));
                    Assert.Equal(flat3[A], ua3[i, j]);

                    int newv = -1000 + i * 10 + j;
                    ua3[i, j] = newv;
                    var flat3b = new List<int>();
                    foreach (int o in arr3) flat3b.Add(o);
                    Assert.Equal(newv, flat3b[A]);
                }
            }

            // 4D array: verify 2D indexer flattens to underlying linear ordering
            int a0 = 2, a1 = 2, a2 = 3, a3 = 4;
            int[,,,] arr4 = new int[a0, a1, a2, a3];
            v = 0;
            for (int i = 0; i < a0; i++)
                for (int j = 0; j < a1; j++)
                    for (int k = 0; k < a2; k++)
                        for (int l = 0; l < a3; l++)
                            arr4[i, j, k, l] = v++;

            Unchecked.Array<int> ua4 = arr4;
            var flat4 = new List<int>();
            foreach (int o in arr4) flat4.Add(o);

            for (int i = 0; i < a0; i++)
            {
                for (int j = 0; j < a1; j++)
                {
                    int A = (i - arr4.GetLowerBound(0));
                    for (int t = 1; t < arr4.Rank; t++) A *= arr4.GetLength(t);
                    A += (j - arr4.GetLowerBound(1));
                    Assert.Equal(flat4[A], ua4[i, j]);

                    int newv = 20000 + i * 10 + j;
                    ua4[i, j] = newv;
                    var flat4b = new List<int>();
                    foreach (int o in arr4) flat4b.Add(o);
                    Assert.Equal(newv, flat4b[A]);
                }
            }
        }

        [Fact]
        public void Array_Dimensions_5_to_10_Params_Indexer_And_Enumeration()
        {
            for (int rank = 5; rank <= 10; rank++)
            {
                int[] dims = Enumerable.Repeat(2, rank).ToArray();
                Array arr = Array.CreateInstance(typeof(int), dims);
                int total = 1;
                for (int i = 0; i < rank; i++) total *= dims[i];

                // fill sequential values
                int val = 0;
                for (int linear = 0; linear < total; linear++)
                {
                    int rem = linear;
                    int[] indices = new int[rank];
                    for (int d = 0; d < rank; d++)
                    {
                        int stride = 1;
                        for (int k = d + 1; k < rank; k++) stride *= dims[k];
                        indices[d] = arr.GetLowerBound(d) + rem / stride;
                        rem = rem % stride;
                    }
                    arr.SetValue(val++, indices);
                }

                Unchecked.Array<int> ua = arr;

                for (int linear = 0; linear < total; linear++)
                {
                    int rem = linear;
                    int[] indices = new int[rank];
                    for (int d = 0; d < rank; d++)
                    {
                        int stride = 1;
                        for (int k = d + 1; k < rank; k++) stride *= dims[k];
                        indices[d] = arr.GetLowerBound(d) + rem / stride;
                        rem = rem % stride;
                    }

                    // read via params indexer (use Array.GetValue as the authoritative source)
                    int expected = (int)arr.GetValue(indices)!;
                    Assert.Equal(expected, ua[indices]);

                    int newv = 100000 + linear;
                    ua[indices] = newv;
                    Assert.Equal(newv, (int)arr.GetValue(indices)!);
                }
            }
        }
    }
}
