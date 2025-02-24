using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace TreeVal;

public static class LinearExpressionTreeRecorder
{
    public static IEnumerable<Expression?> RecordVisitationOf(Expression? node)
    {
        var visitor = new Visitor();
        visitor.Visit(node);
        return visitor.Tape;
    }
        
    private class Visitor : ExpressionVisitor
    {
        // TODO consider revisiting initial capacity
        public readonly List<Expression?> Tape = new(1);

        [return: NotNullIfNotNull("node")]
        public override Expression? Visit(Expression? node)
        {
            // TODO idk why this happens but it ain't helpin shit
            if (node == null)
            {
                return null;
            }
            
            Tape.Add(node);
            return base.Visit(node);
        }
    }
}
