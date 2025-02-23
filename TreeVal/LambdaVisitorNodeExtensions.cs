using System.Linq.Expressions;

namespace TreeVal;

public static class LambdaVisitorNodeExtensions
{
    public static IVisitorNode<LambdaExpression> Lambda(
        // TODO consider changing parameters to IVisitorNode<ParameterExpression>[]
        this IVisitorNodeFactory factory, IVisitorNode[] parameters, IVisitorNode body)
        => factory
            .OfType<LambdaExpression>()
            .HavingChildren(new[] {body}.Concat(parameters).ToArray());

    public static IVisitorNode<ParameterExpression> Parameter<T>(this IVisitorNodeFactory factory)
        => factory
            .OfType<ParameterExpression>()
            .Where(parameter => parameter.Type == typeof(T));
}
