using System.Reflection;
using System.Runtime.CompilerServices;

namespace TreeVal.Extensions;

public static class MethodInfoExtensions
{
    public static bool IsExtensionMethod(this MethodInfo method)
        => method.IsDefined(typeof(ExtensionAttribute), true);
}
