using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;
using TreeVal.Stage.Eval;
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

        public void Perform(BehaviorContext behaviorContext)
        {
            throw new NotImplementedException();
            try
            {
                // _child.CreateVisitor(_node).Visit();   
            }
            catch (Exception ex)
            {
                behaviorContext.RecordResult(new RejectionResult(ex));
            }
        }
    }
}
