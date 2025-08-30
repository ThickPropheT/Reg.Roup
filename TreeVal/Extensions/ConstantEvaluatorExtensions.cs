using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class ConstantEvaluatorExtensions
{
    public static IEvaluatorConditionBuilder<ConstantExpression> Constant(this IVisitorNodeFactory factory)
        => factory.OfType<ConstantExpression>();

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant(
        this IVisitorNodeFactory factory, EValue<object?>? value)
        => factory
            .OfType<ConstantExpression>()
            .Equals(constant => constant.Value, value);

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant(this IVisitorNodeFactory factory, EType type)
        => factory
            .OfType<ConstantExpression>()
            .Where(constant => type.IsEqualTo(constant.Type));

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant(
        this IVisitorNodeFactory factory, EType type, EValue<object?>? value)
        => factory
            .OfType<ConstantExpression>()
            .Where(constant => type.IsEqualTo(constant.Type))
            .Equals(constant => constant.Value, value);

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant<T>(this IVisitorNodeFactory factory)
    {
        EType type = typeof(T);

        return factory
            .OfType<ConstantExpression>()
            .Where(constant => type.IsEqualTo(constant.Type));
    }

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant<T>(
        this IVisitorNodeFactory factory, EValue<T?>? value)
    {
        EType type = typeof(T);
        value ??= EValue<T>.Null();

        return factory
            .OfType<ConstantExpression>()
            .Where(constant => type.IsEqualTo(constant.Type))
            .Equals(constant => constant.Value, value);
    }
}
