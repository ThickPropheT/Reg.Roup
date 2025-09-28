using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Stage.Read;

public abstract partial class MediaBehavior
{
    public class MoveForward : IAfterEnteringBehavior
    {
        public IStageContext Perform(IStageContext current)
        {
            current.TapeHead.MoveForward();

            return new StageContext(current.TapeHead);
        }
    }
}
