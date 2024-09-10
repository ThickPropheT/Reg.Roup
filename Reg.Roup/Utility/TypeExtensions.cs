using System;

namespace Reg.Roup.Utility
{
    internal static class TypeExtensions
    {
        public static bool CanBeAssignedNull(this Type type)
            => !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
    }
}