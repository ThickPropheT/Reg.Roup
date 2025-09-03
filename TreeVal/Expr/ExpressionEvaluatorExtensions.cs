using System.Linq.Expressions;
using TreeVal.Scaffolding;

namespace TreeVal.Expr;

public static class ExpressionEvaluatorExtensions
{
    public static IEvaluatorBuilder<Expression> AnyOne(this IEvaluatorBuilderFactory factory)
        => factory.OfType<Expression>();
}
