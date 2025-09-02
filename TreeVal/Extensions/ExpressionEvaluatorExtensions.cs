using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class ExpressionEvaluatorExtensions
{
    public static IEvaluatorBuilder<Expression> AnyOne(this IVisitorNodeFactory factory)
        => factory.OfType<Expression>();
}
