using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;
using TreeVal.Scaffolding.Stage.Get;

namespace TreeVal.Stage.Children;

public static class VisitChildrenStage
{
    public interface IBuilder : IVisitationStageBuilder
    {
        void AddChildren(Func<IEnumerable<IVisitorFactory>> getChildren);
    }

    public static IVisitationStageBuilder.Identity<IBuilder> Key { get; } = new();

    public static Accessors<IBuilder> ChildrenStage(this IVisitorBuilder builder)
        => new(builder);

    public static void AddChildren(this IVisitorBuilder builder, Func<Node, IEnumerable<IVisitorFactory>> getChildren)
        => builder
            .ChildrenStage()
            .Get()
            .OrCreate((n, childrenStage) => childrenStage.AddChildren(() => getChildren(n)));

    public static void AddChildren<TNode>(
        this IVisitorBuilder<TNode> builder, Func<TNode, IEnumerable<IVisitorFactory>> getChildren)
        => builder.AddChildren(n => getChildren((TNode) n.Value));
}
