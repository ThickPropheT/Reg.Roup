using TreeVal.Eval;

namespace TreeVal.Scaffolding;

public class MoveForwardStageBuilder : VisitationStageBuilder, IReadNodeStageBuilder
{
    public MoveForwardStageBuilder()
    {
        AfterEntering(_ => new MediaBehavior.MoveForward());
    }
}