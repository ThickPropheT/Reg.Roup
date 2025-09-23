namespace TreeVal.Eval;

public class BehaviorContext
{
    public IStageContext StageContext { get; }

    public VisitationResult VisitationResult { get; }

    public BehaviorContext(IStageContext stageContext)
    {
        StageContext = stageContext;
    }

    public void RecordResult(VisitationResult result)
    {
    }
}
