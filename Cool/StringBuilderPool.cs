using System;
using System.Collections.Concurrent;
using System.Text;

namespace Cool;

public class StringBuilderPool
{
    public static readonly StringBuilderPool Shared = new();
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
            sb.Clear();
            if (sb.Capacity < capacity) sb.Capacity = capacity;
            return sb;
        }
        return new StringBuilder(capacity);
    }
    public void Return(StringBuilder sb)
    {
        if (sb.Capacity > MaxCachedCapacity) return;
        if (_pool.Count >= MaxPoolSize) return;
        _pool.Enqueue(sb);
    }
}