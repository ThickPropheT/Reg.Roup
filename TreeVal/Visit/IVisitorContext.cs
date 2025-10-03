using TreeVal.Media;
using TreeVal.Visit.Stage;

namespace TreeVal.Visit;

public interface IVisitorContext
{
    ITapeHead TapeHead { get; }

    IVisitor Visitor { get; }

    IEnumerable<StageVisitationResult> StageVisitations { get; }

    IStageContext CreateStageContext(IVisitationStage stage, IStageContext? stageContext);

    void RecordVisitation(StageVisitationResult result);
}
