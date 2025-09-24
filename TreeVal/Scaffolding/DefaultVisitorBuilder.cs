using TreeVal.Eval;

namespace TreeVal.Scaffolding;

public static class DefaultVisitorBuilder
{
    public static VisitorBuilder Create(IVisitorBuilderFactory originator)
    {
        var builder = new VisitorBuilder(originator);

        builder
            .Get<IReadNodeStageBuilder>()
            .OrCreateStage(_ => new MoveForwardStageBuilder());

        return builder;
    }

    public static VisitorBuilder<TNode> Create<TNode>(IVisitorBuilderFactory originator)
    {
        var builder = new VisitorBuilder<TNode>(originator);

        builder
            .Get<IReadNodeStageBuilder>()
            .OrCreateStage(_ => new MoveForwardStageBuilder());

        return builder;
    }
}
