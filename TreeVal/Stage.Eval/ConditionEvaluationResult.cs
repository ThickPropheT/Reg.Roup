namespace TreeVal.Stage.Eval;

public class ConditionEvaluationResult : VisitationResult
{
    public IConditionEvaluation Evaluation { get; }

    public EvaluationStatus Status { get; init; } = EvaluationStatus.Accepted;

    public ConditionEvaluationResult(IConditionEvaluation evaluation)
    {
        Evaluation = evaluation;
    }

    public static ConditionEvaluationResult ForRejection(IConditionEvaluation evaluation)
        => new(evaluation)
        {
            Status = EvaluationStatus.Rejected
        };

    public static ConditionEvaluationResult ForError(IConditionEvaluation evaluation, Exception error)
        => new(evaluation)
        {
            Status = EvaluationStatus.Rejected,
            Error = error
        };
}
