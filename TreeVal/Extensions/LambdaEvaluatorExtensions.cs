using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class LambdaEvaluatorExtensions
{
    public static IEvaluatorBuilder<LambdaExpression> Lambda(
        this IVisitorNodeFactory factory, IEvaluatorNodeFactory[] parameters, IEvaluatorNodeFactory body)
        => factory
            .OfType<LambdaExpression>()
            .HavingChildren(new[] {body}.Concat(parameters).ToArray());

    public static IEvaluatorBuilder<ParameterExpression> Parameter<T>(this IVisitorNodeFactory factory)
        => factory
            .OfType<ParameterExpression>()
            .Equals(parameter => parameter.Type, typeof(T));

    public static IEvaluatorBuilder<ParameterExpression> Parameter<T>(this IVisitorNodeFactory factory, string? name)
        => factory
            .OfType<ParameterExpression>()
            .Equals(parameter => parameter.Type, typeof(T))
            .Equals(parameter => parameter.Name, name);
}
