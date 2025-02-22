using System.Linq.Expressions;

namespace Treeval;

public static class ConstantVisitorNodeExtensions
{
    public static IVisitorNode<ConstantExpression> Constant(this IVisitorNodeFactory factory)
        => factory
            .OfType<ConstantExpression>();
    
    public static IVisitorNode<ConstantExpression> Constant(this IVisitorNodeFactory factory, object? value)
        => factory
            .OfType<ConstantExpression>()
            .Where(constant => constant.Value == value);
    
    public static IVisitorNode<ConstantExpression> Constant<T>(this IVisitorNodeFactory factory)
        => factory
            .OfType<ConstantExpression>()
            .Where(constant => constant.Type == typeof(T));
}
