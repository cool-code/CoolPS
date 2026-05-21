using System.Runtime.CompilerServices;

namespace Cool;

public static partial class NoBoundCheck
{
    public readonly ref partial struct ReadOnlySpan<T>
    {
        #region implicit and explicit operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpan<T>(System.ReadOnlySpan<T> span) => AsReadOnlySpan(ref span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.ReadOnlySpan<T>(ReadOnlySpan<T> span) => AsSystemReadOnlySpan(ref span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpan<T>(System.Span<T> span) => AsReadOnlySpan(ref span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Span<T>(ReadOnlySpan<T> span) => AsSpan(ref span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.Span<T>(ReadOnlySpan<T> span) => AsSystemSpan(ref span);
        #endregion

        #region Static members and constructors
        public static ReadOnlySpan<T> Empty => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan(T[] array) : this(new System.ReadOnlySpan<T>(array)) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan(T[] array, int start, int length) : this(new System.ReadOnlySpan<T>(array, start, length)) { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ReadOnlySpan(void* pointer, int length) : this(new System.ReadOnlySpan<T>(pointer, length)) { }
        #endregion

        #region Equality Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ReadOnlySpan<T> left, ReadOnlySpan<T> right) => AsSystemReadOnlySpan(ref left) == AsSystemReadOnlySpan(ref right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ReadOnlySpan<T> left, ReadOnlySpan<T> right) => AsSystemReadOnlySpan(ref left) != AsSystemReadOnlySpan(ref right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? o) => AsSystemReadOnlySpanRef(in this).Equals(o);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => AsSystemReadOnlySpanRef(in this).GetHashCode();
        #endregion

        #region Common Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(System.Span<T> destination) => AsSystemReadOnlySpanRef(in this).CopyTo(destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(System.Span<T> destination) => AsSystemReadOnlySpanRef(in this).TryCopyTo(destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T[] ToArray() => AsSystemReadOnlySpanRef(in this).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() => AsSystemReadOnlySpanRef(in this).ToString();
        #endregion

        #region Enumeration
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>Gets an enumerator for this span.</summary>
        public Enumerator GetEnumerator() => new(this);

        /// <summary>Enumerates the elements of a <see cref="ReadOnlySpan{T}"/>.</summary>
        public ref struct Enumerator
        {
            /// <summary>The span being enumerated.</summary>
            private readonly ReadOnlySpan<T> _span;
            /// <summary>The next index to yield.</summary>
            private int _index;

            /// <summary>Initialize the enumerator.</summary>
            /// <param name="span">The span to enumerate.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(ReadOnlySpan<T> span)
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