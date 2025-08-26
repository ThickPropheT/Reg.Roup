using System.Diagnostics;
using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class AcceptChildrenEvaluatorNode : EvaluatorNode
{
    private readonly Expression _parent;

    public override VisitationContext.MovementStrategy HeadMovementStrategy { get; }
    public override VisitationContext.EvaluationStrategy ChildEvaluationStrategy { get; }

    public AcceptChildrenEvaluatorNode(IEnumerable<ICondition> conditions, Expression parent)
        : base(conditions, [])
    {
        _parent = parent;
        HeadMovementStrategy = VisitationContext.MovementStrategy.TryMoveForward;
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.From(EvaluateChildren);
    }

    private bool EvaluateChildren(VisitationContext context, Expression current, IEvaluatorNode _)
    {
        // TODO note that this is a list and not a queue. removing things just cherry picks them out
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
                break;
            }

            current = next;
            context.Head.MoveForward();
        }

        // TODO
        //  this can 100% happen. not sure if it's a problem or not, but it probably isn't helping.
        Debug.Assert(!tape.Any(), "_parent has unvisited child nodes.");
        return true;
    }
}
