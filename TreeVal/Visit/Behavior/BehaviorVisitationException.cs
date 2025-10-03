namespace TreeVal.Visit.Behavior;

public class BehaviorVisitationException : VisitationException
{
    private BehaviorVisitationException(Exception inner, IVisitorContext visitorContext, VisitationResult result)
        : base(inner, visitorContext, result)
    {
    }

    public static VisitationException ForError(Exception inner, IBehaviorContext behaviorContext)
        => inner as VisitationException
           ?? new BehaviorVisitationException(
               inner,
               behaviorContext.VisitorContext,
               behaviorContext.VisitationResult
           );
}
