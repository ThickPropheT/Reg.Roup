using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Read;

public static class ScaffoldingExtensions
{
    public static IStageQuery<IReadNodeStageBuilder> GetReadStage(this IVisitorBuilder builder)
        => builder.Get<IReadNodeStageBuilder>();
}
