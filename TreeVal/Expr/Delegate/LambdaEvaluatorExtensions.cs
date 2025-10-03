using System.Linq.Expressions;
using TreeVal.Scaffolding;
using TreeVal.Stage.Children.HavingChildren;
using TreeVal.Stage.Eval.Equals;

namespace TreeVal.Expr.Delegate;

public static class LambdaEvaluatorExtensions
{
    public static IVisitorBuilder<LambdaExpression> Lambda(
        this IVisitorBuilderFactory factory, IVisitorBuilder body)
        => factory
            .OfType<LambdaExpression>()
            .HavingChild(body);

    public static IVisitorBuilder<LambdaExpression> Lambda(
        this IVisitorBuilderFactory factory, IVisitorBuilder[] parameters, IVisitorBuilder body)
        => factory
            .OfType<LambdaExpression>()
            .HavingChildren(new[] { body }.Concat(parameters).ToArray());

    public static IVisitorBuilder<ParameterExpression> Parameter<T>(this IVisitorBuilderFactory factory)
        => factory
            .OfType<ParameterExpression>()
            .Equals(typeof(T), parameter => parameter.Type);

    public static IVisitorBuilder<ParameterExpression> Parameter<T>(this IVisitorBuilderFactory factory,
        string? name)
        => factory
            .OfType<ParameterExpression>()
            .Equals(typeof(T), parameter => parameter.Type)
            .Equals(name, parameter => parameter.Name);
}
