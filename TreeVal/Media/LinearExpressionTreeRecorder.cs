using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using TreeVal.__Temp__;

namespace TreeVal.Media;

public class LinearExpressionTreeRecorder : IVisitationRecorder<Expression>, IVisitationRecorder
{
    public static IEnumerable<Node<Expression>> RecordVisitationOf(Expression node)
        => new LinearExpressionTreeRecorder().RecordVisitationOf(new Node<Expression>(node));

    public IEnumerable<Node> RecordVisitationOf(Node node)
    {
        if (node.Value is not Expression e)
            throw new SkepticalException("Not confident this will ever happen.");

        return RecordVisitationOf(e);
    }

    public IEnumerable<Node<Expression>> RecordVisitationOf(Node<Expression> node)
    {
        var visitor = new Visitor();
        visitor.Visit(node.Value);
        return visitor.Tape.Select(n => new Node<Expression>(n));
    }

    private class Visitor : ExpressionVisitor
    {
        public readonly List<Expression> Tape = new(1);

        [return: NotNullIfNotNull("node")]
        public override Expression? Visit(Expression? node)
        {
            // idk why this happens, but ignoring it doesn't seem to hurt anything
            if (node == null)
                return null;

            Tape.Add(node);
            return base.Visit(node);
        }
    }
}
