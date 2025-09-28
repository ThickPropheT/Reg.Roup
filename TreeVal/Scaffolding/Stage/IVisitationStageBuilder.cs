using TreeVal.Media;
using TreeVal.Visit.Behavior;

namespace TreeVal.Scaffolding.Stage;

public interface IVisitationStageBuilder : IVisitationStageFactory
{
    Identity Key { get; }

    void AfterEntering(Func<Node, Func<Node, IAfterEnteringBehavior>?, IAfterEnteringBehavior> afterEntering);
    void AddBehaviors(Func<Node, IEnumerable<IBehavior>> getBehaviors);
    void BeforeLeaving(Func<Node, Func<Node, IBeforeLeavingBehavior>?, IBeforeLeavingBehavior> getBehavior);

    public class Identity
    {
        private readonly object _identity;

        public Identity(object identity)
        {
            _identity = identity;
        }
    }

    public class Identity<T> : Identity
    {
        public Identity()
            : base(typeof(T))
        {
        }
    }
}
