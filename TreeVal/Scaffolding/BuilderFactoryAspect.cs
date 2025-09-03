using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class BuilderFactoryAspect : IEvaluatorBuilderFactory
{
    private readonly IEvaluatorBuilderFactory _source;
    private readonly Func<INodeEvaluator, INodeEvaluator> _pipe;

    public BuilderFactoryAspect(IEvaluatorBuilderFactory source, Func<INodeEvaluator, INodeEvaluator> pipe)
    {
        _source = source;
        _pipe = pipe;
    }

    public IEvaluatorBuilder Where(Func<Node, bool> predicate, string predicateExpression = "")
        => new ProxyEvaluatorBuilder((conditions, childLookups) =>
            ThroughPipe(_source.Where(predicate, predicateExpression), conditions, childLookups));

    public IEvaluatorBuilder<T> OfType<T>()
        => new ProxyEvaluatorBuilder<T>((conditions, childLookups)
            => ThroughPipe(_source.OfType<T>(), conditions, childLookups));

    public IEvaluatorBuilder OneOf(
        INodeEvaluatorFactory option1, INodeEvaluatorFactory option2, params INodeEvaluatorFactory[] options)
        => new ProxyEvaluatorBuilder((conditions, _)
            => ThroughPipe(_source.OneOf(option1, option2, options), conditions, []));

    private INodeEvaluator ThroughPipe(
        IEvaluatorBuilder builder,
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups
    )
    {
        foreach (var condition in conditions)
        {
            builder.AddCondition(condition);
        }

        foreach (var childLookup in childLookups)
        {
            builder.AddChildren(childLookup);
        }

        var evaluator = builder.ToEvaluator();

        return _pipe(evaluator);
    }
}
