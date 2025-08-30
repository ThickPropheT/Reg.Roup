using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class EvaluatorNode : IEvaluatorNode
{
    private readonly IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> _childLookups;

    public IEnumerable<ICondition> Conditions { get; }

    public VisitationContext.MovementStrategy HeadMovementStrategy { get; protected init; }
    public VisitationContext.EvaluationStrategy? ChildEvaluationStrategy { get; protected init; }

    public EvaluatorNode(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> childLookups)
    {
        Conditions = conditions;
        _childLookups = childLookups;

        HeadMovementStrategy = VisitationContext.MovementStrategy.MoveForward;
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.AllOf;
    }

    public virtual IEnumerable<IEvaluatorNodeFactory> EnumerateChildren(Expression current)
        => _childLookups.SelectMany(lookup => lookup(current));
}
