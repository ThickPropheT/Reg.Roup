using TreeVal.Visit.Stage;

namespace TreeVal.Visit.Behavior.AfterEntering;

public interface IAfterEnteringBehavior
{
    IStageContext Perform(IBehaviorContext behaviorContext);
}
