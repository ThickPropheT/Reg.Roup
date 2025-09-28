namespace TreeVal.Visit.Behavior;

public class BehaviorVisitationResult : VisitationResult
{
    public IBehaviorContext BehaviorContext { get; }

    public BehaviorVisitationResult(IBehaviorContext behaviorContext)
    {
        BehaviorContext = behaviorContext;
    }

    public static BehaviorVisitationResult ForError(IBehaviorContext behaviorContext, Exception error)
        => new(behaviorContext) { Error = error };
}
