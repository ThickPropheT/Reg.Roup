using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Stage.Children.HavingChildren;
using TreeVal.Stage.Read;

namespace TreeVal.Stage.Children.SkipChildren;

public static class ScaffoldingExtensions
{
    public static IVisitorBuilder AcceptChildren<T>(this IVisitorBuilderFactory factory, T parent)
    {
        var builder = new VisitorBuilder(factory);

        builder
            .GetReadStage()
            .OrCreateStage(_ => new MovePastChildrenStageBuilder(
                new Node<T>(parent),
                new LinearExpressionTreeRecorder())
            );

        return builder;
    }

    public static IVisitorBuilder<TNode> AcceptChildren<TNode>(this IVisitorBuilder<TNode> builder)
        => builder.HavingChild(parent => builder.Originator.AcceptChildren(parent));
}
