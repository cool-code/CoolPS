using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Sequential)]
    private sealed class Array4D
    {
        internal readonly IntPtr LengthAndPadding;
        private readonly int Dim1Length;
        private readonly int Dim2Length;
        private readonly int Dim3Length;
        private readonly int Dim4Length;
        private readonly int Dim1LowerBound;
        private readonly int Dim2LowerBound;
        private readonly int Dim3LowerBound;
        private readonly int Dim4LowerBound;
        internal byte Data;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference<T>() => ref Unsafe.As<byte, T>(ref Data);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetFlattenedIndex(uint index1, uint index2, uint index3, uint index4) => (((((index1 * (uint)Dim2Length) + index2) * (uint)Dim3Length) + index3) * (uint)Dim4Length) + index4;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint GetFlattenedIndex(int index1, int index2, int index3, int index4) => (uint)((((((index1 * Dim2Length) + index2) * Dim3Length) + index3) * Dim4Length) + index4);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>(int index1, int index2, int index3, int index4) => ref Unsafe.Add(ref GetReference<T>(), GetFlattenedIndex(index1, index2, index3, index4));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T Get<T>(uint index1, uint index2, uint index3, uint index4) => ref Unsafe.Add(ref GetReference<T>(), GetFlattenedIndex(index1, index2, index3, index4));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => dimension switch
        {
            0 => Dim1Length,
            1 => Dim2Length,
            2 => Dim3Length,
            3 => Dim4Length,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => dimension switch
        {
            0 => Dim1Length,
            1 => Dim2Length,
            2 => Dim3Length,
            3 => Dim4Length,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => dimension switch
        {
            0 => Dim1LowerBound,
            1 => Dim2LowerBound,
            2 => Dim3LowerBound,
            3 => Dim4LowerBound,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => dimension switch
        {
            0 => Dim1LowerBound + Dim1Length - 1,
            1 => Dim2LowerBound + Dim2Length - 1,
            2 => Dim3LowerBound + Dim3Length - 1,
            3 => Dim4LowerBound + Dim4Length - 1,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
    }

    #region Unchecked Array
    [StructLayout(LayoutKind.Sequential)]
    /// <summary>
    /// Lightweight, unchecked wrapper over a managed four-dimensional array.
    /// </summary>
    /// <remarks>
    /// - High-performance wrapper: intentionally omits bounds checks and other safety checks.
    /// - Only supports zero-based arrays Indexer, non-zero lower bounds are not supported.
    /// - Intended for use on .NET Framework 4.7+ and .NET 7+ (PowerShell scenarios).
    /// - The indexer mapping uses row-major order: offset = (((((index1 * dim2Length) + index2) * dim3Length) + index3) * dim4Length) + index4.
    /// - Callers must ensure indices are valid; out-of-range accesses are undefined behavior.
    /// </remarks>
    public readonly struct Array4D<T>
    {
        #region Fields and Constructor
        private readonly T[,,,] _array;
        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing 4D arrays.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Array4D(T[,,,] array) => _array = array;
        #endregion
        #region Properties and Indexer
        public int Rank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 4;
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)_array.GetLength();
        }
        public long LongLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _array.GetLength();
        }

        public bool IsFixedSize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => true;
        }

        public bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }
        public bool IsSynchronized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }
        public object SyncRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _array;
        }

        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref GetReference(), index);
        }
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref GetReference(), (nint)index);
        }
        public ref T this[int index1, int index2, int index3, int index4]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.AsRef<T[,,,], Array4D>(_array).Get<T>(index1, index2, index3, index4);
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.AsRef<T[,,,], Array4D>(_array).Get<T>(index1, index2, index3, index4);
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref _array.GetReference<T>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[,,,] ToArray() => _array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(int dimension) => Unsafe.AsRef<T[,,,], Array4D>(_array).GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => Unsafe.AsRef<T[,,,], Array4D>(_array).GetLongLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(int dimension) => Unsafe.AsRef<T[,,,], Array4D>(_array).GetLowerBound(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(int dimension) => Unsafe.AsRef<T[,,,], Array4D>(_array).GetUpperBound(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index) => this[(uint)index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index) => this[(uint)index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index) => this[(uint)index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index) => this[(uint)index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4) => this[(uint)index1, (uint)index2, (uint)index3, (uint)index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4) => this[(uint)index1, (uint)index2, (uint)index3, (uint)index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4) => this[(uint)index1, (uint)index2, (uint)index3, (uint)index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4) => this[(uint)index1, (uint)index2, (uint)index3, (uint)index4] = value;
        #endregion

        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array4D<T>(T[,,,] value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[,,,](Array4D<T> value) => value._array;
        #endregion

        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayEnumerator<T> GetEnumerator() => new(_array, sizeof(int) * 8);
        #endregion
    }
    #endregion
}