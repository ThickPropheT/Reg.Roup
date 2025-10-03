using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Read;

public class ReadCurrentStageBuilder : VisitationStageBuilder, ReadNodeStage.IBuilder
{
    public ReadCurrentStageBuilder() 
        : base(ReadNodeStage.Key)
    {
        AfterEntering((_, _) => new MediaBehavior.ReadCurrent());
    }
}
