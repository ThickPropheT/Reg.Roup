using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Children;

public static class VisitChildrenStage
{
    public interface IBuilder : IVisitationStageBuilder
    {
        void AddChildren(Func<IEnumerable<IVisitorFactory>> getChildren);
    }

    public static IVisitationStageBuilder.Identity<IBuilder> Key { get; } = new();

    public static IStageQuery<IBuilder> GetVisitChildrenStage(this IVisitorBuilder builder)
        => builder.Get<IBuilder>();

    public static void AddChildren(this IVisitorBuilder builder, Func<Node, IEnumerable<IVisitorFactory>> getChildren)
        => builder
            .GetVisitChildrenStage()
            .OrCreateStage((n, childrenStage) => childrenStage.AddChildren(() => getChildren(n)));

    public static void AddChildren<TNode>(
        this IVisitorBuilder<TNode> builder, Func<TNode, IEnumerable<IVisitorFactory>> getChildren)
        => builder.AddChildren(n => getChildren((TNode) n.Value));
}
