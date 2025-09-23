using TreeVal.Eval.Condition;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public class NodeEvaluator : INodeEvaluator
{
    private readonly IEnumerable<Func<Node, IEnumerable<ICondition>>> _conditionLookups;
    private readonly IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> _childLookups;

    public VisitationContext.MovementStrategy HeadMovementStrategy { get; init; }
    public VisitationContext.EvaluationStrategy? ChildEvaluationStrategy { get; init; }

    public NodeEvaluator(
        IEnumerable<Func<Node, IEnumerable<ICondition>>> conditionLookups,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups
    )
    {
        _conditionLookups = conditionLookups;
        _childLookups = childLookups;

        HeadMovementStrategy = VisitationContext.MovementStrategy.MoveForward;
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.AllOf;
    }

    public virtual IEnumerable<ICondition> EnumerateConditions(Node current)
        => _conditionLookups.SelectMany(lookup =>
        {
            try
            {
                return lookup(current);
            }
            catch (Exception ex)
            {
                return [new LookupFailedCondition(ex)];
            }
        });

    public virtual IEnumerable<INodeEvaluatorFactory> EnumerateChildren(Node current)
        => _childLookups.SelectMany(lookup =>
        {
            try
            {
                return lookup(current);
            }
            catch (Exception ex)
            {
                return [new LookupFailedEvaluator(ex)];
            }
        });

    private class LookupFailedCondition : ICondition
    {
        private readonly Exception _error;

        public LookupFailedCondition(Exception error)
        {
            _error = error;
        }

        public void Evaluate(Node node, IConditionEvaluation evaluation)
            => throw new ConditionFailedException(evaluation, "Failure occurred during condition lookup", _error);
    }

    private class LookupFailedEvaluator : INodeEvaluatorFactory
    {
        private readonly Exception _error;

        public LookupFailedEvaluator(Exception error)
        {
            _error = error;
        }

        public INodeEvaluator ToEvaluator()
            => new NodeEvaluator([_ => [new LookupFailedCondition(_error)]], []);
    }
}
