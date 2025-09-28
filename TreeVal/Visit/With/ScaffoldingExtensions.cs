using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Stage.Children;
using TreeVal.Stage.Eval;

namespace TreeVal.Visit.With;

public static class ScaffoldingExtensions
{
    public static IVisitorBuilder<TNode> With<TNode, T>(
        this IVisitorBuilder<TNode> builder,
        Func<TNode, T> selector,
        Action<IVisitorBuilder<T>, T> apply
    )
    {
        builder.AddChildren(n =>
        {
            var t = selector(n);
            var w = new WhenBuilder<T>(t, builder.Originator);
            apply(w, t);
            return [w];
        });

        return builder;
    }

    private class WhenBuilder<T> : VisitorBuilder, IVisitorBuilder<T>
    {
        private readonly Node<T> _node;

        public WhenBuilder(T t, IVisitorBuilderFactory originator)
            : base(originator)
        {
            _node = new Node<T>(t);
        }

        public override IVisitor CreateVisitor(Node node)
            // => new Visitor();
            => throw new NotImplementedException();

        // protected Visitor ToEvaluatorImpl(
        //     IEnumerable<Func<Node, IEnumerable<ICondition>>> conditionLookups,
        //     IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>> childLookups)
        //     => new(conditionLookups, childLookups)
        //     {
        //         HeadMovementStrategy = VisitationContext.MovementStrategy.From((_, _, _) => _node)
        //     };
    }
}
