using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Stage.Read;

public abstract partial class MediaBehavior
{
    public class ReadCurrent : IAfterEnteringBehavior
    {
        public IStageContext Perform(IStageContext current)
            => new StageContext(current.TapeHead);
    }
}
