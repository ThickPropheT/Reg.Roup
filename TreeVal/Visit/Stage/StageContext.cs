using TreeVal.Media;
using TreeVal.Visit.Behavior;

namespace TreeVal.Visit.Stage;

public class StageContext : IStageContext
{
    private readonly List<BehaviorContext> _visitations = new(1);

    public ITapeHead TapeHead { get; }

    public IEnumerable<BehaviorContext> Visitations => _visitations.AsReadOnly();

    public StageContext(ITapeHead tapeHead)
    {
        TapeHead = tapeHead;
    }

    public void RecordVisitation(BehaviorContext visitation)
        => _visitations.Add(visitation);
}
