#if NETFRAMEWORK
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cool;

public static partial class Unchecked
{

    #region No Bound Check Span<T>
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref partial struct Span<T>
    {
        #region Fields and Constructor
        private readonly SpanStub<T> _stub;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(System.Span<T> span)
        {
            _stub = AsSpanStub(span);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span(Pinnable<T> pinnable, IntPtr intPtr, int value)
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

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref _stub.GetReference(), index);

        }
        #endregion
        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int start)
        {
            return new Span<T>(_stub.Pinnable, _stub.ByteOffset.Add<T>(start), _stub.Length - start);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int start, int length)
        {
            return new Span<T>(_stub.Pinnable, _stub.ByteOffset.Add<T>(start), length);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public unsafe ref T GetPinnableReference()
        {
            if (_stub.Length != 0)
            {
                return ref _stub.GetReference();
            }
            return ref Unsafe.AsRef<T>(null);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal ref T DangerousGetPinnableReference()
        {
            return ref _stub.GetReference();
        }
        #endregion
    }

    #endregion
}
#endif