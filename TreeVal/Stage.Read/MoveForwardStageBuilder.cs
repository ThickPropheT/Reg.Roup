using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Read;

public class MoveForwardStageBuilder : VisitationStageBuilder, IReadNodeStageBuilder
{
    public MoveForwardStageBuilder()
        : base(new IVisitationStageBuilder.Identity<IReadNodeStageBuilder>())
    {
        AfterEntering((_, c) => new MediaBehavior.MoveForward());
    }
}
