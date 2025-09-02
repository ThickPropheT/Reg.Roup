using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class OfTypeEvaluatorExtensions
{
    public static IEvaluatorBuilder OfType(this IVisitorNodeFactory factory, ExpressionType nodeType)
        => factory
            .OfType<Expression>()
            .Equals(nodeType, e => e.NodeType);

    public static IEvaluatorBuilder<T> OfType<T>(this IVisitorNodeFactory factory, ExpressionType nodeType)
        where T : Expression
        => factory
            .OfType<T>()
            .Equals(nodeType, e => e.NodeType);
}
