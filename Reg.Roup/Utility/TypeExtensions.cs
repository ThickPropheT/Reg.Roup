using System;

namespace Reg.Roup.Utility
{
    internal static class TypeExtensions
    {
        public static bool CanBeAssignedNull(this Type type)
            => !type.IsValueType || Nullable.GetUnderlyingType(type) != null;

        public static bool IsNullable(this Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        public static Type? TryGetNullableType(this Type type)
            => IsNullable(type)
                ? Nullable.GetUnderlyingType(type)
                : null;
    }
}