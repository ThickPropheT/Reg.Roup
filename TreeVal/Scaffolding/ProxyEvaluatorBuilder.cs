using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class ProxyEvaluatorBuilder : VisitorBuilder
{
    private readonly Func<
            IEnumerable<Func<Node, IEnumerable<ICondition>>>,
            IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>>,
            IVisitor
        >?
        _toEvaluator;

    public ProxyEvaluatorBuilder(
        Func<
                IEnumerable<Func<Node, IEnumerable<ICondition>>>,
                IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>>,
                IVisitor
            >
            toEvaluator)
    {
        _toEvaluator = toEvaluator;
    }

    public override IVisitor CreateVisitor(Node node)
        => _toEvaluator?.Invoke(conditionLookups, childLookups)
           ?? throw new NotSupportedException();
}

public class ProxyEvaluatorBuilder<T> : ProxyEvaluatorBuilder, IVisitorBuilder<T>
{
    public ProxyEvaluatorBuilder(
        Func<
                IEnumerable<Func<Node, IEnumerable<ICondition>>>,
                IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>>,
                IVisitor
            >
            toEvaluator)
        : base(toEvaluator)
    {
    }
}
