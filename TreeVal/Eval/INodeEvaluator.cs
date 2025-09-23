using TreeVal.Eval.Condition;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public interface INodeEvaluator
{
    VisitationContext.MovementStrategy HeadMovementStrategy { get; }
    VisitationContext.EvaluationStrategy? ChildEvaluationStrategy { get; }

    IEnumerable<ICondition> EnumerateConditions(Node current);
    IEnumerable<INodeEvaluatorFactory> EnumerateChildren(Node current);
}
