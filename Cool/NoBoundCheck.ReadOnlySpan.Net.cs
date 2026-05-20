#if NET7_0_OR_GREATER
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class NoBoundCheck
{
    #region No Bound Check ReadOnlySpan<T>
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref partial struct ReadOnlySpan<T>
    {
        #region Fields and Constructor
        /// <summary>A byref or a native ptr.</summary>
        internal readonly ref T _reference;
        /// <summary>The number of elements this Span contains.</summary>
        private readonly int _length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan(System.ReadOnlySpan<T> span)
        {
            _reference = ref MemoryMarshal.GetReference(span);
            _length = span.Length;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan(ref T reference, int length)
        {
            _reference = ref reference;
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
            get => ref Unsafe.Add(ref _reference, index);
        }
        #endregion
        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Slice(int start)
        {
            return new ReadOnlySpan<T>(ref this[start], _length - start);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Slice(int start, int length)
        {
            return new ReadOnlySpan<T>(ref this[start], length);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public unsafe ref readonly T GetPinnableReference()
        {
            if (_length != 0)
            {
                return ref _reference;
            }
            return ref Unsafe.AsRef<T>(null);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal ref T DangerousGetPinnableReference()
        {
            return ref _reference;
        }
        #endregion
    }

    #endregion
}
#endif