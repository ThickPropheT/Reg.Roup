using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Scaffolding;

namespace TreeVal.Expr;

public static class OfTypeEvaluatorExtensions
{
    public static IEvaluatorBuilder OfType(this IEvaluatorBuilderFactory factory, ExpressionType nodeType)
        => factory
            .OfType<Expression>()
            .Equals(nodeType, e => e.NodeType);

    public static IEvaluatorBuilder<T> OfType<T>(this IEvaluatorBuilderFactory factory, ExpressionType nodeType)
        where T : Expression
        => factory
            .OfType<T>()
            .Equals(nodeType, e => e.NodeType);
}
