using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Eval.AcceptChildren;

public class AcceptChildrenNodeEvaluator : Visitor
{
    private readonly Node _parent;
    private readonly IVisitationRecorder _recorder;

    public AcceptChildrenNodeEvaluator(
        IEnumerable<Func<Node, IEnumerable<ICondition>>> conditionLookups,
        Node parent,
        IVisitationRecorder recorder
    )
        : base(conditionLookups, [])
    {
        _parent = parent;
        _recorder = recorder;
        HeadMovementStrategy = VisitationContext.MovementStrategy.From(MoveHead);
    }

    private Node? MoveHead(VisitationContext _, TapeHead head, INodeEvaluation evaluation)
    {
        var childTape = _recorder.RecordVisitationOf(_parent).ToList();
        childTape.Remove(_parent);

        if (!childTape.Any())
        {
            // there weren't any children, actually.
            return null;
        }

        var current = head.Read();

        var past = head
            .ReadToStart()
            .TakeWhile(node => node != _parent);

        foreach (var node in past)
        {
            if (node == current)
                // current is used for the ACTUAL work below, so we'll leave it in for now.
                continue;

            // TODO
            //  - this assumes that we got to this point linearly along the tape.
            //     seems like a safe enough assumption, but the head CAN move backward as well.
            //     we can't rely on walking up the evaluation hierarchy in search of
            //     the parent's children, because they could have been evaluated in
            //     a sibling evaluator whose results have not yet been recorded on
            //     the parent evaluation.
            //  - if this becomes an issue, consider adding a second TapeHead
            //     that be used to write an absolute log of evaluated nodes in real time.
            //  - alternatively, modify All & Any evaluation strategies to record
            //     results of child evaluations immediately following each evaluation,
            //     rather than after completion of all child evaluations.
            childTape.Remove(node);
        }

        if (!childTape.Any())
        {
            // somebody beat us to it.
            return null;
        }

        // - if current != _parent, then some number of children of _parent
        //     have already been processed by another evaluator.
        // - if current == _parent, then no children of _parent have been
        //     evaluated yet.
        if (current == _parent)
        {
            // no need to check to see if we can move forward.
            // if we're here, then childTape is not empty and therefore,
            // _parent has children left on head's tape.
            current = head.MoveForward();
        }

        var isOnTape = childTape.Contains(current);

        while (isOnTape)
        {
            childTape.Remove(current);

            current = head.PeekForward();

            // if the head has run out of tape, bail out.
            if (current == null)
            {
                // TODO
                //  should we just let this kind of error be thrown by the head itself?
                System.Diagnostics.Debug.Assert(
                    !childTape.Any(),
                    "DBG: expected context.Head to be able to move forward. _parent has unvisited child nodes.");
                break;
            }

            isOnTape = childTape.Contains(current);

            // if we're about to move too far forward, bail out.
            if (!isOnTape)
                break;

            // otherwise, move forward.
            head.MoveForward();
        }

        return current;
    }
}
