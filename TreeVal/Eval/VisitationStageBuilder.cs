using TreeVal.Media;

namespace TreeVal.Eval;

public class VisitationStageBuilder : IVisitationStageBuilder
{
    private Func<Node, IAfterEnteringBehavior>? _afterEntering;
    private Func<Node, IBeforeLeavingBehavior>? _beforeLeaving;

    private readonly List<Func<Node, IEnumerable<IBehavior>>> _behaviorFactories = [];

    public IVisitationStageBuilder.Identity Key { get; }

    public VisitationStageBuilder(IVisitationStageBuilder.Identity? key)
    {
        Key = key ?? new IVisitationStageBuilder.Identity<IVisitationStageBuilder>();
    }

    public void AfterEntering(Func<Node, IAfterEnteringBehavior> afterEntering)
        => _afterEntering = afterEntering;

    public void AddBehaviors(Func<Node, IEnumerable<IBehavior>> getBehaviors)
        => _behaviorFactories.Add(getBehaviors);

    public void BeforeLeaving(Func<Node, IBeforeLeavingBehavior> getBehavior)
        => _beforeLeaving = getBehavior;

    public IVisitationStage CreateStage()
        => new VisitationStage(
            _behaviorFactories,
            _afterEntering ?? (_ => new MediaBehavior.ReadCurrent()),
            _beforeLeaving
        );
}
