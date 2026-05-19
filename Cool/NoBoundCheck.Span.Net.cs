#if NET7_0_OR_GREATER
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class NoBoundCheck
{
    #region Unsafe Read and Write for Span<T>
    public readonly ref partial struct Span<T>
    {
        #region Fields and Constructor
        private readonly ref T _ref;
        private readonly int _length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(System.Span<T> span)
        {
            _ref = ref MemoryMarshal.GetReference(span);
            _length = span.Length;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span(ref T reference, int length)
        {
            _ref = ref reference;
            _length = length;
        }
        #endregion
        #region Properties and Indexer
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _length;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _length == 0;
        }

        public readonly ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref _ref, index);
        }
        #endregion
        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int start)
        {
            return new Span<T>(ref this[start], _length - start);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int start, int length)
        {
            return new Span<T>(ref this[start], length);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public unsafe readonly ref T GetPinnableReference()
        {
            if (_length != 0)
            {
                return ref _ref;
            }
            return ref Unsafe.AsRef<T>(null);
        }
        #endregion
    }

    #endregion
}
#endif