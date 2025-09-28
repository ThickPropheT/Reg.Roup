using TreeVal.Media;

namespace TreeVal.Visit.Stage;

public interface IVisitationStage
{
    IStageContext Visit(TapeHead head, IStageContext stageContext);
}
