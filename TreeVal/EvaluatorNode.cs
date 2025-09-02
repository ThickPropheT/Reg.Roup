using TreeVal.Condition;

namespace TreeVal;

public class EvaluatorNode : IEvaluatorNode
{
    private readonly IEnumerable<Func<Node, IEnumerable<IEvaluatorNodeFactory>>> _childLookups;

    public IEnumerable<ICondition> Conditions { get; }

    public VisitationContext.MovementStrategy HeadMovementStrategy { get; init; }
    public VisitationContext.EvaluationStrategy? ChildEvaluationStrategy { get; init; }

    public EvaluatorNode(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<IEvaluatorNodeFactory>>> childLookups)
    {
        Conditions = conditions;
        _childLookups = childLookups;

        HeadMovementStrategy = VisitationContext.MovementStrategy.MoveForward;
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.AllOf;
    }

    public virtual IEnumerable<IEvaluatorNodeFactory> EnumerateChildren(Node current)
        => _childLookups.SelectMany(lookup => lookup(current));
}
