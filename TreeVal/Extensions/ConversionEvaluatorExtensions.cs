using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class ConversionEvaluatorExtensions
{
    public static IEvaluatorConditionBuilder<UnaryExpression> Cast(this VisitorNodeFactory factory)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .AcceptChildren();
    
    public static IEvaluatorConditionBuilder<UnaryExpression> Cast(this VisitorNodeFactory factory, IEvaluatorNodeFactory operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .HavingChild(operand);
    
    public static IEvaluatorConditionBuilder<UnaryExpression> Cast<T>(this VisitorNodeFactory factory, IEvaluatorNodeFactory operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Where(cast => cast.Type == typeof(T))
            .HavingChild(operand);
    
    public static IEvaluatorConditionBuilder<UnaryExpression> Cast(this VisitorNodeFactory factory, IEvaluatorNodeFactory operand, Type toType)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Where(cast => cast.Type == toType)
            .HavingChild(operand);
}
