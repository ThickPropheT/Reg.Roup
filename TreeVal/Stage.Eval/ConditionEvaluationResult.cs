namespace TreeVal.Stage.Eval;

public class ConditionEvaluationResult : VisitationResult
{
    public IConditionEvaluation Evaluation { get; }

    public Exception? Error { get; init; }

    public ConditionEvaluationResult(IConditionEvaluation evaluation)
    {
        Evaluation = evaluation;
    }

    public static ConditionEvaluationResult ForRejection(IConditionEvaluation evaluation)
        // => new(evaluation);
        => throw new NotImplementedException();

    public static ConditionEvaluationResult ForError(IConditionEvaluation evaluation, Exception error)
        => new(evaluation) { Error = error };
}
