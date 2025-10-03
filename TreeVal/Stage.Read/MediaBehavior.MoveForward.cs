using TreeVal.Visit.Behavior;
using TreeVal.Visit.Behavior.AfterEntering;
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
                return new StageContext(behaviorContext.StageContext);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw BehaviorVisitationException.ForError(ex, behaviorContext);
            }
        }
    }
}
