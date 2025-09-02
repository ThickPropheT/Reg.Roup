using TreeVal.Condition;
using TreeVal.Media;

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

        public WhenBuilder(T t)
        {
            _node = new Node<T>(t);
        }

        public void AddChildren(Func<T, IEnumerable<INodeEvaluatorFactory>> getChildren)
            => base.AddChildren(e => getChildren((T) e.Value));

        protected override NodeEvaluator ToEvaluatorImpl(IEnumerable<ICondition> conditions,
            IEnumerable<Func<Node, IEnumerable<INodeEvaluatorFactory>>> childLookups)
            => new(conditions, childLookups)
            {
                HeadMovementStrategy = VisitationContext.MovementStrategy.From((_, _) => _node)
            };
    }
}
