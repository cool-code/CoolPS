using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    #region Generic Unchecked Array (Supports Arbitrary Dimensions)
    public readonly partial struct Array<T>
    {
        public ref T this[int index1, int index2]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                for (int i = 1; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
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
                for (int i = 1; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
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
                for (int i = 1; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
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
                for (int i = 1; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                for (int i = 2; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                for (int i = 2; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                for (int i = 2; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                for (int i = 2; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                for (int i = 3; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                for (int i = 3; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                for (int i = 3; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                for (int i = 3; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                for (int i = 4; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                for (int i = 4; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                for (int i = 4; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                for (int i = 4; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                for (int i = 5; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                for (int i = 5; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                for (int i = 5; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                for (int i = 5; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                for (int i = 6; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                for (int i = 6; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                for (int i = 6; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                for (int i = 6; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                for (int i = 7; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                for (int i = 7; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                for (int i = 7; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
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
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                for (int i = 7; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                for (int i = 8; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                for (int i = 8; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                for (int i = 8; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                for (int i = 8; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                for (int i = 9; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                for (int i = 9; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                for (int i = 9; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                for (int i = 9; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                for (int i = 10; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                for (int i = 10; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                for (int i = 10; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                for (int i = 10; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                for (int i = 11; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                for (int i = 11; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                for (int i = 11; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                for (int i = 11; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                for (int i = 12; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                for (int i = 12; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                for (int i = 12; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                for (int i = 12; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                for (int i = 13; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                for (int i = 13; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                for (int i = 13; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                for (int i = 13; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                for (int i = 14; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                for (int i = 14; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                for (int i = 14; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                for (int i = 14; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                for (int i = 15; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                for (int i = 15; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                for (int i = 15; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                for (int i = 15; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                for (int i = 16; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                for (int i = 16; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                for (int i = 16; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                for (int i = 16; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                for (int i = 17; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                for (int i = 17; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                for (int i = 17; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                for (int i = 17; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                for (int i = 18; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                for (int i = 18; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                for (int i = 18; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                for (int i = 18; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                for (int i = 19; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                for (int i = 19; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                for (int i = 19; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                for (int i = 19; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                for (int i = 20; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                for (int i = 20; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                for (int i = 20; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                for (int i = 20; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                for (int i = 21; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                for (int i = 21; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                for (int i = 21; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                for (int i = 21; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                for (int i = 22; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                for (int i = 22; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                for (int i = 22; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                for (int i = 22; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                for (int i = 23; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                for (int i = 23; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                for (int i = 23; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                for (int i = 23; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                for (int i = 24; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                for (int i = 24; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                for (int i = 24; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                for (int i = 24; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                for (int i = 25; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                for (int i = 25; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                for (int i = 25; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                for (int i = 25; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                for (int i = 26; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                for (int i = 26; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                for (int i = 26; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                for (int i = 26; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                for (int i = 27; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                for (int i = 27; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                for (int i = 27; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                for (int i = 27; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                for (int i = 28; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                for (int i = 28; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                for (int i = 28; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                for (int i = 28; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29, int index30]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                for (int i = 29; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29, uint index30]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                for (int i = 29; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29, long index30]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                for (int i = 29; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29, ulong index30]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                for (int i = 29; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29, int index30, int index31]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                flattenedIndex *= Unsafe.Add(ref length, 29);
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                for (int i = 30; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index31 - Unsafe.Add(ref bound, 30);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29, uint index30, uint index31]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                flattenedIndex *= Unsafe.Add(ref length, 29);
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                for (int i = 30; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index31 - Unsafe.Add(ref bound, 30);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29, long index30, long index31]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                flattenedIndex *= Unsafe.Add(ref length, 29);
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                for (int i = 30; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index31 - Unsafe.Add(ref bound, 30);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29, ulong index30, ulong index31]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                flattenedIndex *= Unsafe.Add(ref length, 29);
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                for (int i = 30; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index31 - Unsafe.Add(ref bound, 30);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        public ref T this[int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29, int index30, int index31, int index32]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                int flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                flattenedIndex *= Unsafe.Add(ref length, 29);
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                flattenedIndex *= Unsafe.Add(ref length, 30);
                flattenedIndex += index31 - Unsafe.Add(ref bound, 30);
                for (int i = 31; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index32 - Unsafe.Add(ref bound, 31);
                return ref Unsafe.Add(ref GetReference(), (nint)(uint)flattenedIndex);
            }
        }
        public ref T this[uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29, uint index30, uint index31, uint index32]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                uint flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                flattenedIndex *= Unsafe.Add(ref length, 29);
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                flattenedIndex *= Unsafe.Add(ref length, 30);
                flattenedIndex += index31 - Unsafe.Add(ref bound, 30);
                for (int i = 31; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index32 - Unsafe.Add(ref bound, 31);
                return ref Unsafe.Add(ref GetReference(), flattenedIndex);
            }
        }
        public ref T this[long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29, long index30, long index31, long index32]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref int length = ref Unsafe.As<byte, int>(ref rawData);
                ref int bound = ref Unsafe.Add(ref length, _rank);
                long flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                flattenedIndex *= Unsafe.Add(ref length, 29);
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                flattenedIndex *= Unsafe.Add(ref length, 30);
                flattenedIndex += index31 - Unsafe.Add(ref bound, 30);
                for (int i = 31; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index32 - Unsafe.Add(ref bound, 31);
                return ref Unsafe.Add(ref GetReference(), (nint)flattenedIndex);
            }
        }
        public ref T this[ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29, ulong index30, ulong index31, ulong index32]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ref byte rawData = ref _array.GetRawArrayData();
                ref uint length = ref Unsafe.As<byte, uint>(ref rawData);
                ref uint bound = ref Unsafe.Add(ref length, _rank);
                ulong flattenedIndex = index1 - bound;
                flattenedIndex *= Unsafe.Add(ref length, 1);
                flattenedIndex += index2 - Unsafe.Add(ref bound, 1);
                flattenedIndex *= Unsafe.Add(ref length, 2);
                flattenedIndex += index3 - Unsafe.Add(ref bound, 2);
                flattenedIndex *= Unsafe.Add(ref length, 3);
                flattenedIndex += index4 - Unsafe.Add(ref bound, 3);
                flattenedIndex *= Unsafe.Add(ref length, 4);
                flattenedIndex += index5 - Unsafe.Add(ref bound, 4);
                flattenedIndex *= Unsafe.Add(ref length, 5);
                flattenedIndex += index6 - Unsafe.Add(ref bound, 5);
                flattenedIndex *= Unsafe.Add(ref length, 6);
                flattenedIndex += index7 - Unsafe.Add(ref bound, 6);
                flattenedIndex *= Unsafe.Add(ref length, 7);
                flattenedIndex += index8 - Unsafe.Add(ref bound, 7);
                flattenedIndex *= Unsafe.Add(ref length, 8);
                flattenedIndex += index9 - Unsafe.Add(ref bound, 8);
                flattenedIndex *= Unsafe.Add(ref length, 9);
                flattenedIndex += index10 - Unsafe.Add(ref bound, 9);
                flattenedIndex *= Unsafe.Add(ref length, 10);
                flattenedIndex += index11 - Unsafe.Add(ref bound, 10);
                flattenedIndex *= Unsafe.Add(ref length, 11);
                flattenedIndex += index12 - Unsafe.Add(ref bound, 11);
                flattenedIndex *= Unsafe.Add(ref length, 12);
                flattenedIndex += index13 - Unsafe.Add(ref bound, 12);
                flattenedIndex *= Unsafe.Add(ref length, 13);
                flattenedIndex += index14 - Unsafe.Add(ref bound, 13);
                flattenedIndex *= Unsafe.Add(ref length, 14);
                flattenedIndex += index15 - Unsafe.Add(ref bound, 14);
                flattenedIndex *= Unsafe.Add(ref length, 15);
                flattenedIndex += index16 - Unsafe.Add(ref bound, 15);
                flattenedIndex *= Unsafe.Add(ref length, 16);
                flattenedIndex += index17 - Unsafe.Add(ref bound, 16);
                flattenedIndex *= Unsafe.Add(ref length, 17);
                flattenedIndex += index18 - Unsafe.Add(ref bound, 17);
                flattenedIndex *= Unsafe.Add(ref length, 18);
                flattenedIndex += index19 - Unsafe.Add(ref bound, 18);
                flattenedIndex *= Unsafe.Add(ref length, 19);
                flattenedIndex += index20 - Unsafe.Add(ref bound, 19);
                flattenedIndex *= Unsafe.Add(ref length, 20);
                flattenedIndex += index21 - Unsafe.Add(ref bound, 20);
                flattenedIndex *= Unsafe.Add(ref length, 21);
                flattenedIndex += index22 - Unsafe.Add(ref bound, 21);
                flattenedIndex *= Unsafe.Add(ref length, 22);
                flattenedIndex += index23 - Unsafe.Add(ref bound, 22);
                flattenedIndex *= Unsafe.Add(ref length, 23);
                flattenedIndex += index24 - Unsafe.Add(ref bound, 23);
                flattenedIndex *= Unsafe.Add(ref length, 24);
                flattenedIndex += index25 - Unsafe.Add(ref bound, 24);
                flattenedIndex *= Unsafe.Add(ref length, 25);
                flattenedIndex += index26 - Unsafe.Add(ref bound, 25);
                flattenedIndex *= Unsafe.Add(ref length, 26);
                flattenedIndex += index27 - Unsafe.Add(ref bound, 26);
                flattenedIndex *= Unsafe.Add(ref length, 27);
                flattenedIndex += index28 - Unsafe.Add(ref bound, 27);
                flattenedIndex *= Unsafe.Add(ref length, 28);
                flattenedIndex += index29 - Unsafe.Add(ref bound, 28);
                flattenedIndex *= Unsafe.Add(ref length, 29);
                flattenedIndex += index30 - Unsafe.Add(ref bound, 29);
                flattenedIndex *= Unsafe.Add(ref length, 30);
                flattenedIndex += index31 - Unsafe.Add(ref bound, 30);
                for (int i = 31; i < _rank; i++)
                {
                    flattenedIndex *= Unsafe.Add(ref length, i);
                }
                flattenedIndex += index32 - Unsafe.Add(ref bound, 31);
                return ref Unsafe.Add(ref GetReference(), (nuint)flattenedIndex);
            }
        }
        #region Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2) => this[index1, index2];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2) => this[index1, index2] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3) => this[index1, index2, index3];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3) => this[index1, index2, index3] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4) => this[index1, index2, index3, index4];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4) => this[index1, index2, index3, index4] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5) => this[index1, index2, index3, index4, index5];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5) => this[index1, index2, index3, index4, index5] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5) => this[index1, index2, index3, index4, index5];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5) => this[index1, index2, index3, index4, index5] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5) => this[index1, index2, index3, index4, index5];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5) => this[index1, index2, index3, index4, index5] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5) => this[index1, index2, index3, index4, index5];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5) => this[index1, index2, index3, index4, index5] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6) => this[index1, index2, index3, index4, index5, index6];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6) => this[index1, index2, index3, index4, index5, index6] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6) => this[index1, index2, index3, index4, index5, index6];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6) => this[index1, index2, index3, index4, index5, index6] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6) => this[index1, index2, index3, index4, index5, index6];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6) => this[index1, index2, index3, index4, index5, index6] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6) => this[index1, index2, index3, index4, index5, index6];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6) => this[index1, index2, index3, index4, index5, index6] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7) => this[index1, index2, index3, index4, index5, index6, index7];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7) => this[index1, index2, index3, index4, index5, index6, index7] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7) => this[index1, index2, index3, index4, index5, index6, index7];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7) => this[index1, index2, index3, index4, index5, index6, index7] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7) => this[index1, index2, index3, index4, index5, index6, index7];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7) => this[index1, index2, index3, index4, index5, index6, index7] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7) => this[index1, index2, index3, index4, index5, index6, index7];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7) => this[index1, index2, index3, index4, index5, index6, index7] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8) => this[index1, index2, index3, index4, index5, index6, index7, index8];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8) => this[index1, index2, index3, index4, index5, index6, index7, index8] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8) => this[index1, index2, index3, index4, index5, index6, index7, index8];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8) => this[index1, index2, index3, index4, index5, index6, index7, index8] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8) => this[index1, index2, index3, index4, index5, index6, index7, index8];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8) => this[index1, index2, index3, index4, index5, index6, index7, index8] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8) => this[index1, index2, index3, index4, index5, index6, index7, index8];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8) => this[index1, index2, index3, index4, index5, index6, index7, index8] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29, int index30) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29, int index30) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29, uint index30) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29, uint index30) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29, long index30) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29, long index30) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29, ulong index30) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29, ulong index30) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29, int index30, int index31) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29, int index30, int index31) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29, uint index30, uint index31) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29, uint index30, uint index31) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29, long index30, long index31) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29, long index30, long index31) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29, ulong index30, ulong index31) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29, ulong index30, ulong index31) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29, int index30, int index31, int index32) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31, index32];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, int index1, int index2, int index3, int index4, int index5, int index6, int index7, int index8, int index9, int index10, int index11, int index12, int index13, int index14, int index15, int index16, int index17, int index18, int index19, int index20, int index21, int index22, int index23, int index24, int index25, int index26, int index27, int index28, int index29, int index30, int index31, int index32) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31, index32] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29, uint index30, uint index31, uint index32) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31, index32];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, uint index1, uint index2, uint index3, uint index4, uint index5, uint index6, uint index7, uint index8, uint index9, uint index10, uint index11, uint index12, uint index13, uint index14, uint index15, uint index16, uint index17, uint index18, uint index19, uint index20, uint index21, uint index22, uint index23, uint index24, uint index25, uint index26, uint index27, uint index28, uint index29, uint index30, uint index31, uint index32) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31, index32] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29, long index30, long index31, long index32) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31, index32];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, long index1, long index2, long index3, long index4, long index5, long index6, long index7, long index8, long index9, long index10, long index11, long index12, long index13, long index14, long index15, long index16, long index17, long index18, long index19, long index20, long index21, long index22, long index23, long index24, long index25, long index26, long index27, long index28, long index29, long index30, long index31, long index32) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31, index32] = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29, ulong index30, ulong index31, ulong index32) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31, index32];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, ulong index1, ulong index2, ulong index3, ulong index4, ulong index5, ulong index6, ulong index7, ulong index8, ulong index9, ulong index10, ulong index11, ulong index12, ulong index13, ulong index14, ulong index15, ulong index16, ulong index17, ulong index18, ulong index19, ulong index20, ulong index21, ulong index22, ulong index23, ulong index24, ulong index25, ulong index26, ulong index27, ulong index28, ulong index29, ulong index30, ulong index31, ulong index32) => this[index1, index2, index3, index4, index5, index6, index7, index8, index9, index10, index11, index12, index13, index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26, index27, index28, index29, index30, index31, index32] = value;
        #endregion
    }
    #endregion
}
