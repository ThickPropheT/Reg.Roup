using System.Runtime.CompilerServices;
using TreeVal.Scaffolding.Stage.Create;
using TreeVal.Stage.Read;

namespace TreeVal.Scaffolding;

public static class DefaultVisitorBuilder
{
    public static VisitorBuilder Create(
        IVisitorBuilderFactory originator, [CallerMemberName] string callerMemberName = "")
    {
        var builder = new VisitorBuilder(originator) { CreatedBy = callerMemberName };

        builder
            .ReadStage()
            .Create()
            .OrUpdate(_ => new MoveForwardStageBuilder { CreationSite = nameof(DefaultVisitorBuilder) });

        return builder;
    }

    public static VisitorBuilder<TNode> Create<TNode>(
        IVisitorBuilderFactory originator, [CallerMemberName] string callerMemberName = "")
    {
        var builder = new VisitorBuilder<TNode>(originator)
        {
            CreatedBy = $"{callerMemberName}`1[{typeof(TNode).Name}]"
        };

        builder
            .ReadStage()
            .Create()
            .OrUpdate(_ => new MoveForwardStageBuilder { CreationSite = nameof(DefaultVisitorBuilder) });

        return builder;
    }
}
