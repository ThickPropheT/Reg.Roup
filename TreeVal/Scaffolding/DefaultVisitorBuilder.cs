using TreeVal.Eval;

namespace TreeVal.Scaffolding;

public static class DefaultVisitorBuilder
{
    public static VisitorBuilder Create(IVisitorBuilderFactory creator)
    {
        var builder = new VisitorBuilder(creator);

        builder
            .Get<IReadNodeStageBuilder>()
            .OrCreateStage(_ => new MoveForwardStageBuilder());

        return builder;
    }

    public static VisitorBuilder<TNode> Create<TNode>(IVisitorBuilderFactory creator)
    {
        var builder = new VisitorBuilder<TNode>(creator);

        builder
            .Get<IReadNodeStageBuilder>()
            .OrCreateStage(_ => new MoveForwardStageBuilder());

        return builder;
    }
}
