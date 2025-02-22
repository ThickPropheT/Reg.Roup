using System.Linq.Expressions;

namespace Treeval;

public static class AnyVisitorNodeExtensions
{
    public static IVisitorNode Any(this IVisitorNodeFactory factory)
        => factory.OfType<Expression>();

    public static IVisitorNode AcceptChildren(this IVisitorNodeFactory factory, Expression parent)
        => new AcceptChildrenNode(parent);

    private class AcceptChildrenNode : ExpressionVisitorNodeFactory.VisitorNodeBase
    {
        private readonly Expression _parent;

        public AcceptChildrenNode(Expression parent)
        {
            _parent = parent;
        }

        // TODO this doesn't handle conditions or children
        public override IExpressionVisitorNode ToVisitor()
            => new ProxyVisitor((self, context) =>
            {
                var tape = LinearExpressionTreeRecorder.RecordVisitationOf(_parent).ToList();
                tape.Remove(_parent);

                Expression? current = null;

                do
                {
                    if (current != null)
                    {
                        tape.Remove(current);
                    }

                    current = context.MoveForward();
                } while (tape.Contains(current));

                // if current wasn't a child of _parent, then move back one
                context.MoveBackward();

                context.Accept(self);
                return null;
            });
    }
}
