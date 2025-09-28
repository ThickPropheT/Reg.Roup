using TreeVal.Visit.Stage;

namespace TreeVal.Visit.Behavior;

public class BehaviorContext : IBehaviorContext
{
    private VisitationResult? _result;

    public IStageContext StageContext { get; }

    public VisitationResult VisitationResult => _result ?? new VisitationResult();

    public BehaviorContext(IStageContext stageContext)
    {
        StageContext = stageContext;
    }

    public void RecordResult(VisitationResult result)
        => _result = result;
}
