using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Read;

public class MoveForwardStageBuilder : VisitationStageBuilder, ReadNodeStage.IBuilder
{
    public MoveForwardStageBuilder()
        : base(ReadNodeStage.Key)
    {
        AfterEntering((_, _) => new MediaBehavior.MoveForward());
    }
}
