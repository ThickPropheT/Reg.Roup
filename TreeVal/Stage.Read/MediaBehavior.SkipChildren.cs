using System.Diagnostics;
using TreeVal.Media;
using TreeVal.Visit;
using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Stage.Read;

public abstract partial class MediaBehavior
{
    public class SkipChildren : IAfterEnteringBehavior
    {
        private readonly Node _parent;
        private readonly IVisitationRecorder _recorder;

        public SkipChildren(Node parent, IVisitationRecorder recorder)
        {
            _parent = parent;
            _recorder = recorder;
        }

        public IStageContext Perform(IBehaviorContext behaviorContext)
        {
            var stageContext = behaviorContext.StageContext;
            var head = stageContext.TapeHead;

            var childTape = _recorder.RecordVisitationOf(_parent).ToList();
            childTape.Remove(_parent);

            if (!childTape.Any())
            {
                throw new NotImplementedException(
                    "This isn't 1:1 with how things used to work. this used to return null.");

                // there weren't any children, actually.
                return stageContext;
            }

            var current = head.Read();

            var past = head
                .ReadToStart()
                .TakeWhile(node => node != _parent);

            try
            {
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
                    throw new NotImplementedException(
                        "This isn't 1:1 with how things used to work. this used to return null.");

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
                        Debug.Assert(
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
            }
            catch (IndexOutOfRangeException ex)
            {
                throw VisitationException.BehaviorError(ex, behaviorContext);
            }

            return new StageContext(stageContext);
        }
    }
}
