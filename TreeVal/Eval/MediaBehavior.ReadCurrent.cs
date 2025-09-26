namespace TreeVal.Eval;

public abstract partial class MediaBehavior
{
    public class ReadCurrent : IAfterEnteringBehavior
    {
        public IStageContext Perform(IStageContext current)
            => new StageContext(current.TapeHead);
    }
}