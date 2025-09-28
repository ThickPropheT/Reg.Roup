using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Children;

public static class ScaffoldingExtensions
{
    public static IStageQuery<IVisitChildrenStageBuilder> GetVisitChildrenStage(this IVisitorBuilder builder)
        => builder.Get<IVisitChildrenStageBuilder>();

    public static void AddChildren(this IVisitorBuilder builder, Func<Node, IEnumerable<IVisitorFactory>> getChildren)
        => builder
            .GetVisitChildrenStage()
            .OrCreateStage((n, childrenStage) => childrenStage.AddChildren(() => getChildren(n)));

    public static void AddChildren<TNode>(
        this IVisitorBuilder<TNode> builder, Func<TNode, IEnumerable<IVisitorFactory>> getChildren)
        => builder.AddChildren(n => getChildren((TNode) n.Value));
}
