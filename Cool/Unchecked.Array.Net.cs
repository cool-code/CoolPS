#if !NETFRAMEWORK
using System;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    #region Unchecked Array
    public readonly partial struct Array<T>
    {
        #region ReadOnlySpan-based indexer
        public ref T this[params ReadOnlySpan<int> indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref GetReference(ref indices.GetReference(), indices.Length);
        }
        public ref T this[params ReadOnlySpan<uint> indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref GetReference(ref indices.GetReference(), indices.Length);
        }
        public ref T this[params ReadOnlySpan<long> indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref GetReference(ref indices.GetReference(), indices.Length);
        }
        public ref T this[params ReadOnlySpan<ulong> indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref GetReference(ref indices.GetReference(), indices.Length);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params ReadOnlySpan<int> indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params ReadOnlySpan<uint> indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params ReadOnlySpan<long> indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params ReadOnlySpan<ulong> indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params ReadOnlySpan<int> indices) => this[indices] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params ReadOnlySpan<uint> indices) => this[indices] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params ReadOnlySpan<long> indices) => this[indices] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params ReadOnlySpan<ulong> indices) => this[indices] = value;
        #endregion
    }
    #endregion
}
#endif