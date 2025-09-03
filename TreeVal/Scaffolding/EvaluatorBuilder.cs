using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class EvaluatorBuilder : IEvaluatorBuilder
{
    private readonly List<ICondition> _conditions = new(1);
    private readonly List<Func<Node, IEnumerable<INodeEvaluatorFactory>>> _childLookups = new(1);

    public void AddCondition(ICondition condition)
        => _conditions.Add(condition);

    public void AddChildren(Func<Node, IEnumerable<INodeEvaluatorFactory>> getChildren)
        => _childLookups.Add(getChildren);

    public INodeEvaluator ToEvaluator()
        => ToEvaluatorImpl(_conditions, _childLookups);

    protected virtual INodeEvaluator ToEvaluatorImpl(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups)
        => new NodeEvaluator(conditions.ToArray(), childLookups);
}
