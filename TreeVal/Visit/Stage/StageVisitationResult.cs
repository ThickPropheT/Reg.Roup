using TreeVal.Stage.Eval;

namespace TreeVal.Visit.Stage;

public class StageVisitationResult : VisitationResult
{
    public IStageContext StageContext { get; }

    public EvaluationStatus Status { get; init; } = EvaluationStatus.Accepted;

    public StageVisitationResult(IStageContext stageContext)
    {
        StageContext = stageContext;
    }

    public static StageVisitationResult ForRejection(IStageContext stageContext)
        => new(stageContext)
        {
            Status = EvaluationStatus.Rejected
        };

    public static StageVisitationResult ForError(IStageContext stageContext, Exception error)
        => new(stageContext)
        {
            Status = EvaluationStatus.Rejected,
            Error = error
        };
}
