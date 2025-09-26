namespace TreeVal.Eval;

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
