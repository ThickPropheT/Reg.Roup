namespace TreeVal.Eval.Any;

public static class NodeEvaluationExtensions
{
    public static void RejectWhenAllRejected(
        this INodeEvaluation evaluation, IEnumerable<INodeEvaluation> childEvaluations)
    {
        var evaluations = childEvaluations.ToArray();
        
        evaluation.Record(evaluations);

        if (evaluations.Any(e => e.Status == EvaluationStatus.Accepted))
            return;
        
        evaluation.Reject();
    }
}
