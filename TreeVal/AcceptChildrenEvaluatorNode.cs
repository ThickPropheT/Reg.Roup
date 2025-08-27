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
        HeadMovementStrategy = VisitationContext.MovementStrategy.From(MoveHead);
        ChildEvaluationStrategy = VisitationContext.EvaluationStrategy.From(EvaluateChildren);
    }

    private Expression? MoveHead(VisitationContext _, TapeHead head)
    {
        var tape = LinearExpressionTreeRecorder.RecordVisitationOf(_parent).ToList();
        tape.Remove(_parent);

        // there weren't any children, actually.
        if (!tape.Any())
        {
            return null;
        }

        // TODO should we just pass this in like all the other methods around here?
        var current = head.Read();

        // - if current != _parent, then some number of children of _parent
        //     have already been processed by another evaluator.
        // - if current == _parent, then no children of _parent have been
        //     evaluated yet.
        if (current == _parent)
        {
            // no need to check to see if we can move forward.
            // if we're here, then tape is not empty and therefore,
            // _parent has children left on head's tape.
            current = head.MoveForward();
        }

        while (tape.Contains(current))
        {
            tape.Remove(current);

            current = head.PeekForward();

            // if the head has run out of tape, bail out. 
            if (current == null)
            {
                // TODO
                //  should we just let this kind of error be thrown by the head itself?
                Debug.Assert(!tape.Any(),
                    "expected context.Head to be able to move forward. _parent has unvisited child nodes.");
                break;
            }

            // otherwise, move forward.
            head.MoveForward();
        }

        Debug.Assert(!tape.Any(), "_parent has unvisited child nodes.");
        return current;
    }

    private bool EvaluateChildren(VisitationContext context, Expression current, IEvaluatorNode _)
    {
        throw new SkepticalException("Should we even be here rn?");
    }
}
