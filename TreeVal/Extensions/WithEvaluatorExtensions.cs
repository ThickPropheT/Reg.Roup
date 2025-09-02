using TreeVal.Condition;

namespace TreeVal.Extensions;

public static class WithEvaluatorExtensions
{
    public static IEvaluatorBuilder<TNode> With<TNode, T>(
        this IEvaluatorBuilder<TNode> builder,
        Func<TNode, T> selector,
        Action<IEvaluatorBuilder<T>, T> apply
    )
    {
        builder.AddChildren(n =>
        {
            var t = selector(n);
            var w = new WhenBuilder<T>(t);
            apply(w, t);
            return [w];
        });

        return builder;
    }

    private class WhenBuilder<T> : EvaluatorBuilder, IEvaluatorBuilder<T>
    {
        private readonly Node<T> _node;

        public WhenBuilder(T node)
        {
            _node = new Node<T>(node);
        }

        public void AddChildren(Func<T, IEnumerable<IEvaluatorNodeFactory>> getChildren)
            => base.AddChildren(e => getChildren((T) e.Value));

        protected override EvaluatorNode ToEvaluatorImpl(IEnumerable<ICondition> conditions,
            IEnumerable<Func<Node, IEnumerable<IEvaluatorNodeFactory>>> childLookups)
            => new(conditions, childLookups)
            {
                HeadMovementStrategy = VisitationContext.MovementStrategy.From((_, _) => _node)
            };
    }
}
