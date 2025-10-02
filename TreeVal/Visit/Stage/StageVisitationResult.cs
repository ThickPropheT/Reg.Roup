using TreeVal.Media;
using TreeVal.Stage.Eval;
using TreeVal.Visit.Behavior;

namespace TreeVal.Visit.Stage;

public class StageVisitationResult : VisitationResult
{
    public IStageContext StageContext { get; }

    public ITapeHead TapeHead => StageContext.TapeHead;
    public IEnumerable<BehaviorVisitationResult> BehaviorVisitations => StageContext.BehaviorVisitations;

    public EvaluationStatus Status { get; init; } = EvaluationStatus.Accepted;
    public string? Message { get; init; }

    public StageVisitationResult(IStageContext stageContext)
    {
        StageContext = stageContext;
    }

    public static StageVisitationResult ForRejection(IStageContext stageContext, string message)
        => new(stageContext)
        {
            Status = EvaluationStatus.Rejected,
            Message = message
        };

    public static StageVisitationResult ForError(IStageContext stageContext, Exception error)
        => new(stageContext)
        {
            Status = EvaluationStatus.Rejected,
            Error = error
        };
}
