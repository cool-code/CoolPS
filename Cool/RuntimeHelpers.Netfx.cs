#if NETFRAMEWORK
using System;
using System.Reflection;
namespace Cool;

public static class RuntimeHelpersExtensions
{
    private static bool IsReferenceOrContainsReferencesCore(Type type)
    {
        if (type.GetTypeInfo().IsPrimitive) return false;

        if (!type.GetTypeInfo().IsValueType) return true;

        // If type is a Nullable<> of something, unwrap it first.
        Type? underlyingNullable = Nullable.GetUnderlyingType(type);
        if (underlyingNullable != null)
            type = underlyingNullable;

        if (type.GetTypeInfo().IsEnum) return false;

        foreach (FieldInfo field in type.GetTypeInfo().DeclaredFields)
        {
            if (field.IsStatic) continue;
            if (IsReferenceOrContainsReferencesCore(field.FieldType)) return true;
        }
        return false;
    }

    private static class PerTypeValues<T>
    {
        public static readonly bool IsReferenceOrContainsReferences = IsReferenceOrContainsReferencesCore(typeof(T));
    }

    extension(System.Runtime.CompilerServices.RuntimeHelpers) {
        public static bool IsReferenceOrContainsReferences<T>() => PerTypeValues<T>.IsReferenceOrContainsReferences;
    }
}
#endif