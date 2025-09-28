using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Stage.Eval;

public class RejectIfAllBehaviorsFailed : IBeforeLeavingBehavior
{
    public void Perform(IBehaviorContext behaviorContext)
    {
        var stageContext = behaviorContext.StageContext;

        var rejection = TryFindRejection(stageContext);

        if (rejection == null)
            return;

        stageContext.VisitorContext.RecordVisitation(rejection);
    }

    private static StageVisitationResult? TryFindRejection(IStageContext stageContext)
    {
        try
        {
            return stageContext.BehaviorVisitations
                .All(v => v.BehaviorContext.VisitationResult is ConditionEvaluationResult
                {
                    Evaluation.Status: EvaluationStatus.Rejected
                })
                ? StageVisitationResult.ForRejection(stageContext)
                : null;
        }
        catch (Exception ex)
        {
            // I don't expect the above will ever throw, but the desired outcome is clear, so I'm including this.
            return StageVisitationResult.ForError(stageContext, ex);
        }
    }
}
