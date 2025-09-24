using TreeVal.Media;

namespace TreeVal.Eval;

public interface IVisitationStageBuilder : IVisitationStageFactory
{
    Identity Key { get; }

    void AfterEntering(Func<Node, IAfterEnteringBehavior> afterEntering);
    void AddBehaviors(Func<Node, IEnumerable<IBehavior>> getBehaviors);
    void BeforeLeaving(Func<Node, IBeforeLeavingBehavior> getBehavior);

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
