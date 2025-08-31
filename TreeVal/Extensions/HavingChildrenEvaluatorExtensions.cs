using System.Linq.Expressions;

namespace TreeVal.Extensions;

// TODO test coverage
public static class HavingChildrenEvaluatorExtensions
{
    private static readonly DefaultVisitorNodeFactory Factory = new();

    public static IEvaluatorBuilder<Expression> HavingChildren<TExpression>(
        this IEvaluatorBuilder<Expression> builder, params IEvaluatorNodeFactory[] children)
    {
        builder.AddChildren(_ => children);
        return builder;
    }

    public static IEvaluatorBuilder<TNode> HavingChild<TNode>(
        this IEvaluatorBuilder<TNode> builder, IEvaluatorNodeFactory child)
    {
        builder.AddChildren(_ => [child]);
        return builder;
    }

    public static IEvaluatorBuilder<TNode> HavingChildren<TNode>(
        this IEvaluatorBuilder<TNode> node, params IEvaluatorNodeFactory[] children)
    {
        node.AddChildren(_ => children);
        return node;
    }

    public static IEvaluatorBuilder<TNode> HavingChild<TNode>(
        this IEvaluatorBuilder<TNode> builder, Func<TNode, IEvaluatorNodeFactory> getChild)
    {
        builder.AddChildren(node => [getChild(node)]);
        return builder;
    }

    public static IEvaluatorBuilder<TNode> HavingChildren<TNode>(
        this IEvaluatorBuilder<TNode> builder, Func<TNode, IEnumerable<IEvaluatorNodeFactory>> getChildren)
    {
        builder.AddChildren(node => getChildren(node));
        return builder;
    }

    // TODO should these go here or in the accept children extensions
    public static IEvaluatorBuilder<TNode> HavingAnyChild<TNode>(this IEvaluatorBuilder<TNode> builder)
        where TNode : Expression
        => builder.HavingChild(parent => Factory.AcceptChildren(parent));
}
