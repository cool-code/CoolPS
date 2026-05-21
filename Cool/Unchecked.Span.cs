using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    public readonly ref partial struct Span<T>
    {
        #region implicit and explicit operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Span<T>(System.Span<T> span) => AsSpan(ref span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.Span<T>(Span<T> span) => AsSystemSpan(ref span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.ReadOnlySpan<T>(Span<T> span) => AsSystemReadOnlySpan(ref span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpan<T>(Span<T> span) => AsReadOnlySpan(ref span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Span<T>(System.ReadOnlySpan<T> span) => AsSpan(ref span);
        #endregion

        #region Static members and constructors
        public static Span<T> Empty => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(T[] array) : this(new System.Span<T>(array)) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(T[] array, int start, int length) : this(new System.Span<T>(array, start, length)) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span(void* pointer, int length) : this(new System.Span<T>(pointer, length)) { }
        #endregion

        #region Equality Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Span<T> left, Span<T> right) => AsSystemSpan(ref left) == AsSystemSpan(ref right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Span<T> left, Span<T> right) => AsSystemSpan(ref left) != AsSystemSpan(ref right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? o) => AsSystemSpanRef(in this).Equals(o);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => AsSystemSpanRef(in this).GetHashCode();
        #endregion

        #region Common Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Clear() => AsSystemSpanRef(in this).Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Fill(T value) => AsSystemSpanRef(in this).Fill(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(System.Span<T> destination) => AsSystemSpanRef(in this).CopyTo(destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(System.Span<T> destination) => AsSystemSpanRef(in this).TryCopyTo(destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T[] ToArray() => AsSystemSpanRef(in this).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() => AsSystemSpanRef(in this).ToString();
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
            public readonly ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref _span[_index];
            }
        }
        #endregion
    }
}