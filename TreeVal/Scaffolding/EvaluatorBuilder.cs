using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class EvaluatorBuilder : IEvaluatorBuilder
{
    protected readonly List<Func<Node, IEnumerable<ICondition>>> ConditionLookups = new(1);
    protected readonly List<Func<Node, IEnumerable<INodeEvaluatorFactory>>> ChildLookups = new(1);

    public void AddConditions(Func<Node, IEnumerable<ICondition>> getConditions)
        => ConditionLookups.Add(getConditions);

    public void AddChildren(Func<Node, IEnumerable<INodeEvaluatorFactory>> getChildren)
        => ChildLookups.Add(getChildren);

    public INodeEvaluator ToEvaluator()
        => ToEvaluatorImpl(ConditionLookups, ChildLookups);

    protected virtual INodeEvaluator ToEvaluatorImpl(
        IEnumerable<Func<Node, IEnumerable<ICondition>>> conditionLookups,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups)
        => new NodeEvaluator(conditionLookups, childLookups);
}
