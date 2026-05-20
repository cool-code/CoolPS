using System.Runtime.CompilerServices;

namespace Cool;

public static partial class NoBoundCheck
{
    public readonly ref partial struct ReadOnlySpan<T>
    {
        #region implicit and explicit operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpan<T>(in System.ReadOnlySpan<T> span) => AsReadOnlySpan(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator System.ReadOnlySpan<T>(in ReadOnlySpan<T> span) => AsSystemReadOnlySpan(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpan<T>(in System.Span<T> span) => AsReadOnlySpan(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Span<T>(in ReadOnlySpan<T> span) => AsSpan(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator System.Span<T>(in ReadOnlySpan<T> span) => AsSystemSpan(span);
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
        public static bool operator ==(in ReadOnlySpan<T> left, in ReadOnlySpan<T> right) => AsSystemReadOnlySpan(left) == AsSystemReadOnlySpan(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in ReadOnlySpan<T> left, in ReadOnlySpan<T> right) => AsSystemReadOnlySpan(left) != AsSystemReadOnlySpan(right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? o) => AsSystemReadOnlySpan<T>(this).Equals(o);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => AsSystemReadOnlySpan<T>(this).GetHashCode();
        #endregion

        #region Common Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void CopyTo(System.Span<T> destination) => AsSystemReadOnlySpan<T>(this).CopyTo(destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool TryCopyTo(System.Span<T> destination) => AsSystemReadOnlySpan<T>(this).TryCopyTo(destination);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T[] ToArray() => AsSystemReadOnlySpan<T>(this).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() => AsSystemReadOnlySpan<T>(this).ToString();
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