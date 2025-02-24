using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class IgnoreBoxingEvaluatorExtensions
{
    // TODO
    //  modify this to take a child IVisitorNode, rather than doing this chaining thing.
    //  i think there's in the chaining logic that's returning new VisitorNode and orphaning
    //  the ignore boxing part
    public static IVisitorNodeFactory IgnoreBoxing(this IVisitorNodeFactory factory)
        => new IgnoreBoxingVisitorNodeFactory(factory);

    private class IgnoreBoxingVisitorNodeFactory : IVisitorNodeFactory
    {
        private readonly IVisitorNodeFactory _source;
        private readonly IEvaluatorBuilder<UnaryExpression> _ignored;

        public IgnoreBoxingVisitorNodeFactory(IVisitorNodeFactory source)
        {
            _source = source;
            _ignored = source.Cast();
        }

        public IEvaluatorBuilder OfType(ExpressionType nodeType) 
            => new IgnoreNode(_ignored, _source.OfType(nodeType));

        public IEvaluatorBuilder<TExpression> OfType<TExpression>(ExpressionType? nodeType = null)
            where TExpression : Expression
            => new IgnoreNode<TExpression>(_ignored, _source.OfType<TExpression>(nodeType));

        public IEvaluatorBuilder OneOf(params IEvaluatorBuilder[] children)
            => new IgnoreNode(_ignored, _source.OneOf(children));
        
    }

    private class IgnoreNode : ExpressionTreeEvaluator.EvaluatorBuilderBase
    {
        private readonly IEvaluatorBuilder _ignored;
        private readonly IEvaluatorBuilder _inner;

        public IgnoreNode(IEvaluatorBuilder ignored, IEvaluatorBuilder inner)
        {
            _ignored = ignored;
            _inner = inner;
        }
        
        // TODO this doesn't handle conditions or children
        public override IEvaluatorNode ToEvaluator()
            => new ProxyEvaluator((self, context) =>
            {
                context.Try(copy => _ignored.ToEvaluator().Evaluate(copy));

                _inner.ToEvaluator().Evaluate(context);

                if (context.HasRejection)
                {
                    context.Reject(self);
                    return null;
                }
                
                context.Accept(self);
                return null;
            });
    }

    private class IgnoreNode<T> : IgnoreNode, IEvaluatorBuilder<T>
    {
        private readonly IEvaluatorBuilder<T> _innerT;

        public IgnoreNode(IEvaluatorBuilder ignored, IEvaluatorBuilder<T> inner) 
            : base(ignored, inner)
        {
            _innerT = inner;
        }

        public IEvaluatorBuilder<T> Where(Func<T, bool> predicate, string predicateExpression = "")
            => _innerT.Where(predicate, predicateExpression);

        public new IEvaluatorBuilder<T> HavingChild(IEvaluatorBuilder child)
            => _innerT.HavingChild(child);

        public new IEvaluatorBuilder<T> HavingChildren(params IEvaluatorBuilder[] children)
            => _innerT.HavingChildren(children);

        public IEvaluatorBuilder<T> HavingChildren(Func<T, IEvaluatorBuilder[]> buildChildren)
            => _innerT.HavingChildren(buildChildren);

        public IEvaluatorBuilder<T> WithEachChildBeing<TChild>(Func<T, IEnumerable<TChild>> findChildren, Func<TChild, IEvaluatorBuilder> buildChildren)
            => _innerT.WithEachChildBeing(findChildren, buildChildren);
    }
}
