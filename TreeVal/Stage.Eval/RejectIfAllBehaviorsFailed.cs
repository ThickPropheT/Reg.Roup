using TreeVal.Visit.Behavior;

namespace TreeVal.Stage.Eval;

public class RejectIfAllBehaviorsFailed : IBeforeLeavingBehavior
{
    public void Perform(BehaviorContext behaviorContext)
    {
        var rejection = TryFindRejection(behaviorContext);

        if (rejection == null)
            return;

        behaviorContext.RecordResult(rejection);
    }

    private static RejectionResult? TryFindRejection(BehaviorContext context)
    {
        try
        {
            return context.StageContext.Visitations.All(v => v.VisitationResult is RejectionResult)
                ? new RejectionResult()
                : null;
        }
        catch (Exception ex)
        {
            // I don't expect the above will ever throw, but the desired outcome is clear, so I'm including this.
            return new RejectionResult(ex);
        }
    }
}
