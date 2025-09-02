using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class LambdaEvaluatorExtensions
{
    public static IEvaluatorBuilder<LambdaExpression> Lambda(
        this IVisitorNodeFactory factory, INodeEvaluatorFactory body)
        => factory
            .OfType<LambdaExpression>()
            .HavingChild(body);

    public static IEvaluatorBuilder<LambdaExpression> Lambda(
        this IVisitorNodeFactory factory, INodeEvaluatorFactory[] parameters, INodeEvaluatorFactory body)
        => factory
            .OfType<LambdaExpression>()
            .HavingChildren(new[] { body }.Concat(parameters).ToArray());

    public static IEvaluatorBuilder<ParameterExpression> Parameter<T>(this IVisitorNodeFactory factory)
        => factory
            .OfType<ParameterExpression>()
            .Equals(typeof(T), parameter => parameter.Type);

    public static IEvaluatorBuilder<ParameterExpression> Parameter<T>(this IVisitorNodeFactory factory, string? name)
        => factory
            .OfType<ParameterExpression>()
            .Equals(typeof(T), parameter => parameter.Type)
            .Equals(name, parameter => parameter.Name);
}
