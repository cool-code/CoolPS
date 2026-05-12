using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace Cool;

public class StringBuilderPool
{
    public static readonly StringBuilderPool Shared = new();
    private volatile int _currentCount = 0;
    private readonly ConcurrentQueue<StringBuilder> _pool = new();
    private readonly int MaxCachedCapacity = 128 * 1024;
    private readonly int MaxPoolSize = 32;
    public StringBuilderPool() { }
    public StringBuilderPool(int maxCachedCapacity, int maxPoolSize)
    {
        if (maxCachedCapacity <= 0) throw new ArgumentOutOfRangeException(nameof(maxCachedCapacity));
        if (maxPoolSize <= 0) throw new ArgumentOutOfRangeException(nameof(maxPoolSize));
        MaxCachedCapacity = maxCachedCapacity;
        MaxPoolSize = maxPoolSize;
    }
    public StringBuilder Rent(int capacity = 128)
    {
        if (_pool.TryDequeue(out var sb))
        {
            Interlocked.Decrement(ref _currentCount);
            sb.Clear();
            if (sb.Capacity < capacity) sb.Capacity = capacity;
            return sb;
        }
        return new StringBuilder(capacity);
    }
    public void Return(StringBuilder sb)
    {
        if (sb.Capacity > MaxCachedCapacity) return;
        if (_currentCount >= MaxPoolSize) return;
        _pool.Enqueue(sb);
        Interlocked.Increment(ref _currentCount);
    }
}