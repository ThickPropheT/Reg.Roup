using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Eval.AcceptChildren;

using System.Diagnostics;

public class AcceptChildrenNodeEvaluator : NodeEvaluator
{
    private readonly Node _parent;
    private readonly IVisitationRecorder _recorder;

    public AcceptChildrenNodeEvaluator(IEnumerable<ICondition> conditions, Node parent, IVisitationRecorder recorder)
        : base(conditions, [])
    {
        _parent = parent;
        _recorder = recorder;
        HeadMovementStrategy = VisitationContext.MovementStrategy.From(MoveHead);
    }

    private Node? MoveHead(VisitationContext _, TapeHead head, INodeEvaluation evaluation)
    {
        var tape = _recorder.RecordVisitationOf(_parent).ToList();
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
                Debug.Assert(
                    !tape.Any(),
                    "DBG: expected context.Head to be able to move forward. _parent has unvisited child nodes.");
                break;
            }

            // otherwise, move forward.
            head.MoveForward();
        }

        Debug.Assert(!tape.Any(), "DBG: _parent has unvisited child nodes.");
        return current;
    }
}
