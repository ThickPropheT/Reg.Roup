using TreeVal.Media;

namespace TreeVal.Eval;

public interface IVisitationStageBuilder : IVisitationStageFactory
{
    void AfterEntering(Func<Node, IAfterEnteringBehavior> afterEntering);
    void AddBehaviors(Func<Node, IEnumerable<IBehavior>> getBehaviors);
    void BeforeLeaving(Func<Node, IBeforeLeavingBehavior> getBehavior);
}
