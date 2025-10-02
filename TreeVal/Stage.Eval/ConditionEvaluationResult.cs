using TreeVal.Visit;

namespace TreeVal.Stage.Eval;

public class ConditionEvaluationResult : VisitationResult
{
    public IConditionContext Context { get; }

    public EvaluationStatus Status { get; init; } = EvaluationStatus.Accepted;

    public ConditionEvaluationResult(IConditionContext context)
    {
        Context = context;
    }

    public static ConditionEvaluationResult ForRejection(IConditionContext context)
        => new(context)
        {
            Status = EvaluationStatus.Rejected
        };

    public static ConditionEvaluationResult ForError(IConditionContext context, Exception error)
        => new(context)
        {
            Status = EvaluationStatus.Rejected,
            Error = error
        };
}
