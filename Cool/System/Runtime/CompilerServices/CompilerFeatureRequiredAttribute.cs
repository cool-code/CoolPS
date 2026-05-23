#if NETFRAMEWORK
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit;
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    internal sealed class CompilerFeatureRequiredAttribute(string featureName) : Attribute
    {
        public string FeatureName { get; } = featureName;
        public bool IsOptional { get; init; }
        public const string RefStructs = nameof(RefStructs);
        public const string RequiredMembers = nameof(RequiredMembers);
        public const string GenericMath = nameof(GenericMath);
    }
}
#endif