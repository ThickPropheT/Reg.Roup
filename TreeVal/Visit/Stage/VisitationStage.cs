using TreeVal.Media;
using TreeVal.Stage.Eval;
using TreeVal.Visit.Behavior;

namespace TreeVal.Visit.Stage;

public class VisitationStage : IVisitationStage
{
    public Func<Node, IAfterEnteringBehavior> AfterEntering { get; }
    public Func<Node, IBeforeLeavingBehavior>? BeforeLeaving { get; }

    private readonly IEnumerable<Func<Node, IEnumerable<IBehavior>>> _behaviors;

    public VisitationStage(
        IEnumerable<Func<Node, IEnumerable<IBehavior>>> behaviors,
        Func<Node, IAfterEnteringBehavior> afterEntering,
        Func<Node, IBeforeLeavingBehavior>? beforeLeaving = null
    )
    {
        AfterEntering = afterEntering;
        _behaviors = behaviors;
        BeforeLeaving = beforeLeaving;
    }

    public IStageContext Visit(TapeHead head, IStageContext stageContext)
    {
        var nextStageContext = PerformBehavior(
            stageContext,
            n => AfterEntering(n),
            (behavior, _) => behavior.Perform(stageContext)
        );

        foreach (var behavior in _behaviors.SelectMany(getBehavior => getBehavior(stageContext.TapeHead.Read())))
        {
            PerformBehavior(
                stageContext,
                _ => behavior,
                (_, behaviorContext) => behavior.Perform(behaviorContext)
            );
        }

        if (BeforeLeaving != null)
        {
            PerformBehavior(
                stageContext,
                n => BeforeLeaving(n),
                (behavior, behaviorContext) => behavior.Perform(behaviorContext)
            );
        }

        return nextStageContext;
    }

    private static IStageContext PerformBehavior<TBehavior>(
        IStageContext stageContext,
        Func<Node, TBehavior> getBehavior,
        Func<TBehavior, BehaviorContext, IStageContext> performBehavior
    )
    {
        var behavior = getBehavior(stageContext.TapeHead.Read());
        var behaviorContext = new BehaviorContext(stageContext);

        try
        {
            return performBehavior(behavior, behaviorContext);
        }
        catch (Exception ex)
        {
            // TODO
            //  visitation is supposed to be divorced from evaluation now,
            //  we should probably find a different way to do this error handling.
            behaviorContext.RecordResult(new RejectionResult(ex));
        }
        finally
        {
            stageContext.RecordVisitation(behaviorContext);
        }

        return stageContext;
    }

    private static void PerformBehavior<TBehavior>(
        IStageContext stageContext,
        Func<Node, TBehavior> getBehavior,
        Action<TBehavior, BehaviorContext> performBehavior
    )
        => PerformBehavior(
            stageContext,
            getBehavior,
            (behavior, behaviorContext) =>
            {
                performBehavior(behavior, behaviorContext);
                return stageContext;
            });
}
