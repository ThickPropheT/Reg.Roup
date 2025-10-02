using TreeVal.Media;
using TreeVal.Visit.Behavior;

namespace TreeVal.Visit.Stage;

public interface IStageContext
{
    IVisitorContext VisitorContext { get; }
    IVisitationStage Stage { get; }
    ITapeHead TapeHead { get; }

    IEnumerable<BehaviorVisitationResult> BehaviorVisitations { get; }

    IBehaviorContext CreateBehaviorContext();

    void RecordVisitation(BehaviorVisitationResult result);
}
