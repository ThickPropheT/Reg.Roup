using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class BuilderFactoryAspect : IVisitorBuilderFactory
{
    private readonly IVisitorBuilderFactory _source;
    private readonly Func<IVisitor, IVisitor> _pipe;

    public IStageDirector StageDirector => _source.StageDirector;

    public BuilderFactoryAspect(IVisitorBuilderFactory source, Func<IVisitor, IVisitor> pipe)
    {
        _source = source;
        _pipe = pipe;
    }

    public IVisitorBuilder Where(Func<Node, bool> predicate, string predicateExpression = "")
        => new ProxyEvaluatorBuilder((conditions, childLookups) =>
            ThroughPipe(_source.Where(predicate, predicateExpression), conditions, childLookups));

    public IVisitorBuilder<T> OfType<T>()
        => new ProxyEvaluatorBuilder<T>((conditions, childLookups) =>
            ThroughPipe(_source.OfType<T>(), conditions, childLookups));

    public IVisitorBuilder OneOf(
        IVisitorFactory option1, IVisitorFactory option2, params IVisitorFactory[] options)
        => new ProxyEvaluatorBuilder((conditions, _) =>
            ThroughPipe(_source.OneOf(option1, option2, options), conditions, []));

    private IVisitor ThroughPipe(
        IVisitorBuilder builder,
        IEnumerable<Func<Node, IEnumerable<ICondition>>> conditionLookups,
        IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>> childLookups
    )
    {
        foreach (var conditionLookup in conditionLookups)
        {
            builder.AddConditions(conditionLookup);
        }

        foreach (var childLookup in childLookups)
        {
            builder.AddChildren(childLookup);
        }

        var visitor = builder.CreateVisitor();

        return _pipe(visitor);
    }
}
