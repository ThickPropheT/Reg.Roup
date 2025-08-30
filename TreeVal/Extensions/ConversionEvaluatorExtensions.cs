using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class ConversionEvaluatorExtensions
{
    public static IEvaluatorConditionBuilder<UnaryExpression> Cast(this IVisitorNodeFactory factory)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .AcceptChildren();

    public static IEvaluatorConditionBuilder<UnaryExpression> Cast(
        this IVisitorNodeFactory factory, IEvaluatorNodeFactory operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .HavingChild(operand);

    public static IEvaluatorConditionBuilder<UnaryExpression> Cast<T>(
        this IVisitorNodeFactory factory, IEvaluatorNodeFactory operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Equals(typeof(T), cast => cast.Type)
            .HavingChild(operand);

    public static IEvaluatorConditionBuilder<UnaryExpression> Cast(
        this IVisitorNodeFactory factory, IEvaluatorNodeFactory operand, Type toType)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Where(cast => cast.Type == toType)
            .HavingChild(operand);
}
