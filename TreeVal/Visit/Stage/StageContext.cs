using TreeVal.Media;
using TreeVal.Visit.Behavior;

namespace TreeVal.Visit.Stage;

public class StageContext : IStageContext
{
    private readonly List<BehaviorVisitationResult> _visitations = new(1);

    public IVisitorContext VisitorContext { get; }
    public IVisitationStage Stage { get; }
    public ITapeHead TapeHead { get; }

    public IEnumerable<BehaviorVisitationResult> BehaviorVisitations => _visitations.AsReadOnly();

    public StageContext(IVisitorContext visitorContext, IVisitationStage stage, ITapeHead tapeHead)
    {
        VisitorContext = visitorContext;
        Stage = stage;
        TapeHead = tapeHead;
    }

    public StageContext(IStageContext current)
    {
        VisitorContext = current.VisitorContext;
        Stage = current.Stage;
        TapeHead = current.TapeHead;
    }

    public IBehaviorContext CreateBehaviorContext()
        => new BehaviorContext(this);

    public void RecordVisitation(BehaviorVisitationResult result)
        => _visitations.Add(result);
}
