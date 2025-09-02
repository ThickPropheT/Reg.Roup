using TreeVal.Condition;
using TreeVal.Media;

namespace TreeVal;

public class ProxyEvaluatorBuilder : EvaluatorBuilder
{
    private readonly Func<
            IEnumerable<ICondition>,
            IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>>, NodeEvaluator>
        _toEvaluator;

    public ProxyEvaluatorBuilder(
        Func<
                IEnumerable<ICondition>,
                IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>>, NodeEvaluator>
            toEvaluator)
    {
        _toEvaluator = toEvaluator;
    }

    protected override NodeEvaluator ToEvaluatorImpl(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups)
        => _toEvaluator(conditions.ToArray(), childLookups);
}

public class ProxyEvaluatorBuilder<T> : ProxyEvaluatorBuilder, IEvaluatorBuilder<T>
{
    public ProxyEvaluatorBuilder(
        Func<IEnumerable<ICondition>, IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>>, NodeEvaluator>
            toEvaluator) : base(toEvaluator)
    {
    }

    public void AddChildren(Func<T, IEnumerable<INodeEvaluatorFactory>> getChildren)
        => base.AddChildren(e => getChildren((T) e.Value));
}
