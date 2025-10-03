using TreeVal.Media;
using TreeVal.Visit.Behavior;
using TreeVal.Visit.Behavior.AfterEntering;

namespace TreeVal.Visit.Stage;

public class VisitationStage : IVisitationStage
{
    private readonly IEnumerable<Func<Node, IEnumerable<IBehavior>>> _behaviors;

    public required string CreatedBy { get; init; }
    public string? CreationSite { get; init; }

    public Func<Node, IAfterEnteringBehavior> AfterEntering { get; }
    public Func<Node, IBeforeLeavingBehavior>? BeforeLeaving { get; }

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

    public IStageContext Visit(IStageContext stageContext)
    {
        stageContext = PerformBehavior(
            stageContext,
            n => AfterEntering(n),
            (behavior, behaviorContext) => behavior.PerformAndRecordResult(behaviorContext));

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

        return stageContext;
    }

    private static IStageContext PerformBehavior<TBehavior>(
        IStageContext stageContext,
        Func<Node, TBehavior> getBehavior,
        Func<TBehavior, IBehaviorContext, IStageContext> performBehavior
    )
    {
        var behavior = getBehavior(stageContext.TapeHead.Read());
        var behaviorContext = stageContext.CreateBehaviorContext(behavior);

        try
        {
            stageContext = performBehavior(behavior, behaviorContext);
            stageContext.RecordVisitation(new BehaviorVisitationResult(behaviorContext));

            return stageContext;
        }
        catch (Exception ex)
        {
            stageContext.RecordVisitation(BehaviorVisitationResult.ForError(behaviorContext, ex));
            throw BehaviorVisitationException.ForError(ex, behaviorContext);
        }
    }

    private static void PerformBehavior<TBehavior>(
        IStageContext stageContext,
        Func<Node, TBehavior> getBehavior,
        Action<TBehavior, IBehaviorContext> performBehavior
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
