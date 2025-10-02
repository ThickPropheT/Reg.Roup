using TreeVal.Media;
using TreeVal.Visit.Stage;

namespace TreeVal.Visit.Behavior;

public interface IBehaviorContext
{
    IVisitorContext VisitorContext { get; }
    IStageContext StageContext { get; }

    ITapeHead TapeHead { get; }

    VisitationResult VisitationResult { get; }

    void RecordResult(VisitationResult result);
}
