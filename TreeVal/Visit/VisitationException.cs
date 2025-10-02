using TreeVal.Media;
using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Visit;

public class VisitationException : Exception
{
    public ITapeHead Head { get; }

    public IVisitorContext VisitorContext { get; }
    public VisitationResult Result { get; }

    private VisitationException(Exception inner, IVisitorContext visitorContext, VisitationResult result)
        : base("Error occurred during the visitation of a node.", inner)
    {
        Head = visitorContext.TapeHead;
        VisitorContext = visitorContext;
        Result = result;
    }

    public static VisitationException BehaviorError(Exception inner, IBehaviorContext behaviorContext)
        => inner as VisitationException
           ?? new(
               inner,
               behaviorContext.VisitorContext,
               behaviorContext.VisitationResult
           );

    public static VisitationException StageError(Exception inner, StageVisitationResult stageResult)
        => inner as VisitationException
           ?? new(
               inner,
               stageResult.StageContext.VisitorContext,
               stageResult
           );
}
