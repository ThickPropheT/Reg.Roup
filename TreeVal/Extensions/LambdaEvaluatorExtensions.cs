using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class LambdaEvaluatorExtensions
{
    public static IEvaluatorBuilder<LambdaExpression> Lambda(
        // TODO consider changing parameters to IVisitorNode<ParameterExpression>[]
        this VisitorNodeFactory factory, IEvaluatorBuilder[] parameters, IEvaluatorBuilder body)
        => factory
            .OfType<LambdaExpression>()
            .HavingChildren(new[] {body}.Concat(parameters).ToArray());

    public static IEvaluatorBuilder<ParameterExpression> Parameter<T>(this VisitorNodeFactory factory)
        => factory
            .OfType<ParameterExpression>()
            .Where(parameter => parameter.Type == typeof(T));
}
