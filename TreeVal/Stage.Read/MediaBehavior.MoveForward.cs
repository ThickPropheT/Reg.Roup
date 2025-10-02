using TreeVal.Visit;
using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Stage.Read;

public abstract partial class MediaBehavior
{
    public class MoveForward : IAfterEnteringBehavior
    {
        public IStageContext Perform(IBehaviorContext behaviorContext)
        {
            try
            {
                behaviorContext.TapeHead.MoveForward();
                return behaviorContext.StageContext;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw VisitationException.BehaviorError(ex, behaviorContext);
            }
        }
    }
}
