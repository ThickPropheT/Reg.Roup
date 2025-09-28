using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Read;

public static class ReadNodeStage
{
    public interface IBuilder : IVisitationStageBuilder
    {
    }

    public static IVisitationStageBuilder.Identity<IBuilder> Key { get; } = new();

    public static IStageQuery<IBuilder> GetReadStage(this IVisitorBuilder builder)
        => builder.Get<IBuilder>();
}
