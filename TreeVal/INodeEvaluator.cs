using TreeVal.Condition;
using TreeVal.Media;

namespace TreeVal;

public interface INodeEvaluator
{
    IEnumerable<ICondition> Conditions { get; }
    VisitationContext.MovementStrategy HeadMovementStrategy { get; }
    VisitationContext.EvaluationStrategy? ChildEvaluationStrategy { get; }

    IEnumerable<INodeEvaluatorFactory> EnumerateChildren(Node current);
}
