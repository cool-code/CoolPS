#if !NET7_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
#if NETFRAMEWORK || !NET5_0_OR_GREATER
    internal static class IsExternalInit {}
#endif
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    internal sealed class CompilerFeatureRequiredAttribute : Attribute
    {
        public CompilerFeatureRequiredAttribute(string featureName) => FeatureName = featureName;
        public string FeatureName { get; }
        public bool IsOptional { get; init; }
        public const string RefStructs = nameof(RefStructs);
        public const string RequiredMembers = nameof(RequiredMembers);
        public const string GenericMath = nameof(GenericMath);
    }
}
#endif
