using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Eval.AcceptChildren;
using TreeVal.Scaffolding;

namespace TreeVal.Expr.Conversion;

public static class ConversionEvaluatorExtensions
{
    public static IVisitorBuilder<UnaryExpression> Cast(this IVisitorBuilderFactory factory)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .HavingChild(parent => factory.AcceptChildren(parent));

    public static IVisitorBuilder<UnaryExpression> Cast(
        this IVisitorBuilderFactory factory, IVisitorFactory operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .HavingChild(operand);

    public static IVisitorBuilder<UnaryExpression> Cast<T>(
        this IVisitorBuilderFactory factory, IVisitorFactory operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Equals(typeof(T), cast => cast.Type)
            .HavingChild(operand);

    public static IVisitorBuilder<UnaryExpression> Cast(
        this IVisitorBuilderFactory factory, IVisitorFactory operand, Type toType)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Where(cast => cast.Type == toType)
            .HavingChild(operand);
}
