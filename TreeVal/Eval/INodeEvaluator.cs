using TreeVal.Eval.Condition;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public interface INodeEvaluator
{
    IEnumerable<ICondition> Conditions { get; }
    VisitationContext.MovementStrategy HeadMovementStrategy { get; }
    VisitationContext.EvaluationStrategy? ChildEvaluationStrategy { get; }

    IEnumerable<INodeEvaluatorFactory> EnumerateChildren(Node current);
}
