using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Stage.Read;

public abstract partial class MediaBehavior
{
    public class MoveForward : IAfterEnteringBehavior
    {
        public IStageContext Perform(IBehaviorContext behaviorContext)
        {
            var stageContext = behaviorContext.StageContext;
            stageContext.TapeHead.MoveForward();
            return stageContext;
        }
    }
}
