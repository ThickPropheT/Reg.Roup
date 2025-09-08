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
        >?
        _toEvaluator;

    protected ProxyEvaluatorBuilder()
    {
    }

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

    public static (
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups
        ) Scoped(Action<IEvaluatorBuilder> body)
    {
        var builder = new ProxyEvaluatorBuilder();
        body(builder);
        return (builder.Conditions, builder.ChildLookups);
    }

    protected override INodeEvaluator ToEvaluatorImpl(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups)
        => _toEvaluator?.Invoke(conditions.ToArray(), childLookups)
           ?? throw new NotSupportedException();
}

public class ProxyEvaluatorBuilder<T> : ProxyEvaluatorBuilder, IEvaluatorBuilder<T>
{
    private ProxyEvaluatorBuilder()
    {
    }

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

    public static (
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups
        ) Scoped(Action<IEvaluatorBuilder<T>> body)
    {
        var builder = new ProxyEvaluatorBuilder<T>();
        body(builder);
        return (builder.Conditions, builder.ChildLookups);
    }
}
