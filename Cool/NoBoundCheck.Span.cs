using System.Runtime.CompilerServices;

namespace Cool;

public static partial class NoBoundCheck
{
    public readonly ref partial struct Span<T>
    {
        #region implicit and explicit operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Span<T>(in System.Span<T> span) => AsSpan(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Span<T>(in Span<T> span) => AsSystemSpan(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.ReadOnlySpan<T>(in Span<T> span) => AsSystemReadOnlySpan(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpan<T>(in Span<T> span) => AsReadOnlySpan(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Span<T>(in System.ReadOnlySpan<T> span) => AsSpan(span);
        #endregion

        #region Equality Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in Span<T> left, in Span<T> right) => AsSystemSpan(left) == AsSystemSpan(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in Span<T> left, in Span<T> right) => AsSystemSpan(left) != AsSystemSpan(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? o) => AsSystemSpan<T>(this).Equals(o);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => AsSystemSpan<T>(this).GetHashCode();
        #endregion

        #region Common Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Clear() => AsSystemSpan<T>(this).Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Fill(T value) => AsSystemSpan<T>(this).Fill(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(System.Span<T> destination) => AsSystemSpan<T>(this).CopyTo(destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(System.Span<T> destination) => AsSystemSpan<T>(this).TryCopyTo(destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T[] ToArray() => AsSystemSpan<T>(this).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() => AsSystemSpan<T>(this).ToString();
        #endregion

        #region Enumeration
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>Gets an enumerator for this span.</summary>
        public Enumerator GetEnumerator() => new(this);

        /// <summary>Enumerates the elements of a <see cref="Span{T}"/>.</summary>
        public ref struct Enumerator
        {
            /// <summary>The span being enumerated.</summary>
            private readonly Span<T> _span;
            /// <summary>The next index to yield.</summary>
            private int _index;

            /// <summary>Initialize the enumerator.</summary>
            /// <param name="span">The span to enumerate.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(Span<T> span)
            {
                _span = span;
                _index = -1;
            }

            /// <summary>Advances the enumerator to the next element of the span.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                int index = _index + 1;
                if (index < _span.Length)
                {
                    _index = index;
                    return true;
                }

                return false;
            }

            /// <summary>Gets the element at the current position of the enumerator.</summary>
            public readonly ref readonly T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref _span[_index];
            }
        }
        #endregion
    }
}