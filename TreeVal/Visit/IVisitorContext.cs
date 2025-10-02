using TreeVal.Media;
using TreeVal.Visit.Stage;

namespace TreeVal.Visit;

public interface IVisitorContext
{
    ITapeHead TapeHead { get; }

    IEnumerable<StageVisitationResult> StageVisitations { get; }

    IStageContext CreateStageContext(IVisitationStage stage);

    void RecordVisitation(StageVisitationResult result);
}
