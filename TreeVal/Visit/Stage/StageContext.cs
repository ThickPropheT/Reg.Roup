using TreeVal.Media;
using TreeVal.Visit.Behavior;

namespace TreeVal.Visit.Stage;

public class StageContext : IStageContext
{
    private readonly List<BehaviorVisitationResult> _visitations = new(1);

    public IVisitorContext VisitorContext { get; }
    public ITapeHead TapeHead { get; }

    public IEnumerable<BehaviorVisitationResult> BehaviorVisitations => _visitations.AsReadOnly();

    public StageContext(IVisitorContext visitorContext, ITapeHead tapeHead)
    {
        TapeHead = tapeHead;
        VisitorContext = visitorContext;
    }

    public IBehaviorContext CreateBehaviorContext()
        => new BehaviorContext(this);

    public void RecordVisitation(BehaviorVisitationResult result)
        => _visitations.Add(result);
}
