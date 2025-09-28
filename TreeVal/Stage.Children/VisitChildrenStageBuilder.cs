using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;
using TreeVal.Stage.Eval;

namespace TreeVal.Stage.Children;

public class VisitChildrenStageBuilder : VisitationStageBuilder, IVisitChildrenStageBuilder
{
    public VisitChildrenStageBuilder()
        : base(new IVisitationStageBuilder.Identity<IVisitChildrenStageBuilder>())
    {
        BeforeLeaving((_, c) => new RejectIfAnyBehaviorFailed());
    }

    public void AddChildren(Func<IEnumerable<IVisitorFactory>> getChildren)
        => AddBehaviors(n => getChildren().Select(child => new Visit(child, n)));
}
