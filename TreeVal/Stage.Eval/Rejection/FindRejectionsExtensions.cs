using TreeVal.Stage.Children;
using TreeVal.Visit;
using TreeVal.Visit.Stage;

namespace TreeVal.Stage.Eval.Rejection;

public static class FindRejectionsExtensions
{
    public static IEnumerable<VisitationResult> FindRejections(this IVisitorContext visitorContext)
        => visitorContext.StageVisitations.SelectMany(stageResult => stageResult.FindRejections());

    public static IEnumerable<VisitationResult> FindRejections(this StageVisitationResult stageResult)
    {
        if (stageResult.Status == EvaluationStatus.Rejected)
            yield return stageResult;

        // foreach (var behaviorResult in stageResult.BehaviorVisitations
        //              .Where(b => b.Error != null
        //                          || b.VisitationResult.Error != null
        //                          || b.VisitationResult is ConditionEvaluationResult
        //                          {
        //                              Status: EvaluationStatus.Rejected
        //                          }))
        // {
        //     yield return behaviorResult;
        // }

        foreach (var behaviorResult in stageResult.BehaviorVisitations)
        {
            if (behaviorResult.Error != null)
            {
                yield return behaviorResult;
            }

            var visitationResult = behaviorResult.VisitationResult;

            if (visitationResult.Error != null
                || visitationResult is ConditionEvaluationResult { Status: EvaluationStatus.Rejected })
            {
                yield return behaviorResult.VisitationResult;
                continue;
            }

            if (visitationResult is not ChildVisitationResult childResult)
                continue;

            foreach (var childRejection in childResult.VisitorContext.FindRejections())
            {
                yield return childRejection;
            }
        }
    }
}
