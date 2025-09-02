using TreeVal.Condition;

namespace TreeVal;

public class ProxyEvaluatorBuilder : EvaluatorBuilder
{
    private readonly Func<
            IEnumerable<ICondition>,
            IEnumerable<Func<Node, IEnumerable<IEvaluatorNodeFactory>>>, EvaluatorNode>
        _toEvaluator;

    public ProxyEvaluatorBuilder(
        Func<
                IEnumerable<ICondition>,
                IEnumerable<Func<Node, IEnumerable<IEvaluatorNodeFactory>>>, EvaluatorNode>
            toEvaluator)
    {
        _toEvaluator = toEvaluator;
    }

    protected override EvaluatorNode ToEvaluatorImpl(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<IEvaluatorNodeFactory>>> childLookups)
        => _toEvaluator(conditions.ToArray(), childLookups);
}

public class ProxyEvaluatorBuilder<T> : ProxyEvaluatorBuilder, IEvaluatorBuilder<T>
{
    public ProxyEvaluatorBuilder(
        Func<IEnumerable<ICondition>, IEnumerable<Func<Node, IEnumerable<IEvaluatorNodeFactory>>>, EvaluatorNode>
            toEvaluator) : base(toEvaluator)
    {
    }

    public void AddChildren(Func<T, IEnumerable<IEvaluatorNodeFactory>> getChildren)
        => base.AddChildren(e => getChildren((T) e.Value));
}
