namespace TreeVal.Extensions;

// TODO test coverage
public static class HavingChildrenEvaluatorExtensions
{
    private static readonly DefaultVisitorNodeFactory Factory = new();

    public static IEvaluatorBuilder HavingChild(
        this IEvaluatorBuilder builder, IEvaluatorNodeFactory child)
    {
        builder.AddChildren(_ => [child]);
        return builder;
    }

    public static IEvaluatorBuilder HavingChildren(
        this IEvaluatorBuilder builder, params IEvaluatorNodeFactory[] children)
    {
        builder.AddChildren(_ => children);
        return builder;
    }

    public static IEvaluatorBuilder<T> HavingChild<T>(
        this IEvaluatorBuilder<T> builder, IEvaluatorNodeFactory child)
    {
        builder.AddChildren(_ => [child]);
        return builder;
    }

    public static IEvaluatorBuilder<T> HavingChildren<T>(
        this IEvaluatorBuilder<T> node, params IEvaluatorNodeFactory[] children)
    {
        node.AddChildren(_ => children);
        return node;
    }

    public static IEvaluatorBuilder<T> HavingChild<T>(
        this IEvaluatorBuilder<T> builder, Func<T, IEvaluatorNodeFactory> getChild)
    {
        builder.AddChildren(node => [getChild(node)]);
        return builder;
    }

    public static IEvaluatorBuilder<T> HavingChildren<T>(
        this IEvaluatorBuilder<T> builder, Func<T, IEnumerable<IEvaluatorNodeFactory>> getChildren)
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
