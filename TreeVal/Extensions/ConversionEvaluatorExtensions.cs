using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class ConversionEvaluatorExtensions
{
    public static IEvaluatorBuilder<UnaryExpression> Cast(this IVisitorNodeFactory factory)
        => factory.OfType<UnaryExpression>(ExpressionType.Convert);
    
    public static IEvaluatorBuilder<UnaryExpression> Cast(this IVisitorNodeFactory factory, IEvaluatorBuilder operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .HavingChild(operand);
    
    public static IEvaluatorBuilder<UnaryExpression> Cast<T>(this IVisitorNodeFactory factory, IEvaluatorBuilder operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Where(cast => cast.Type == typeof(T))
            .HavingChild(operand);
    
    public static IEvaluatorBuilder<UnaryExpression> Cast(this IVisitorNodeFactory factory, IEvaluatorBuilder operand, Type toType)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Where(cast => cast.Type == toType)
            .HavingChild(operand);
}
