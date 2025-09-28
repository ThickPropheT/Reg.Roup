using TreeVal.Media;
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

    public IStageContext Visit(IStageContext stageContext)
    {
        var nextStageContext = PerformBehavior(
            stageContext,
            n => AfterEntering(n),
            (behavior, behaviorContext) => behavior.Perform(behaviorContext)
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
        Func<TBehavior, IBehaviorContext, IStageContext> performBehavior
    )
    {
        var behaviorContext = stageContext.CreateBehaviorContext();
        var behavior = getBehavior(stageContext.TapeHead.Read());

        try
        {
            var result = performBehavior(behavior, behaviorContext);

            stageContext.RecordVisitation(new BehaviorVisitationResult(behaviorContext));

            return result;
        }
        catch (Exception ex)
        {
            stageContext.RecordVisitation(BehaviorVisitationResult.ForError(behaviorContext, ex));
        }

        return stageContext;
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
