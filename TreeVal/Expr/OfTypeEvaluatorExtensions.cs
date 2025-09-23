using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Scaffolding;

namespace TreeVal.Expr;

public static class OfTypeEvaluatorExtensions
{
    public static IVisitorBuilder OfType(this IVisitorBuilderFactory factory, ExpressionType nodeType)
        => factory
            .OfType<Expression>()
            .Equals(nodeType, e => e.NodeType);

    public static IVisitorBuilder<T> OfType<T>(this IVisitorBuilderFactory factory, ExpressionType nodeType)
        where T : Expression
        => factory
            .OfType<T>()
            .Equals(nodeType, e => e.NodeType);
}
