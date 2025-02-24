using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class AnyOneEvaluatorExtensions
{
    public static IEvaluatorBuilder AnyOne(this VisitorNodeFactory factory)
        => factory.OfType<Expression>();

    public static IEvaluatorBuilder AcceptChildren(this VisitorNodeFactory factory, Expression parent)
        => new AcceptChildrenNode(parent);

    private class AcceptChildrenNode : EvaluatorBuilderBase
    {
        private readonly Expression _parent;

        public AcceptChildrenNode(Expression parent)
        {
            _parent = parent;
        }

        // TODO this doesn't handle conditions or children
        public override IEvaluatorNode ToEvaluator()
            => new ProxyEvaluator((self, context) =>
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

                    if (!context.CanMoveForward())
                    {
                        if (!tape.Any())
                        {
                            context.Accept(self);
                        }
                        else
                        {
                            context.Reject(self);
                        }

                        return null;
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
