using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public class VisitChildrenStageBuilder : VisitationStageBuilder, IVisitChildrenStageBuilder
{
    public VisitChildrenStageBuilder()
    {
        BeforeLeaving(_ => new RejectIfAnyBehaviorFailed());
    }

    public void AddChildren(Func<IEnumerable<IVisitorFactory>> getChildren)
        => AddBehaviors(n => getChildren().Select(child => new Visit(child, n)));
}