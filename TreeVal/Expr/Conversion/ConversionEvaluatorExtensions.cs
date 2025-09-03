using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Eval.AcceptChildren;
using TreeVal.Scaffolding;

namespace TreeVal.Expr.Conversion;

public static class ConversionEvaluatorExtensions
{
    public static IEvaluatorBuilder<UnaryExpression> Cast(this IEvaluatorBuilderFactory factory)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .AcceptChildren();

    public static IEvaluatorBuilder<UnaryExpression> Cast(
        this IEvaluatorBuilderFactory factory, INodeEvaluatorFactory operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .HavingChild(operand);

    public static IEvaluatorBuilder<UnaryExpression> Cast<T>(
        this IEvaluatorBuilderFactory factory, INodeEvaluatorFactory operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Equals(typeof(T), cast => cast.Type)
            .HavingChild(operand);

    public static IEvaluatorBuilder<UnaryExpression> Cast(
        this IEvaluatorBuilderFactory factory, INodeEvaluatorFactory operand, Type toType)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Where(cast => cast.Type == toType)
            .HavingChild(operand);
}
