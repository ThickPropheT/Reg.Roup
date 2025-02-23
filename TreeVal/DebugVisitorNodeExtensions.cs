using System.Linq.Expressions;

namespace TreeVal;

public static class DebugVisitorNodeExtensions
{
    public static IVisitorNode Debug(this IVisitorNodeFactory factory, Action<Expression> observe)
        => factory
            .OfType<Expression>()
            .Where(e =>
            {
                observe(e);
                return false;
            });
}
