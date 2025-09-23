using System.Linq.Expressions;
using TreeVal.Scaffolding;

namespace TreeVal.Expr;

public static class ExpressionEvaluatorExtensions
{
    public static IVisitorBuilder<Expression> AnyOne(this IVisitorBuilderFactory factory)
        => factory.OfType<Expression>();
}
