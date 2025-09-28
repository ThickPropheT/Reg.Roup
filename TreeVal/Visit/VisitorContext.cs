using TreeVal.Media;
using TreeVal.Visit.Stage;

namespace TreeVal.Visit;

public class VisitorContext : IVisitorContext
{
    private readonly List<StageVisitationResult> _visitations = new(1);

    public ITapeHead TapeHead { get; }

    public IEnumerable<StageVisitationResult> StageVisitations => _visitations.AsReadOnly();

    public VisitorContext(ITapeHead tapeHead)
    {
        TapeHead = tapeHead;
    }

    public IStageContext CreateStageContext()
        => new StageContext(this, TapeHead);

    public void RecordVisitation(StageVisitationResult result)
        => _visitations.Add(result);
}
