using System.Linq.Expressions;
using TreeVal.Eval;
using TreeVal.Extensions;
using TreeVal.Scaffolding;

namespace TreeVal.Expr.Delegate;

public static class LambdaEvaluatorExtensions
{
    public static IEvaluatorBuilder<LambdaExpression> Lambda(
        this IEvaluatorBuilderFactory factory, INodeEvaluatorFactory body)
        => factory
            .OfType<LambdaExpression>()
            .HavingChild(body);

    public static IEvaluatorBuilder<LambdaExpression> Lambda(
        this IEvaluatorBuilderFactory factory, INodeEvaluatorFactory[] parameters, INodeEvaluatorFactory body)
        => factory
            .OfType<LambdaExpression>()
            .HavingChildren(new[] { body }.Concat(parameters).ToArray());

    public static IEvaluatorBuilder<ParameterExpression> Parameter<T>(this IEvaluatorBuilderFactory factory)
        => factory
            .OfType<ParameterExpression>()
            .Equals(typeof(T), parameter => parameter.Type);

    public static IEvaluatorBuilder<ParameterExpression> Parameter<T>(this IEvaluatorBuilderFactory factory, string? name)
        => factory
            .OfType<ParameterExpression>()
            .Equals(typeof(T), parameter => parameter.Type)
            .Equals(name, parameter => parameter.Name);
}
