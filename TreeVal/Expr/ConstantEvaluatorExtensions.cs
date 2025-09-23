using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Primitives;
using TreeVal.Scaffolding;

namespace TreeVal.Expr;

public static class ConstantEvaluatorExtensions
{
    public static IEvaluatorBuilder<ConstantExpression> Constant(this IEvaluatorBuilderFactory factory)
        => factory.OfType<ConstantExpression>();

    public static IEvaluatorBuilder<ConstantExpression> Constant(
        this IEvaluatorBuilderFactory factory, EValue<object?>? value)
        => factory
            .OfType<ConstantExpression>()
            .Case((@case, _) =>
            [
                @case.When(value)
                    .IsNotNull()
                    // ReSharper disable once VariableHidesOuterVariable
                    .Then((node, value) => node.Equals(value, constant => constant.Value))
            ]);

    public static IEvaluatorBuilder<ConstantExpression> Constant(this IEvaluatorBuilderFactory factory, EType type)
        => factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type);

    public static IEvaluatorBuilder<ConstantExpression> Constant(
        this IEvaluatorBuilderFactory factory, EType type, EValue<object?>? value)
        => factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type)
            .Case((@case, _) =>
            [
                @case.When(value)
                    .IsNotNull()
                    // ReSharper disable once VariableHidesOuterVariable
                    .Then((node, value) => node.Equals(value, constant => constant.Value))
            ]);

    public static IEvaluatorBuilder<ConstantExpression> Constant<T>(this IEvaluatorBuilderFactory factory)
    {
        EType type = typeof(T);

        return factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type);
    }

    public static IEvaluatorBuilder<ConstantExpression> Constant<T>(
        this IEvaluatorBuilderFactory factory, EValue<T?>? value)
    {
        EType type = typeof(T);
        value ??= EValue<T>.Null();

        return factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type)
            .Case((@case, _) =>
            [
                @case.When(value)
                    .IsNotNull()
                    // ReSharper disable once VariableHidesOuterVariable
                    .Then((node, value) => node.Equals(value, constant => constant.Value))
            ]);
    }
}
