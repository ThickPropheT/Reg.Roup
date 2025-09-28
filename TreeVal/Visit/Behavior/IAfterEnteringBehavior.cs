using TreeVal.Visit.Stage;

namespace TreeVal.Visit.Behavior;

public interface IAfterEnteringBehavior
{
    IStageContext Perform(IBehaviorContext behaviorContext);
}
