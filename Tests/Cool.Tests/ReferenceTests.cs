using System;
using Xunit;
using Cool;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool.Tests
{
#pragma warning disable CS8604, CS8625, CS8601, CS8602
    public class ReferenceTests
    {
        [Fact]
        public void String_GetPinnableReference()
        {
            string str = "Hello, World!";
            ref readonly char c = ref str.GetPinnableReference();
            Assert.Equal('H', c);
        }

        [Fact]
        public void NullString_GetPinnableReference()
        {
            string? str = null;
            Assert.Throws<NullReferenceException>(() => { ref readonly char c = ref str.GetPinnableReference(); });
        }

        [Fact]
        public void EmptyString_GetPinnableReference()
        {
            string str = "";
            ref readonly char c = ref str.GetPinnableReference();
            Assert.Equal('\0', c); // GetPinnableReference on empty string returns reference to null terminator
        }

        [Fact]
        public void Array_GetArrayDataReference()
        {
            int[] arr = [1, 2, 3, 4, 5];
            ref int r = ref arr.GetReference();
            Assert.Equal(1, r);
        }

        [Fact]
        public void EmptyArray_GetArrayDataReference()
        {
            int[] arr = [];
            ref int r = ref arr.GetReference();
            Assert.Equal(0, r); // GetArrayDataReference on empty array returns reference to default value
        }

        [Fact]
        public void NullArray_GetArrayDataReference()
        {
            int[]? arr = null;
            Assert.Throws<NullReferenceException>(() => { ref int r = ref arr.GetReference(); });
        }

        [Fact]
        public void MultiDimensionalArray_GetArrayDataReference()
        {
            int[,] arr = new int[2, 2] { { 1, 2 }, { 3, 4 } };
            ref int r = ref arr.GetReference<int>();
            Assert.Equal(1, r);
            ref int r2 = ref Unsafe.Add(ref r, 1);
            Assert.Equal(2, r2);
            ref int r3 = ref Unsafe.Add(ref r, 2);
            Assert.Equal(3, r3);
            ref int r4 = ref Unsafe.Add(ref r, 3);
            Assert.Equal(4, r4);
            Unchecked.Write(ref r, 0, 10);
            Assert.Equal(10, arr[0, 0]);
            Unchecked.Write(ref r, 1, 11);
            Assert.Equal(11, arr[0, 1]);
            Unchecked.Write(ref r, 2, 12);
            Assert.Equal(12, arr[1, 0]);
            Unchecked.Write(ref r, 3, 13);
            Assert.Equal(13, arr[1, 1]);

            int[,,] arr3D = new int[2, 2, 2] { { { 1, 2 }, { 3, 4 } }, { { 5, 6 }, { 7, 8 } } };
            ref int r3D = ref arr3D.GetReference<int>();
            Assert.Equal(1, r3D);
            Unchecked.Write(ref r3D, 0, 10);
            Assert.Equal(10, arr3D[0, 0, 0]);
            Unchecked.Write(ref r3D, 7, 18);
            Assert.Equal(18, arr3D[1, 1, 1]);
        }

        [Fact]
        public void NullMultiDimensionalArray_GetArrayDataReference()
        {
            int[,]? arr = null;
            Assert.Throws<NullReferenceException>(() => { ref int r = ref arr.GetReference<int>(); });
        }

        [Fact]
        public void String_GetReference()
        {
            string str = "Hello, World!";
            ref char c = ref Unchecked.GetReference(str);
            Assert.Equal('H', c);
        }

        [Fact]
        public void Array_GetReference()
        {
            int[] arr = [1, 2, 3, 4, 5];
            ref int r = ref Unchecked.GetReference(arr);
            Assert.Equal(1, r);
        }

        [Fact]
        public void EmptyString_GetReference()
        {
            string str = "";
            ref char c = ref Unchecked.GetReference(str);
            Assert.Equal('\0', c); // GetReference on empty string returns reference to null terminator
        }

        [Fact]
        public void NullString_GetReference()
        {
            string? str = null;
            Assert.Throws<NullReferenceException>(() => { ref char c = ref Unchecked.GetReference(str); });
        }

        [Fact]
        public void EmptyArray_GetReference()
        {
            int[] arr = [];
            ref int r = ref Unchecked.GetReference(arr);
            Assert.Equal(0, r); // GetReference on empty array returns reference to default value
        }

        [Fact]
        public void NullArray_GetReference()
        {
            int[]? arr = null;
            Assert.Throws<NullReferenceException>(() => { ref int r = ref Unchecked.GetReference(arr); });
        }

        [Fact]
        public void MultiDimensionalArray_GetReference()
        {
            int[,] arr = new int[2, 2] { { 1, 2 }, { 3, 4 } };
            ref int r = ref Unchecked.GetReference<int>(arr);
            Assert.Equal(1, r);
            ref int r2 = ref Unsafe.Add(ref r, 1);
            Assert.Equal(2, r2);
            ref int r3 = ref Unsafe.Add(ref r, 2);
            Assert.Equal(3, r3);
            ref int r4 = ref Unsafe.Add(ref r, 3);
            Assert.Equal(4, r4);
        }

        [Fact]
        public void NullMultiDimensionalArray_GetReference()
        {
            int[,]? arr = null;
            Assert.Throws<NullReferenceException>(() => { ref int r = ref Unchecked.GetReference<int>(arr); });
        }

        [Fact]
        public void ObjectArray_GetReference()
        {
            object[] arr = ["Hello", 123, 3.14];
            ref object r = ref Unchecked.GetReference(arr);
            Assert.Equal("Hello", r);
            ref object r2 = ref Unsafe.Add(ref r, 1);
            Assert.Equal(123, r2);
            ref object r3 = ref Unsafe.Add(ref r, 2);
            Assert.Equal(3.14, r3);
            Unchecked.Array<object> arrWrapper = arr;
            Assert.Equal("Hello", arrWrapper[0]);
            Assert.Equal(123, arrWrapper[1]);
            Assert.Equal(3.14, arrWrapper[2]);
        }

        [Fact]
        public void ObjectArray_Null_GetReference()
        {
            object?[] arr = [null, "Hello", 123];
            ref object? r = ref Unchecked.GetReference(arr);
            Assert.Null(r);
            ref object? r2 = ref Unsafe.Add(ref r, 1);
            Assert.Equal("Hello", r2);
            ref object? r3 = ref Unsafe.Add(ref r, 2);
            Assert.Equal(123, r3);
        }

        [Fact]
        public void NonZeroBasedOneDimensionalArray_BehaviorCheck()
        {
            Array arr = Array.CreateInstance(typeof(int), [2], [5]);
            arr.SetValue(42, 5);
            arr.SetValue(99, 6);
            ref var r = ref arr.GetReference<int>();
            Assert.Equal(42, r);
        }

        [Fact]
        public void ZeroBasedOneDimensionalArray_BehaviorCheck()
        {
            Array arr = Array.CreateInstance(typeof(int), [2], [0]);
            arr.SetValue(42, 0);
            arr.SetValue(99, 1);
            ref var r = ref arr.GetReference<int>();
            Assert.Equal(42, r);
        }
    }
#pragma warning restore CS8604, CS8625, CS8601, CS8602
}