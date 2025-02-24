using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class AcceptChildrenEvaluatorNode : EvaluatorNode
{
    private readonly Expression _parent;

    public AcceptChildrenEvaluatorNode(IEnumerable<ICondition> conditions, Expression parent) 
        : base(conditions, [])
    {
        _parent = parent;
    }

    protected override void EvaluateChildren(IVisitationContext context, Expression current)
    {
        var tape = LinearExpressionTreeRecorder.RecordVisitationOf(_parent).ToList();
        tape.Remove(_parent);

        while (tape.Contains(current))
        {
            tape.Remove(current);
            
            if (!context.CanMoveForward())
            {
                if (tape.Any())
                {
                    context.Reject(this);
                }
                else
                {
                    // TODO add auto-accept and remove this
                    context.Accept(this);
                }

                return;
            }
            
            current = context.MoveForward();
        }

        // if current wasn't a child of _parent, then move back one
        context.MoveBackward();

        // TODO add auto-accept and remove this
        context.Accept(this);
    }
}
