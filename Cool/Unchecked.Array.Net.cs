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
            get
            {
                if (indices.Length == 1) return ref this[indices[0]];
                ref int index = ref indices.GetReference();
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = 0;
                for (int i = 0; i < indices.Length; i++)
                {
                    flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, i)) + (Unsafe.Add(ref index, i) - Unsafe.Add(ref bound, i));
                }
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)((uint)flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[params ReadOnlySpan<uint> indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (indices.Length == 1) return ref this[indices[0]];
                ref uint index = ref indices.GetReference();
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = 0;
                for (int i = 0; i < indices.Length; i++)
                {
                    flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, i)) + (Unsafe.Add(ref index, i) - Unsafe.Add(ref bound, i));
                }
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[params ReadOnlySpan<long> indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (indices.Length == 1) return ref this[indices[0]];
                ref long index = ref indices.GetReference();
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = 0;
                for (int i = 0; i < indices.Length; i++)
                {
                    flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, i)) + (Unsafe.Add(ref index, i) - Unsafe.Add(ref bound, i));
                }
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[params ReadOnlySpan<ulong> indices]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (indices.Length == 1) return ref this[indices[0]];
                ref ulong index = ref indices.GetReference();
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = 0;
                for (int i = 0; i < indices.Length; i++)
                {
                    flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, i)) + (Unsafe.Add(ref index, i) - Unsafe.Add(ref bound, i));
                }
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, _offset + (nuint)(flattenedIndex * (ulong)Unsafe.SizeOf<T>())));
            }
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