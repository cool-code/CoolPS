using System;
using System.Runtime.CompilerServices;

namespace Cool;

public static partial class Unchecked
{
    private delegate string AllocateDelegate(int length);
    private delegate string AllocateNIntDelegate(nint length);
    private static readonly AllocateDelegate _stringAllocator = CreateStringAllocator();

    private static AllocateDelegate CreateStringAllocator()
    {
        var method = typeof(string).GetMethod("FastAllocateString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, null, [typeof(int)], null);
        if (method != null) return (AllocateDelegate)Delegate.CreateDelegate(typeof(AllocateDelegate), method);
        method = typeof(string).GetMethod("FastAllocateString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, null, [typeof(nint)], null);
        if (method != null)
        {
            AllocateNIntDelegate? nintAllocator = (AllocateNIntDelegate)Delegate.CreateDelegate(typeof(AllocateNIntDelegate), method);
            return Unsafe.As<AllocateNIntDelegate, AllocateDelegate>(ref nintAllocator);
        }
        return len => new string('\0', len);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string FastAllocateString(int length) => _stringAllocator(length);
}