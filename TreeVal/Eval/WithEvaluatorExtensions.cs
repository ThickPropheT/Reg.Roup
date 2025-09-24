using TreeVal.Eval.Condition;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public static class WithEvaluatorExtensions
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
            => new Visitor();

        protected Visitor ToEvaluatorImpl(
            IEnumerable<Func<Node, IEnumerable<ICondition>>> conditionLookups,
            IEnumerable<Func<Node, IEnumerable<IVisitorFactory>>> childLookups)
            => new(conditionLookups, childLookups)
            {
                HeadMovementStrategy = VisitationContext.MovementStrategy.From((_, _, _) => _node)
            };
    }
}
