#if NETFRAMEWORK
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class NoBoundCheck
{
    #region No Bound Check ReadOnlySpan<T>
    public readonly ref partial struct ReadOnlySpan<T>
    {
        #region Fields and Constructor
        private readonly SpanStub<T> _stub;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan(System.ReadOnlySpan<T> span)
        {
            _stub = AsSpanStub(span);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan(Pinnable<T> pinnable, IntPtr intPtr, int value)
        {
            _stub = new SpanStub<T>(pinnable, intPtr, value);
        }
        #endregion
        #region Properties and Indexer
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _stub.Length;
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _stub.Length == 0;
        }

        public ref readonly T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref _stub.GetReference(), index);
        }
        #endregion
        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Slice(int start)
        {
            return new ReadOnlySpan<T>(_stub.Pinnable, _stub.ByteOffset.Add<T>(start), _stub.Length - start);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Slice(int start, int length)
        {
            return new ReadOnlySpan<T>(_stub.Pinnable, _stub.ByteOffset.Add<T>(start), length);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public unsafe ref readonly T GetPinnableReference()
        {
            if (_stub.Length != 0)
            {
                return ref _stub.GetReference();
            }
            return ref Unsafe.AsRef<T>(null);
        }
        #endregion
    }

    #endregion

}
#endif