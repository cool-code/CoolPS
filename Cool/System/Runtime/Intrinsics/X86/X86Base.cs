#if NETFRAMEWORK
using System.Runtime.CompilerServices;

namespace System.Runtime.Intrinsics.X86;

public abstract partial class X86Base
{
    internal X86Base() { }

    public static bool IsSupported
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => true;
    }

    public abstract class X64
    {
        internal X64() { }
        public static unsafe bool IsSupported
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => sizeof(IntPtr) == 8;
        }
    }
}
#endif