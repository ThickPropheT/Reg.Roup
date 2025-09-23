using TreeVal.Diagnostics;
using TreeVal.Media;

namespace TreeVal.Eval;

public interface INodeEvaluation : IDescribable
{
    INodeEvaluation? Parent { get; }
    
    EvaluationStatus Status { get; }
    
    IConditionEvaluation[] ConditionEvaluations { get; }
    IEnumerable<INodeEvaluation> ChildEvaluations { get; }

    IVisitor GetEvaluator();
    Node? GetTarget(VisitationContext context);

    void Record(IEnumerable<IConditionEvaluation> conditionEvaluations);
    void Record(IEnumerable<INodeEvaluation> childEvaluations);

    void Reject();
}
