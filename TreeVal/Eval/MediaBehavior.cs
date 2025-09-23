namespace TreeVal.Eval;

public static class MediaBehavior
{
    public class MoveForward : IAfterEnteringBehavior
    {
        public IStageContext Perform(IStageContext current)
        {
            current.TapeHead.MoveForward();
            
            return new StageContext(current.TapeHead);
        }
    }

    public class ReadCurrent : IAfterEnteringBehavior
    {
        public IStageContext Perform(IStageContext current)
            => new StageContext(current.TapeHead);
    }
}
