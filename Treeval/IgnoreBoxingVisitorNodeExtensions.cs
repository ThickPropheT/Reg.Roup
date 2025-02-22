using System.Linq.Expressions;

namespace Treeval;

public static class IgnoreBoxingVisitorNodeExtensions
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
        private readonly IVisitorNode<UnaryExpression> _ignored;

        public IgnoreBoxingVisitorNodeFactory(IVisitorNodeFactory source)
        {
            _source = source;
            _ignored = source.Cast();
        }

        public IVisitorNode OfType(ExpressionType nodeType) 
            => new IgnoreNode(_ignored, _source.OfType(nodeType));

        public IVisitorNode<TExpression> OfType<TExpression>(ExpressionType? nodeType = null)
            where TExpression : Expression
            => new IgnoreNode<TExpression>(_ignored, _source.OfType<TExpression>(nodeType));

        public IVisitorNode OneOf(params IVisitorNode[] children)
            => new IgnoreNode(_ignored, _source.OneOf(children));
        
    }

    private class IgnoreNode : ExpressionTreeEvaluator.VisitorNodeBase
    {
        private readonly IVisitorNode _ignored;
        private readonly IVisitorNode _inner;

        public IgnoreNode(IVisitorNode ignored, IVisitorNode inner)
        {
            _ignored = ignored;
            _inner = inner;
        }
        
        // TODO this doesn't handle conditions or children
        public override IExpressionVisitorNode ToVisitor()
            => new ProxyVisitor((self, context) =>
            {
                context.Try(copy => _ignored.ToVisitor().Visit(copy));

                _inner.ToVisitor().Visit(context);

                if (context.HasRejection)
                {
                    context.Reject(self);
                    return null;
                }
                
                context.Accept(self);
                return null;
            });
    }

    private class IgnoreNode<T> : IgnoreNode, IVisitorNode<T>
    {
        private readonly IVisitorNode<T> _innerT;

        public IgnoreNode(IVisitorNode ignored, IVisitorNode<T> inner) 
            : base(ignored, inner)
        {
            _innerT = inner;
        }

        public IVisitorNode<T> Where(Func<T, bool> predicate, string predicateExpression = "")
            => _innerT.Where(predicate, predicateExpression);

        public new IVisitorNode<T> HavingChild(IVisitorNode child)
            => _innerT.HavingChild(child);

        public new IVisitorNode<T> HavingChildren(IVisitorNode[] children)
            => _innerT.HavingChildren(children);

        public IVisitorNode<T> HavingChildren(Func<T, IVisitorNode[]> buildChildren)
            => _innerT.HavingChildren(buildChildren);

        public IVisitorNode<T> WithEachChildBeing<TChild>(Func<T, IEnumerable<TChild>> findChildren, Func<TChild, IVisitorNode> buildChildren)
            => _innerT.WithEachChildBeing(findChildren, buildChildren);
    }
}
