using TreeVal.Visit.Stage;

namespace TreeVal.Visit.Behavior.AfterEntering;

public static class PerformBehaviorExtensions
{
    public static IStageContext PerformAndRecordResult(
        this IAfterEnteringBehavior afterEntering,
        IBehaviorContext behaviorContext
    )
    {
        try
        {
            var stageContext = afterEntering.Perform(behaviorContext);
            
            behaviorContext.RecordResult(new AfterEnteringVisitationResult(behaviorContext));
            
            return stageContext;
        }
        catch (Exception ex)
        {
            behaviorContext.RecordResult(AfterEnteringVisitationResult.ForError(behaviorContext, ex));
            throw BehaviorVisitationException.ForError(ex, behaviorContext);
        }
    }
}
