using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;
using TreeVal.Stage.Eval;
using TreeVal.Visit;
using TreeVal.Visit.Behavior;

namespace TreeVal.Stage.Children;

public class VisitChildrenStageBuilder : VisitationStageBuilder, IVisitChildrenStageBuilder
{
    public VisitChildrenStageBuilder()
        : base(new IVisitationStageBuilder.Identity<IVisitChildrenStageBuilder>())
    {
        BeforeLeaving((_, _) => new RejectIfAnyBehaviorFailed());
    }

    public void AddChildren(Func<IEnumerable<IVisitorFactory>> getChildren)
        => AddBehaviors(n => getChildren().Select(child => new Visit(child, n)));

    private class Visit : IBehavior
    {
        private readonly IVisitorFactory _child;
        private readonly Node _node;

        public Visit(IVisitorFactory child, Node node)
        {
            _child = child;
            _node = node;
        }

        public void Perform(IBehaviorContext behaviorContext)
        {
            var visitorContext = new VisitorContext(behaviorContext.StageContext.TapeHead);

            try
            {
                var visitor = _child.CreateVisitor(_node);
                visitor.Visit(visitorContext);

                behaviorContext.RecordResult(new ChildVisitationResult(visitorContext));
            }
            catch (Exception ex)
            {
                behaviorContext.RecordResult(ChildVisitationResult.ForError(visitorContext, ex));
            }
        }
    }
}
