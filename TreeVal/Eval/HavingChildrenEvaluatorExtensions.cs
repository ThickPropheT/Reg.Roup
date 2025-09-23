using TreeVal.Eval.AcceptChildren;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

// TODO test coverage
public static class HavingChildrenEvaluatorExtensions
{
    public static IVisitorBuilder HavingChild(
        this IVisitorBuilder builder, IVisitorFactory child)
    {
        builder.AddChildren(_ => [child]);
        return builder;
    }

    public static IVisitorBuilder HavingChildren(
        this IVisitorBuilder builder, params IVisitorFactory[] children)
    {
        builder.AddChildren(_ => children);
        return builder;
    }

    public static IVisitorBuilder<T> HavingChild<T>(
        this IVisitorBuilder<T> builder, IVisitorFactory child)
    {
        builder.AddChildren(_ => [child]);
        return builder;
    }

    public static IVisitorBuilder<T> HavingChildren<T>(
        this IVisitorBuilder<T> builder, params IVisitorFactory[] children)
    {
        builder.AddChildren(_ => children);
        return builder;
    }

    public static IVisitorBuilder<T> HavingChild<T>(
        this IVisitorBuilder<T> builder, Func<T, IVisitorFactory> getChild)
    {
        builder.AddChildren(node => [getChild(node)]);
        return builder;
    }

    public static IVisitorBuilder<T> HavingChildren<T>(
        this IVisitorBuilder<T> builder, Func<T, IEnumerable<IVisitorFactory>> getChildren)
    {
        builder.AddChildren(node => getChildren(node));
        return builder;
    }

    public static IVisitorBuilder HavingAnyChild(this IVisitorBuilder builder)
    {
        builder.AddChildren(parent => [builder.Originator.AcceptChildren(parent)]);
        return builder;
    }

    public static IVisitorBuilder<T> HavingAnyChild<T>(this IVisitorBuilder<T> builder)
        => builder.HavingChild(parent => builder.Originator.AcceptChildren(parent));
}
