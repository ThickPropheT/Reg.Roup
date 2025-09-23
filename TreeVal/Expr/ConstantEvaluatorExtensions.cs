using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Primitives;
using TreeVal.Scaffolding;

namespace TreeVal.Expr;

public static class ConstantEvaluatorExtensions
{
    public static IVisitorBuilder<ConstantExpression> Constant(this IVisitorBuilderFactory factory)
        => factory.OfType<ConstantExpression>();

    public static IVisitorBuilder<ConstantExpression> Constant(
        this IVisitorBuilderFactory factory, EValue<object?>? value)
        => factory
            .OfType<ConstantExpression>()
            .Case((@case, _) =>
            [
                @case.When(value)
                    .IsNotNull()
                    // ReSharper disable once VariableHidesOuterVariable
                    .Then((node, value) => node.Equals(value, constant => constant.Value))
            ]);

    public static IVisitorBuilder<ConstantExpression> Constant(this IVisitorBuilderFactory factory, EType type)
        => factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type);

    public static IVisitorBuilder<ConstantExpression> Constant(
        this IVisitorBuilderFactory factory, EType type, EValue<object?>? value)
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

    public static IVisitorBuilder<ConstantExpression> Constant<T>(this IVisitorBuilderFactory factory)
    {
        EType type = typeof(T);

        return factory
            .OfType<ConstantExpression>()
            .Equals(type, constant => constant.Type);
    }

    public static IVisitorBuilder<ConstantExpression> Constant<T>(
        this IVisitorBuilderFactory factory, EValue<T?>? value)
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
