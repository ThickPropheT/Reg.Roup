using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Stage.Eval.Rejection;

public class RejectIfAllBehaviorsFailed : IBeforeLeavingBehavior
{
    public void Perform(IBehaviorContext behaviorContext)
    {
        var rejection = TryFindRejection(behaviorContext.StageContext);

        if (rejection == null)
            return;

        behaviorContext.VisitorContext.RecordVisitation(rejection);
    }

    private static StageVisitationResult? TryFindRejection(IStageContext stageContext)
    {
        try
        {
            return stageContext.BehaviorVisitations
                .All(v => v.VisitationResult is ConditionEvaluationResult
                {
                    Context.Status: EvaluationStatus.Rejected
                })
                ? StageVisitationResult.ForRejection(stageContext, "At least one child behavior must be accepted")
                : null;
        }
        catch (Exception ex)
        {
            // I don't expect the above will ever throw, but the desired outcome is clear, so I'm including this.
            return StageVisitationResult.ForError(stageContext, ex);
        }
    }
}
