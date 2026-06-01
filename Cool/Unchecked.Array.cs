using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Cool;

public static partial class Unchecked
{

    [StructLayout(LayoutKind.Sequential)]
    internal sealed class RawArray
    {
        internal readonly IntPtr LengthAndPadding;
        internal byte Data;
    }

    #region helper methods for Array<T>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ref byte GetRawArrayData(this Array array)
    {
        return ref Unsafe.As<RawArray>(array).Data;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal unsafe static uint GetLength(this Array array)
    {
        return Unsafe.SubtractByteOffset(ref Unsafe.As<byte, uint>(ref array.GetRawArrayData()), (nint)sizeof(IntPtr));
    }
    #endregion

    #region Unchecked Array
    [StructLayout(LayoutKind.Sequential)]
    /// <summary>
    /// Lightweight, unchecked wrapper over a managed array.
    /// </summary>
    /// <remarks>
    /// - High-performance wrapper: intentionally omits bounds checks and other safety checks.
    /// - Intended for use on .NET Framework 4.7+ and .NET 7+ (PowerShell scenarios).
    /// - Callers must ensure indices are valid; out-of-range accesses are undefined behavior.
    /// </remarks>
    public readonly struct Array<T>
    {
        #region Fields and Constructor
        private readonly Array _array;
        internal readonly uint _offset;
        internal readonly uint _rank;
        // The constructor is private to prevent external instantiation,
        // as the class is designed to be used as a wrapper around existing arrays.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe Array(Array array)
        {
            _array = array;
            _offset = Unsafe.GetBaseSize(array) - (uint)(3 * sizeof(IntPtr));
            _rank = _offset / (2 * sizeof(int));
        }
        #endregion

        #region Properties and Indexer
        public int Rank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (int)(_rank ^ ((_rank - 1) >> 31));
            }
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
        public bool IsSZArray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _rank == 0;
        }
        public bool IsMDArray
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _rank > 0;
        }
        // The rank of the array if it's a multi-dimensional array; otherwise, 0 for single-dimensional zero-based arrays.
        public int MDArrayRank
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (int)_rank;
        }
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                if (_rank == 0) return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref rawData), index);
                index -= Unsafe.Add(ref Unsafe.As<byte, int>(ref rawData), _rank);
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)((uint)index * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                if (_rank == 0) return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref rawData), index);
                index -= Unsafe.Add(ref Unsafe.As<byte, uint>(ref rawData), _rank);
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(index * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[long index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                if (_rank == 0) return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref rawData), (nint)index);
                index -= Unsafe.Add(ref Unsafe.As<byte, int>(ref rawData), _rank);
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(index * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[ulong index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                if (_rank == 0) return ref Unsafe.Add(ref Unsafe.As<byte, T>(ref rawData), (nuint)index);
                index -= Unsafe.Add(ref Unsafe.As<byte, uint>(ref rawData), 1);
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, _offset + (nuint)(index * (ulong)Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[int index1, int index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)((uint)flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[uint index1, uint index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[long index1, long index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[ulong index1, ulong index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, _offset + (nuint)(flattenedIndex * (ulong)Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[int index1, int index2, int index3]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)((uint)flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[uint index1, uint index2, uint index3]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[long index1, long index2, long index3]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, _offset + (nuint)(flattenedIndex * (ulong)Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[int index1, int index2, int index3, int index4]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)((uint)flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[long index1, long index2, long index3, long index4]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, _offset + (nuint)(flattenedIndex * (ulong)Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)((uint)flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, _offset + (nuint)(flattenedIndex * (ulong)Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)((uint)flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, _offset + (nuint)(flattenedIndex * (ulong)Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 6)) + (index7 - Unsafe.Add(ref bound, 6));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)((uint)flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 6)) + (index7 - Unsafe.Add(ref bound, 6));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 6)) + (index7 - Unsafe.Add(ref bound, 6));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 6)) + (index7 - Unsafe.Add(ref bound, 6));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, _offset + (nuint)(flattenedIndex * (ulong)Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 6)) + (index7 - Unsafe.Add(ref bound, 6));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 7)) + (index8 - Unsafe.Add(ref bound, 7));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)((uint)flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 6)) + (index7 - Unsafe.Add(ref bound, 6));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 7)) + (index8 - Unsafe.Add(ref bound, 7));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 6)) + (index7 - Unsafe.Add(ref bound, 6));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 7)) + (index8 - Unsafe.Add(ref bound, 7));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, (nint)_offset + (nint)(flattenedIndex * Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 1)) + (index2 - Unsafe.Add(ref bound, 1));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 2)) + (index3 - Unsafe.Add(ref bound, 2));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 3)) + (index4 - Unsafe.Add(ref bound, 3));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 4)) + (index5 - Unsafe.Add(ref bound, 4));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 5)) + (index6 - Unsafe.Add(ref bound, 5));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 6)) + (index7 - Unsafe.Add(ref bound, 6));
                flattenedIndex = (flattenedIndex * Unsafe.Add(ref length, 7)) + (index8 - Unsafe.Add(ref bound, 7));
                return ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref rawData, _offset + (nuint)(flattenedIndex * (ulong)Unsafe.SizeOf<T>())));
            }
        }
        public ref T this[params int[] indices]
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
        public ref T this[params uint[] indices]
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
        public ref T this[params long[] indices]
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
        public ref T this[params ulong[] indices]
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
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetReference() => ref Unsafe.As<byte, T>(ref Unsafe.AddByteOffset(ref _array.GetRawArrayData(), _offset));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() => ref GetReference();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Array ToArray() => _array;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLength(uint dimension)
        {
            if (_rank == 0 && dimension == 0) return Length;
            if (dimension < _rank) return Unsafe.Add(ref Unsafe.As<byte, int>(ref _array.GetRawArrayData()), dimension);
            return 0;
        }
        public int GetLength(int dimension)
        {
            if (_rank == 0 && dimension == 0) return Length;
            if ((uint)dimension < _rank) return Unsafe.Add(ref Unsafe.As<byte, int>(ref _array.GetRawArrayData()), dimension);
            throw new IndexOutOfRangeException(nameof(dimension));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(uint dimension) => GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long GetLongLength(int dimension) => GetLength(dimension);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetLowerBound(uint dimension)
        {
            if (_rank == 0 && dimension == 0) return 0;
            if (dimension < _rank) return Unsafe.Add(ref Unsafe.As<byte, int>(ref _array.GetRawArrayData()), dimension + _rank);
            return 0;
        }
        public int GetLowerBound(int dimension)
        {
            if (_rank == 0 && dimension == 0) return 0;
            if ((uint)dimension < _rank) return Unsafe.Add(ref Unsafe.As<byte, int>(ref _array.GetRawArrayData()), (uint)dimension + _rank);
            throw new IndexOutOfRangeException(nameof(dimension));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUpperBound(uint dimension)
        {
            if (_rank == 0 && dimension == 0) return Length - 1;
            if (dimension < _rank)
            {
                ref int lengthStart = ref Unsafe.As<byte, int>(ref _array.GetRawArrayData());
                int length = Unsafe.Add(ref lengthStart, dimension);
                int lowerBound = Unsafe.Add(ref lengthStart, dimension + _rank);
                return length + lowerBound - 1;
            }
            return -1;
        }
        public int GetUpperBound(int dimension)
        {
            if (_rank == 0 && dimension == 0) return Length - 1;
            if ((uint)dimension < _rank)
            {
                ref int lengthStart = ref Unsafe.As<byte, int>(ref _array.GetRawArrayData());
                int length = Unsafe.Add(ref lengthStart, dimension);
                int lowerBound = Unsafe.Add(ref lengthStart, (uint)dimension + _rank);
                return length + lowerBound - 1;
            }
            throw new IndexOutOfRangeException(nameof(dimension));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index) => this[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5) => this[index1, index2, index3, index4, index5];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5) => this[index1, index2, index3, index4, index5];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5) => this[index1, index2, index3, index4, index5];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5) => this[index1, index2, index3, index4, index5];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6) => this[index1, index2, index3, index4, index5, index6];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6) => this[index1, index2, index3, index4, index5, index6];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6) => this[index1, index2, index3, index4, index5, index6];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6) => this[index1, index2, index3, index4, index5, index6];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7) => this[index1, index2, index3, index4, index5, index6, index7];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7) => this[index1, index2, index3, index4, index5, index6, index7];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7) => this[index1, index2, index3, index4, index5, index6, index7];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7) => this[index1, index2, index3, index4, index5, index6, index7];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8) => this[index1, index2, index3, index4, index5, index6, index7, index8];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8) => this[index1, index2, index3, index4, index5, index6, index7, index8];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8) => this[index1, index2, index3, index4, index5, index6, index7, index8];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8) => this[index1, index2, index3, index4, index5, index6, index7, index8];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params int[] indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params uint[] indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params long[] indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(params ulong[] indices) => this[indices];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index) => this[index] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5) => this[index1, index2, index3, index4, index5] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5) => this[index1, index2, index3, index4, index5] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5) => this[index1, index2, index3, index4, index5] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5) => this[index1, index2, index3, index4, index5] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6) => this[index1, index2, index3, index4, index5, index6] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6) => this[index1, index2, index3, index4, index5, index6] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6) => this[index1, index2, index3, index4, index5, index6] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6) => this[index1, index2, index3, index4, index5, index6] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7) => this[index1, index2, index3, index4, index5, index6, index7] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7) => this[index1, index2, index3, index4, index5, index6, index7] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7) => this[index1, index2, index3, index4, index5, index6, index7] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7) => this[index1, index2, index3, index4, index5, index6, index7] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8) => this[index1, index2, index3, index4, index5, index6, index7, index8] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8) => this[index1, index2, index3, index4, index5, index6, index7, index8] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8) => this[index1, index2, index3, index4, index5, index6, index7, index8] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8) => this[index1, index2, index3, index4, index5, index6, index7, index8] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params int[] indices) => this[indices] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params uint[] indices) => this[indices] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params long[] indices) => this[indices] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, params ulong[] indices) => this[indices] = value;
        #endregion
        #region Implicit Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array<T>(Array value) => new(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Array(Array<T> value) => value._array;
        #endregion
        #region Enumerator
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArrayEnumerator<T> GetEnumerator() => new(_array, _offset);
        #endregion
    }
    #endregion
}