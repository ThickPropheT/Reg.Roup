using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval.AcceptChildren;

public static class AcceptChildrenEvaluatorExtensions
{
    public static IVisitorBuilder AcceptChildren<T>(this IVisitorBuilderFactory factory, T parent)
    {
        var builder = new VisitorBuilder(factory);

        builder
            .Get<IReadNodeStageBuilder>()
            .OrCreateStage(_ => new MovePastChildrenStageBuilder(
                new Node<T>(parent),
                new LinearExpressionTreeRecorder())
            );

        return builder;
    }

    public static IVisitorBuilder<TNode> AcceptChildren<TNode>(this IVisitorBuilder<TNode> builder)
        => builder.HavingChild(parent => builder.Originator.AcceptChildren(parent));

    private class MovePastChildrenStageBuilder : VisitationStageBuilder, IReadNodeStageBuilder
    {
        public MovePastChildrenStageBuilder(Node parent, IVisitationRecorder recorder)
            : base(new IVisitationStageBuilder.Identity<IReadNodeStageBuilder>())
        {
            AfterEntering(_ => new MediaBehavior.SkipChildren(parent, recorder));
        }
    }
}
