using TreeVal.Eval;

namespace TreeVal.Scaffolding;

public class MoveForwardStageBuilder : VisitationStageBuilder, IReadNodeStageBuilder
{
    public MoveForwardStageBuilder()
        : base(new IVisitationStageBuilder.Identity<IReadNodeStageBuilder>())
    {
        AfterEntering(_ => new MediaBehavior.MoveForward());
    }
}
