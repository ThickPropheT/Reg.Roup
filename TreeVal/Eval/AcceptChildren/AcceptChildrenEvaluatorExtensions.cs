using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval.AcceptChildren;

public static class AcceptChildrenEvaluatorExtensions
{
    public static IVisitorBuilder AcceptChildren<T>(this IVisitorBuilderFactory _, T parent)
        => new ProxyEvaluatorBuilder((conditions, _) =>
            new AcceptChildrenNodeEvaluator(conditions, new Node<T>(parent), new LinearExpressionTreeRecorder()));

    public static IVisitorBuilder<TNode> AcceptChildren<TNode>(this IVisitorBuilder<TNode> builder)
        => builder.HavingChild(parent => builder.Originator.AcceptChildren(parent));
}
