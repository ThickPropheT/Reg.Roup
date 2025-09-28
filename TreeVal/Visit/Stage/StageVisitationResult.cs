namespace TreeVal.Visit.Stage;

public class StageVisitationResult : VisitationResult
{
    public IStageContext StageContext { get; }

    public Exception? Error { get; init; }

    public StageVisitationResult(IStageContext stageContext)
    {
        StageContext = stageContext;
    }

    public static StageVisitationResult ForRejection(IStageContext stageContext)
        => new(stageContext);

    public static StageVisitationResult ForError(IStageContext stageContext, Exception error)
        => new(stageContext) { Error = error };
}
