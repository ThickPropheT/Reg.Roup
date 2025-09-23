using TreeVal.Media;

namespace TreeVal.Eval;

public interface IVisitationStage
{
    IStageContext Visit(TapeHead head, IStageContext stageContext);
}
