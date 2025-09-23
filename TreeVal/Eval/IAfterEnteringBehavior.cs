namespace TreeVal.Eval;

public interface IAfterEnteringBehavior
{
    IStageContext Perform(IStageContext current);
}
