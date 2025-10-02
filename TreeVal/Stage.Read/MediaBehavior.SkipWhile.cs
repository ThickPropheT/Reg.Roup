using TreeVal.Media;
using TreeVal.Visit;
using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Stage.Read;

public abstract partial class MediaBehavior
{
    public class SkipWhile : IAfterEnteringBehavior
    {
        private readonly Func<Node, bool> _predicate;

        public SkipWhile(Func<Node, bool> predicate)
        {
            _predicate = predicate;
        }

        public IStageContext Perform(IBehaviorContext behaviorContext)
        {
            var stageContext = behaviorContext.StageContext;
            var tapeHead = stageContext.TapeHead;

            Node node;

            try
            {
                do
                {
                    node = tapeHead.MoveForward();
                } while (_predicate(node));
            }
            catch (IndexOutOfRangeException ex)
            {
                throw VisitationException.BehaviorError(ex, behaviorContext);
            }

            return stageContext;
        }
    }
}
