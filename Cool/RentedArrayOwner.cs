using System;
using System.Runtime.CompilerServices;

namespace Cool;

[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public struct RentedArrayOwner<T>(int minimumLength, bool clearOnReturn = false) : IDisposable
{
    // store the array and the clear-on-return flag directly on the stack
    public T[]? Array = ArrayPool<T>.Shared.Rent(minimumLength);
    private readonly bool _clearOnReturn = clearOnReturn;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        // get the array reference and clear the struct to prevent double return
        T[]? toReturn = Array;
        if (toReturn != null)
        {
            Array = null;
            ArrayPool<T>.Shared.Return(toReturn, _clearOnReturn);
        }
    }
}