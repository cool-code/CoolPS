using System.Collections.Concurrent;
using System.Text;

namespace Cool;

public class StringBuilderPool
{
    public static readonly StringBuilderPool Shared = new();
    private readonly ConcurrentQueue<StringBuilder> _pool = new();

    public StringBuilder Rent(int capacity = 128)
    {
        if (_pool.TryDequeue(out var sb))
        {
            sb.Clear();
            return sb;
        }
        return new StringBuilder(capacity);
    }

    public void Return(StringBuilder sb)
    {
        _pool.Enqueue(sb);
    }
}