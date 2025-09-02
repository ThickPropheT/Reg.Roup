using TreeVal.Condition;
using TreeVal.Media;

namespace TreeVal;

public class NodeEvaluator : INodeEvaluator
{
    private readonly IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> _childLookups;

    public IEnumerable<ICondition> Conditions { get; }

    public VisitationContext.MovementStrategy HeadMovementStrategy { get; init; }
    public VisitationContext.EvaluationStrategy? ChildEvaluationStrategy { get; init; }

    public NodeEvaluator(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups)
    {
        Conditions = conditions;
        _childLookups = childLookups;

        HeadMovementStrategy = VisitationContext.MovementStrategy.MoveForward;
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.AllOf;
    }

    public virtual IEnumerable<INodeEvaluatorFactory> EnumerateChildren(Node current)
        => _childLookups.SelectMany(lookup => lookup(current));
}
