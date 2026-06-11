namespace Cool;

public static partial class BitSet
{
    public interface IAllocator
    {
        nuint BitHighLimit { get; }
        ref nuint GetReference();
        void Init(nuint bitHighLimit);
    }
}