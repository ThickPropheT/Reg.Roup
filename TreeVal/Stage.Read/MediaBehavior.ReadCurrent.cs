using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Stage.Read;

public abstract partial class MediaBehavior
{
    public class ReadCurrent : IAfterEnteringBehavior
    {
        public IStageContext Perform(IBehaviorContext behaviorContext)
            // don't move the TapeHead, just return stageContext.
            => new StageContext(behaviorContext.StageContext);
    }
}
