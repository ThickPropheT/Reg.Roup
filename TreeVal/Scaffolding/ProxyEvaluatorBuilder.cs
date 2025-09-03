using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class ProxyEvaluatorBuilder : EvaluatorBuilder
{
    private readonly Func<
            IEnumerable<ICondition>,
            IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>>,
            INodeEvaluator
        >
        _toEvaluator;

    public ProxyEvaluatorBuilder(
        Func<
                IEnumerable<ICondition>,
                IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>>,
                INodeEvaluator
            >
            toEvaluator)
    {
        _toEvaluator = toEvaluator;
    }

    protected override INodeEvaluator ToEvaluatorImpl(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups)
        => _toEvaluator(conditions.ToArray(), childLookups);
}

public class ProxyEvaluatorBuilder<T> : ProxyEvaluatorBuilder, IEvaluatorBuilder<T>
{
    public ProxyEvaluatorBuilder(
        Func<
                IEnumerable<ICondition>,
                IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>>,
                INodeEvaluator
            >
            toEvaluator)
        : base(toEvaluator)
    {
    }
}
