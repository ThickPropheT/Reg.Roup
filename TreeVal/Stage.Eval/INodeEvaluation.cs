using TreeVal.Diagnostics;
using TreeVal.Media;
using TreeVal.Visit;

namespace TreeVal.Stage.Eval;

public interface INodeEvaluation : IDescribable
{
    INodeEvaluation? Parent { get; }

    EvaluationStatus Status { get; }

    IConditionContext[] ConditionEvaluations { get; }
    IEnumerable<INodeEvaluation> ChildEvaluations { get; }

    IVisitor GetEvaluator(Node node);
    // Node? GetTarget(VisitationContext context);

    void Record(IEnumerable<IConditionContext> conditionEvaluations);
    void Record(IEnumerable<INodeEvaluation> childEvaluations);

    void Reject();
}
