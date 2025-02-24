namespace TreeVal.Extensions;

// TODO test coverage
public static class HavingChildrenEvaluatorExtensions
{
    public static IEvaluatorBuilder HavingChild(
        this IEvaluatorBuilder builder, IEvaluatorBuilder child)
    {
        builder.AddChildren(_ => [child]);
        return builder;
    }

    public static IEvaluatorBuilder HavingChildren(
        this IEvaluatorBuilder builder, params IEvaluatorBuilder[] children)
    {
        builder.AddChildren(_ => children);
        return builder;
    }

    public static IEvaluatorBuilder<TNode> HavingChild<TNode>(
        this IEvaluatorBuilder<TNode> builder, IEvaluatorBuilder child)
    {
        builder.AddChildren(_ => [child]);
        return builder;
    }

    public static IEvaluatorBuilder<TNode> HavingChildren<TNode>(
        this IEvaluatorBuilder<TNode> node, params IEvaluatorBuilder[] children)
    {
        node.AddChildren(_ => children);
        return node;
    }

    public static IEvaluatorBuilder<TNode> HavingChild<TNode>(
        this IEvaluatorBuilder<TNode> builder, Func<TNode, IEvaluatorBuilder> getChild)
    {
        builder.AddChildren(node =>
        {
            if (node is not TNode n)
            {
                throw new InvalidOperationException();
            }

            return [getChild(n)];
        });

        return builder;
    }

    public static IEvaluatorBuilder<TNode> HavingChildren<TNode>(
        this IEvaluatorBuilder<TNode> builder, Func<TNode, IEnumerable<IEvaluatorBuilder>> getChildren)
    {
        builder.AddChildren(node =>
        {
            if (node is not TNode n)
            {
                throw new InvalidOperationException();
            }

            return getChildren(n);
        });

        return builder;
    }
}
