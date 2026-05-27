using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{
    [StructLayout(LayoutKind.Sequential)]
    private struct Array2DInfo
    {
        public unsafe fixed int Values[2];
    }

    [StructLayout(LayoutKind.Sequential)]
    private sealed class RawArray2D
    {
        public LengthAndPadding LengthAndPadding;
        public Array2DInfo Lengths;
        public Array2DInfo LowerBounds;
        public byte Data;
    }

    #region Unchecked Array
    [StructLayout(LayoutKind.Sequential)]
    /// <summary>
    /// Lightweight, unchecked wrapper over a managed two-dimensional array.
    /// </summary>
    /// <remarks>
    /// - High-performance wrapper: intentionally omits bounds checks and other safety checks.
    /// - Supports only zero-based, rectangular 2-D arrays; non-zero lower bounds are not supported.
    /// - Intended for use on .NET Framework 4.7+ and .NET 7+ (PowerShell scenarios).
    /// - The indexer mapping uses row-major order: offset = index1 * dim2Length + index2.
    /// - Callers must ensure indices are valid; out-of-range accesses are undefined behavior.
    /// </remarks>
    public readonly struct Array2D<T>
    {
        #region Fields and Constructor
        private readonly T[,] _array;
        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing 2D arrays.
        private Array2D(T[,] array) => _array = array;
        #endregion
        #region Properties and Indexer
        public int Rank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 2;
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)Unsafe.As<RawArray2D>(_array).LengthAndPadding.Length;
        }

        public long LongLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.As<RawArray2D>(_array).LengthAndPadding.Length;
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
        public unsafe ref T this[int index1, int index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref GetReference(), ((index1 - Unsafe.As<RawArray2D>(_array).LowerBounds.Values[0]) * (nint)Unsafe.As<RawArray2D>(_array).Lengths.Values[1]) + (index2 - Unsafe.As<RawArray2D>(_array).LowerBounds.Values[1]));
        }
        public unsafe ref T this[uint index1, uint index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref GetReference(), ((index1 - (uint)Unsafe.As<RawArray2D>(_array).LowerBounds.Values[0]) * (nuint)Unsafe.As<RawArray2D>(_array).Lengths.Values[1]) + (index2 - (uint)Unsafe.As<RawArray2D>(_array).LowerBounds.Values[1]));
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref _array.GetReference<T>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[,] ToArray() => _array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int GetLength(int dimension) => dimension switch
        {
            0 => Unsafe.As<RawArray2D>(_array).Lengths.Values[0],
            1 => Unsafe.As<RawArray2D>(_array).Lengths.Values[1],
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe long GetLongLength(int dimension) => dimension switch
        {
            0 => Unsafe.As<RawArray2D>(_array).Lengths.Values[0],
            1 => Unsafe.As<RawArray2D>(_array).Lengths.Values[1],
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int GetLowerBound(int dimension) => dimension switch
        {
            0 => Unsafe.As<RawArray2D>(_array).LowerBounds.Values[0],
            1 => Unsafe.As<RawArray2D>(_array).LowerBounds.Values[1],
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe int GetUpperBound(int dimension) => dimension switch
        {
            0 => Unsafe.As<RawArray2D>(_array).LowerBounds.Values[0] + Unsafe.As<RawArray2D>(_array).Lengths.Values[0] - 1,
            1 => Unsafe.As<RawArray2D>(_array).LowerBounds.Values[1] + Unsafe.As<RawArray2D>(_array).Lengths.Values[1] - 1,
            _ => throw new IndexOutOfRangeException(nameof(dimension))
        };
        #endregion

        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array2D<T>(T[,] value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[,](Array2D<T> value) => value._array;
        #endregion

        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new(_array);
        public ref struct Enumerator
        {
            private readonly T[,] _array;
            private readonly uint _length;
            private uint _index;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(T[,] array)
            {
                _array = array;
                _length = Unsafe.As<RawArray2D>(_array).LengthAndPadding.Length;
                _index = uint.MaxValue;
            }
            public readonly ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref Unsafe.Add(ref _array.GetReference<T>(), _index);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++_index < _length;
        }
        #endregion
    }
    #endregion
}