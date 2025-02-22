using System;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation._RecycleBin;

public static class ConversionVisitorNodeExtensions
{
    public static IVisitorNode<UnaryExpression> Cast(this IVisitorNodeFactory factory)
        => factory.OfType<UnaryExpression>(ExpressionType.Convert);
    
    public static IVisitorNode<UnaryExpression> Cast(this IVisitorNodeFactory factory, IVisitorNode operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .HavingChild(operand);
    
    public static IVisitorNode<UnaryExpression> Cast<T>(this IVisitorNodeFactory factory, IVisitorNode operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Where(cast => cast.Type == typeof(T))
            .HavingChild(operand);
    
    public static IVisitorNode<UnaryExpression> Cast(this IVisitorNodeFactory factory, Type toType, IVisitorNode operand)
        => factory
            .OfType<UnaryExpression>(ExpressionType.Convert)
            .Where(cast => cast.Type == toType)
            .HavingChild(operand);
}
