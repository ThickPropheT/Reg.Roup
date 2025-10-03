using TreeVal.Media;
using TreeVal.Visit.Behavior;
using TreeVal.Visit.Behavior.AfterEntering;

namespace TreeVal.Scaffolding.Stage;

public interface IVisitationStageBuilder : IVisitationStageFactory
{
    Identity Key { get; }
    
    string? CreatedBy { get; init; }
    string? CreationSite { get; set; }

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

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Identity) obj);
        }

        protected bool Equals(Identity other)
        {
            return _identity.Equals(other._identity);
        }

        public override int GetHashCode()
        {
            return _identity.GetHashCode();
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
