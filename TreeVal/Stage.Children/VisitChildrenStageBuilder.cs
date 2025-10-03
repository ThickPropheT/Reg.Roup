using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;
using TreeVal.Stage.Eval.Rejection;
using TreeVal.Visit;
using TreeVal.Visit.Behavior;

namespace TreeVal.Stage.Children;

public class VisitChildrenStageBuilder : VisitationStageBuilder, VisitChildrenStage.IBuilder
{
    public VisitChildrenStageBuilder()
        : base(VisitChildrenStage.Key)
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
            IVisitor visitor;

            try
            {
                visitor = _child.CreateVisitor(_node);
            }
            catch (Exception ex)
            {
                behaviorContext.RecordResult(new VisitorResolutionErrorResult(ex));
                throw BehaviorVisitationException.ForError(ex, behaviorContext);
            }

            var context = new VisitorContext(behaviorContext.TapeHead, visitor);

            try
            {
                visitor.Visit(context);

                behaviorContext.RecordResult(new ChildVisitationResult(context));
            }
            catch (Exception ex)
            {
                behaviorContext.RecordResult(ChildVisitationResult.ForError(context, ex));
                throw BehaviorVisitationException.ForError(ex, behaviorContext);
            }
        }
    }
}
