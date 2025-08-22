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
            .Where(parameter => parameter.Type == typeof(T));
}
