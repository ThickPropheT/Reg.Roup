using System.Diagnostics;
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

    private void EvaluateChildren(VisitationContext context, Expression current, IEvaluatorNode _)
    {
        var tape = LinearExpressionTreeRecorder.RecordVisitationOf(_parent).ToList();
        tape.Remove(_parent);

        while (tape.Contains(current))
        {
            tape.Remove(current);

            var next = context.Head.PeekForward();

            // if can't move forward
            if (next == null)
            {
                // TODO
                //  can this situation even happen other than by something being really broken?
                //  handling this case is fine, but maybe throw ex instead?
                Debug.Assert(!tape.Any(), "expected context.Head to be able to move forward. _parent has unvisited child nodes.");
                return;
            }

            current = next;
            context.Head.MoveForward();
        }
    }
}
