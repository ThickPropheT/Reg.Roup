namespace TreeVal.Eval.All;

public static class NodeEvaluationExtensions
{
    public static void RejectWhenAnyRejected(
        this INodeEvaluation evaluation, IEnumerable<INodeEvaluation> childEvaluations)
    {
        var evaluations = childEvaluations.ToArray();
        
        evaluation.Record(evaluations);

        if (evaluations.All(e => e.Status == EvaluationStatus.Accepted))
            return;
        
        evaluation.Reject();
    }
}
