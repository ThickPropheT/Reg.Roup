using TreeVal.Media;
using TreeVal.Visit.Stage;

namespace TreeVal.Visit.Behavior;

public class BehaviorContext : IBehaviorContext
{
    private VisitationResult? _result;

    public IVisitorContext VisitorContext => StageContext.VisitorContext;
    public IStageContext StageContext { get; }
    public object Behavior { get; }

    public ITapeHead TapeHead => StageContext.TapeHead;

    public VisitationResult VisitationResult => _result ?? new VisitationResult();

    public BehaviorContext(IStageContext stageContext, object behavior)
    {
        StageContext = stageContext;
        Behavior = behavior;
    }

    public void RecordResult(VisitationResult result)
        => _result = result;
}
