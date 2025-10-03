namespace TreeVal.Visit.Stage;

public class StageVisitationException : VisitationException
{
    private StageVisitationException(Exception inner, IVisitorContext visitorContext, VisitationResult result)
        : base(inner, visitorContext, result)
    {
    }

    public static VisitationException ForError(Exception inner, StageVisitationResult stageResult)
        => inner as VisitationException
           ?? new StageVisitationException(
               inner,
               stageResult.StageContext.VisitorContext,
               stageResult
           );
}
