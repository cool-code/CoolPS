using System;
using System.Runtime.CompilerServices;

namespace Cool;

[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public ref partial struct ValueStringBuilder(int initialCapacity)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueStringBuilder() : this(128) { }
    private Unchecked.SZArray<char> _chars = ArrayPool<char>.Shared.Rent(initialCapacity);
    private int _pos = 0;

    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => _pos;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _pos = value;
    }

    public readonly int Capacity => _chars.Length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EnsureCapacity(int capacity)
    {
        if ((uint)capacity > (uint)_chars.Length) Grow(capacity - _pos);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => _pos = 0;

    public ref char this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ref _chars[index];
    }

    public override string ToString()
    {
        if (_pos == 0)
        {
            Dispose();
            return string.Empty;
        }
        string s = Unchecked.FastAllocateString(_pos);
        Unchecked.Copy(ref _chars.GetReference(), ref s.GetReference(), _pos);
        Dispose();
        return s;
    }

    public void Insert(int index, char value, int count)
    {
        if (_pos > _chars.Length - count) Grow(count);
        int remaining = _pos - index;
        ref char start = ref _chars[index];
        Unchecked.Copy(ref start, ref Unsafe.Add(ref start, count), remaining);
        Unchecked.Fill(ref start, (uint)count, value);
        _pos += count;
    }

    public void Insert(int index, string? s)
    {
        if (s == null || s.Length == 0) return;
        int count = s.Length;
        if (_pos > (_chars.Length - count)) Grow(count);
        int remaining = _pos - index;
        ref char start = ref _chars[index];
        Unchecked.Copy(ref start, ref Unsafe.Add(ref start, count), remaining);
        Unchecked.Copy(ref s.GetReference(), ref start, count);
        _pos += count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char c)
    {
        int pos = _pos;
        if ((uint)pos < (uint)_chars.Length)
        {
            _chars[pos] = c;
            _pos = pos + 1;
        }
        else
        {
            Grow(1);
            _chars[_pos++] = c;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(string? s)
    {
        if (s == null || s.Length == 0) return;
        int pos = _pos;
        if (s.Length == 1 && (uint)pos < (uint)_chars.Length)
        {
            _chars[pos] = s[0];
            _pos = pos + 1;
        }
        else
        {
            AppendSlow(s);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AppendSlow(string s)
    {
        int pos = _pos;
        if (pos > _chars.Length - s.Length) Grow(s.Length);
        Unchecked.Copy(ref s.GetReference(), ref _chars[pos], s.Length);
        _pos += s.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char c, int count)
    {
        if (_pos > _chars.Length - count) Grow(count);
        Unchecked.Fill(ref _chars[_pos], (uint)count, c);
        _pos += count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(ref char start, int length)
    {
        int pos = _pos;
        if (pos > _chars.Length - length) Grow(length);
        Unchecked.Copy(ref start, ref _chars[pos], length);
        _pos += length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Append(char* start, int length) => Append(ref *start, length);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref char AppendRef(int length)
    {
        int pos = _pos;
        if (pos > _chars.Length - length) Grow(length);
        _pos = pos + length;
        return ref _chars[pos];
    }

    /// <summary>
    /// Resize the internal buffer either by doubling current buffer size or
    /// by adding <paramref name="additionalCapacityBeyondPos"/> to
    /// <see cref="_pos"/> whichever is greater.
    /// </summary>
    /// <param name="additionalCapacityBeyondPos">
    /// Number of chars requested beyond current position.
    /// </param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void Grow(int additionalCapacityBeyondPos)
    {
        const uint ArrayMaxLength = 0x7FFFFFC7; // same as Array.MaxLength

        // Increase to at least the required size (_pos + additionalCapacityBeyondPos), but try
        // to double the size if possible, bounding the doubling to not go beyond the max array length.
        int newCapacity = (int)Math.Max(
            (uint)(_pos + additionalCapacityBeyondPos),
            Math.Min((uint)_chars.Length * 2, ArrayMaxLength));

        // Make sure to let Rent throw an exception if the caller has a bug and the desired capacity is negative.
        // This could also go negative if the actual required length wraps around.
        char[] poolArray = ArrayPool<char>.Shared.Rent(newCapacity);
        Unchecked.Copy(ref _chars.GetReference(), ref poolArray.GetReference(), _pos);
        char[] toReturn = _chars;
        _chars = poolArray;
        ArrayPool<char>.Shared.Return(toReturn);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        char[] toReturn = _chars;
        this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
        ArrayPool<char>.Shared.Return(toReturn);
    }
}