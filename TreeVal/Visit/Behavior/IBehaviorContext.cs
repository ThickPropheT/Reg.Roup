using TreeVal.Visit.Stage;

namespace TreeVal.Visit.Behavior;

public interface IBehaviorContext
{
    IStageContext StageContext { get; }

    VisitationResult VisitationResult { get; }

    void RecordResult(VisitationResult result);
}
