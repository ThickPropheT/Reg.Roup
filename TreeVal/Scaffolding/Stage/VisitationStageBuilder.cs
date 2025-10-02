using TreeVal.Media;
using TreeVal.Stage.Read;
using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Scaffolding.Stage;

public class VisitationStageBuilder : IVisitationStageBuilder
{
    private Func<Node, Func<Node, IAfterEnteringBehavior>?, IAfterEnteringBehavior>? _afterEntering;
    private Func<Node, Func<Node, IBeforeLeavingBehavior>?, IBeforeLeavingBehavior>? _beforeLeaving;

    private readonly List<Func<Node, IEnumerable<IBehavior>>> _behaviorFactories = [];

    public IVisitationStageBuilder.Identity Key { get; }

    public VisitationStageBuilder(IVisitationStageBuilder.Identity key)
    {
        Key = key;
    }

    public void AfterEntering(Func<Node, Func<Node, IAfterEnteringBehavior>?, IAfterEnteringBehavior> afterEntering)
        => _afterEntering = afterEntering;

    public void AddBehaviors(Func<Node, IEnumerable<IBehavior>> getBehaviors)
        => _behaviorFactories.Add(getBehaviors);

    public void BeforeLeaving(Func<Node, Func<Node, IBeforeLeavingBehavior>?, IBeforeLeavingBehavior> getBehavior)
        => _beforeLeaving = getBehavior;

    public IVisitationStage CreateStage()
        => new VisitationStage(
            _behaviorFactories,
            n =>
            {
                var afterEntering = _afterEntering ?? ((_, _) => new MediaBehavior.ReadCurrent());
                return afterEntering(n, null);
            },
            _beforeLeaving != null
                ? n => _beforeLeaving(n, null)
                : null
        )
        {
            CreatedBy = GetType().Name
        };
}
