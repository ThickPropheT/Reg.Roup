using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class AcceptChildrenEvaluatorNode : EvaluatorNode
{
    private readonly Expression _parent;

    public override VisitationContext.EvaluationStrategy ChildEvaluationStrategy { get; }

    public AcceptChildrenEvaluatorNode(IEnumerable<ICondition> conditions, Expression parent)
        : base(conditions, [])
    {
        _parent = parent;
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.From(EvaluateChildren);
    }

    private void EvaluateChildren(VisitationContext context, Expression current)
    {
        var tape = LinearExpressionTreeRecorder.RecordVisitationOf(_parent).ToList();
        tape.Remove(_parent);

        while (tape.Contains(current))
        {
            tape.Remove(current);

            var c = context.Head.PeekForward();

            // if can't move forward
            if (c == null)
            {
                // if there are children left on the tape
                if (tape.Any())
                {
                    context.Reject(this);
                }

                return;
            }

            current = c;
            context.Head.MoveForward();
        }
    }
}
