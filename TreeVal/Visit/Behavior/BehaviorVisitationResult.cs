using TreeVal.Media;
using TreeVal.Stage.Eval;

namespace TreeVal.Visit.Behavior;

public class BehaviorVisitationResult : VisitationResult
{
    public IBehaviorContext BehaviorContext { get; }

    public ITapeHead TapeHead => BehaviorContext.StageContext.TapeHead;
    public VisitationResult VisitationResult => BehaviorContext.VisitationResult;

    public EvaluationStatus Status { get; init; } = EvaluationStatus.Accepted;

    public BehaviorVisitationResult(IBehaviorContext behaviorContext)
    {
        BehaviorContext = behaviorContext;
    }

    public static BehaviorVisitationResult ForError(IBehaviorContext behaviorContext, Exception error)
        => new(behaviorContext)
        {
            Status = EvaluationStatus.Rejected,
            Error = error
        };
}
