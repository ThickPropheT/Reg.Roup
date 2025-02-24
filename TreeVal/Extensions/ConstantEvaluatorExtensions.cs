using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class ConstantEvaluatorExtensions
{
    public static IEvaluatorBuilder<ConstantExpression> Constant(this VisitorNodeFactory factory)
        => factory.OfType<ConstantExpression>();
    
    public static IEvaluatorBuilder<ConstantExpression> Constant(this VisitorNodeFactory factory, EValue<object?>? value)
        => factory
            .OfType<ConstantExpression>()
            .Where(constant => value?.IsEqualTo(constant.Value) != false);
    
    public static IEvaluatorBuilder<ConstantExpression> Constant(this VisitorNodeFactory factory, EType type)
        => factory
            .OfType<ConstantExpression>()
            .Where(constant => type.IsEqualTo(constant.Type));
    
    public static IEvaluatorBuilder<ConstantExpression> Constant(this VisitorNodeFactory factory, EType type, EValue<object?>? value)
        => factory
            .OfType<ConstantExpression>()
            .Where(constant => type.IsEqualTo(constant.Type))
            .Where(constant => value?.IsEqualTo(constant.Value) != false);
    
    public static IEvaluatorBuilder<ConstantExpression> Constant<T>(this VisitorNodeFactory factory)
    {
        EType type = typeof(T);
        
        return factory
            .OfType<ConstantExpression>()
            .Where(constant => type.IsEqualTo(constant.Type));
    }

    public static IEvaluatorBuilder<ConstantExpression> Constant<T>(this VisitorNodeFactory factory, EValue<T?>? value)
    {
        EType type = typeof(T);
        value ??= EValue<T>.Null();
        
        return factory
            .OfType<ConstantExpression>()
            .Where(constant => type.IsEqualTo(constant.Type))
            .Where(constant => value.IsEqualTo(constant.Value));
    }
}
