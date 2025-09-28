using TreeVal.Visit.Stage;

namespace TreeVal.Visit.Behavior;

public class BehaviorContext
{
    public IStageContext StageContext { get; }

    public VisitationResult? VisitationResult { get; private set; }

    public BehaviorContext(IStageContext stageContext)
    {
        StageContext = stageContext;
    }

    public void RecordResult(VisitationResult result)
        => VisitationResult = result;
}
