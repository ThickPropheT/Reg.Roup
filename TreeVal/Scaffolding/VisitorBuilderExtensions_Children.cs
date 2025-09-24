using TreeVal.Eval;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public static class VisitorBuilderExtensions_Children
{
    public static void AddChildren(this IVisitorBuilder builder, Func<Node, IEnumerable<IVisitorFactory>> getChildren)
        => builder
            .Get<IEvaluateChildrenStageBuilder>()
            .OrCreateStage((n, childrenStage) => childrenStage.AddChildren(() => getChildren(n)));

    public static void AddChildren<TNode>(
        this IVisitorBuilder<TNode> builder, Func<TNode, IEnumerable<IVisitorFactory>> getChildren)
        => builder.AddChildren(n => getChildren((TNode) n.Value));
}