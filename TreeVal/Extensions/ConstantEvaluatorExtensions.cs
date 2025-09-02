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
            .When(value).IsNotNull()
            // ReSharper disable once VariableHidesOuterVariable
            .Then((node, value) =>
                node.Equals(value, constant => constant.Value));

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant(this IVisitorNodeFactory factory, EType type)
        => factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type);

    public static IEvaluatorConditionBuilder<ConstantExpression> Constant(
        this IVisitorNodeFactory factory, EType type, EValue<object?>? value)
        => factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type)
            .When(value).IsNotNull()
            // ReSharper disable once VariableHidesOuterVariable
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
        this IVisitorNodeFactory factory, EValue<T?>? value)
    {
        EType type = typeof(T);
        value ??= EValue<T>.Null();

        return factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type)
            .When(value).IsNotNull()
            // ReSharper disable once VariableHidesOuterVariable
            .Then((node, value) =>
                node.Equals(value, constant => constant.Value));
    }
}
