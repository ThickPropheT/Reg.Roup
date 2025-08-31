using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class ConstantEvaluatorExtensions
{
    public static IEvaluatorConditionBuilder<ConstantExpression> Constant(this IVisitorNodeFactory factory)
        => factory.OfType<ConstantExpression>();

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant(
        this IVisitorNodeFactory factory, EValue<object?>? eValue)
        => factory
            .OfType<ConstantExpression>()
            .When(eValue).IsNotNull()
            .Then((node, value) =>
                node.Equals(value, constant => constant.Value));

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant(this IVisitorNodeFactory factory, EType type)
        => factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type);

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant(
        this IVisitorNodeFactory factory, EType type, EValue<object?>? eValue)
        => factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type)
            .When(eValue).IsNotNull()
            .Then((node, value) =>
                node.Equals(value, constant => constant.Value));

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant<T>(this IVisitorNodeFactory factory)
    {
        EType type = typeof(T);

        return factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type);
    }

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant<T>(
        this IVisitorNodeFactory factory, EValue<T?>? eValue)
    {
        EType type = typeof(T);
        eValue ??= EValue<T>.Null();

        return factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type)
            .When(eValue).IsNotNull()
            .Then((node, value) =>
                node.Equals(value, constant => constant.Value));
    }
}
