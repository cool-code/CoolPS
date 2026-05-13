using System;
using Xunit;

namespace Cool.Tests
{
    public class ArrayPoolTests
    {
        [Fact]
        public void RentAndReturn_Basic()
        {
            var pool = Cool.ArrayPool<char>.Shared;
            var arr = pool.Rent(100);
            Assert.True(arr.Length >= 100);
            pool.Return(arr);

            var arr2 = pool.Rent(100);
            Assert.True(arr2.Length >= 100);
            pool.Return(arr2);
        }

        [Fact]
        public void Concurrent_RentReturn_NoCrash()
        {
            var pool = Cool.ArrayPool<int>.Shared;
            const int threads = 8;
            var tasks = new System.Threading.Tasks.Task[threads];
            for (int t = 0; t < threads; t++)
            {
                tasks[t] = System.Threading.Tasks.Task.Run(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        var a = pool.Rent(128);
                        a[0] = 42;
                        pool.Return(a);
                    }
                });
            }
            System.Threading.Tasks.Task.WaitAll(tasks);
        }
    }
}
