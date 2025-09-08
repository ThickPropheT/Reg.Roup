using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class EvaluatorBuilder : IEvaluatorBuilder
{
    protected readonly List<ICondition> Conditions = new(1);
    protected readonly List<Func<Node, IEnumerable<INodeEvaluatorFactory>>> ChildLookups = new(1);

    public void AddCondition(ICondition condition)
        => Conditions.Add(condition);

    public void AddChildren(Func<Node, IEnumerable<INodeEvaluatorFactory>> getChildren)
        => ChildLookups.Add(getChildren);

    public INodeEvaluator ToEvaluator()
        => ToEvaluatorImpl(Conditions, ChildLookups);

    protected virtual INodeEvaluator ToEvaluatorImpl(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups)
        => new NodeEvaluator(conditions.ToArray(), childLookups);
}
