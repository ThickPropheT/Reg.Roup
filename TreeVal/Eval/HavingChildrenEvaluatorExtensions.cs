using TreeVal.Eval.AcceptChildren;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

// TODO test coverage
public static class HavingChildrenEvaluatorExtensions
{
    private static readonly DefaultEvaluatorBuilderFactory Factory = new();

    public static IEvaluatorBuilder HavingChild(
        this IEvaluatorBuilder builder, INodeEvaluatorFactory child)
    {
        builder.AddChildren(_ => [child]);
        return builder;
    }

    public static IEvaluatorBuilder HavingChildren(
        this IEvaluatorBuilder builder, params INodeEvaluatorFactory[] children)
    {
        builder.AddChildren(_ => children);
        return builder;
    }

    public static IEvaluatorBuilder<T> HavingChild<T>(
        this IEvaluatorBuilder<T> builder, INodeEvaluatorFactory child)
    {
        builder.AddChildren(_ => [child]);
        return builder;
    }

    public static IEvaluatorBuilder<T> HavingChildren<T>(
        this IEvaluatorBuilder<T> builder, params INodeEvaluatorFactory[] children)
    {
        builder.AddChildren(_ => children);
        return builder;
    }

    public static IEvaluatorBuilder<T> HavingChild<T>(
        this IEvaluatorBuilder<T> builder, Func<T, INodeEvaluatorFactory> getChild)
    {
        builder.AddChildren(node => [getChild(node)]);
        return builder;
    }

    public static IEvaluatorBuilder<T> HavingChildren<T>(
        this IEvaluatorBuilder<T> builder, Func<T, IEnumerable<INodeEvaluatorFactory>> getChildren)
    {
        builder.AddChildren(node => getChildren(node));
        return builder;
    }

    public static IEvaluatorBuilder HavingAnyChild(this IEvaluatorBuilder builder)
    {
        builder.AddChildren(parent => [Factory.AcceptChildren(parent)]);
        return builder;
    }

    public static IEvaluatorBuilder<T> HavingAnyChild<T>(this IEvaluatorBuilder<T> builder)
        => builder.HavingChild(parent => Factory.AcceptChildren(parent));
}
