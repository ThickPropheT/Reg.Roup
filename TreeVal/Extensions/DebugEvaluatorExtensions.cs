using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class DebugEvaluatorExtensions
{
    public static IEvaluatorBuilder Debug(this IVisitorNodeFactory factory, Action<Expression> observe)
        => factory
            .OfType<Expression>()
            .Where(e =>
            {
                observe(e);
                return false;
            });
}
