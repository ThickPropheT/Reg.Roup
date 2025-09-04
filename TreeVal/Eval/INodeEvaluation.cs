using TreeVal.Diagnostics;
using TreeVal.Media;

namespace TreeVal.Eval;

public interface INodeEvaluation : IDescribable
{
    EvaluationStatus Status { get; }

    INodeEvaluator GetEvaluator();
    Node? GetTarget(VisitationContext context);

    void Record(IEnumerable<IConditionEvaluation> conditionEvaluations);
    void Record(IEnumerable<INodeEvaluation> childEvaluations);

    void Reject();
}
