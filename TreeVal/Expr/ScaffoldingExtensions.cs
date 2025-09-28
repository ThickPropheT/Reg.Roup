using System.Linq.Expressions;
using TreeVal.Scaffolding;
using TreeVal.Stage.Eval.Equals;

namespace TreeVal.Expr;

public static class ScaffoldingExtensions
{
    public static IVisitorBuilder<Expression> AnyOne(this IVisitorBuilderFactory factory)
        => factory.OfType<Expression>();

    public static IVisitorBuilder OfType(this IVisitorBuilderFactory factory, ExpressionType nodeType)
        => factory
            // TODO find a way to be rid of Expression
            .OfType<Expression>()
            .Equals(nodeType, e => e.NodeType);

    public static IVisitorBuilder<T> OfType<T>(this IVisitorBuilderFactory factory, ExpressionType nodeType)
        where T : Expression
        => factory
            .OfType<T>()
            .Equals(nodeType, e => e.NodeType);
}
