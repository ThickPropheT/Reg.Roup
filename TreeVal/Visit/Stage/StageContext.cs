using TreeVal.Media;
using TreeVal.Visit.Behavior;

namespace TreeVal.Visit.Stage;

public interface IStageContext
{
    ITapeHead TapeHead { get; }

    IEnumerable<BehaviorContext> Visitations { get; }

    void RecordVisitation(BehaviorContext visitation);
}

public class StageContext : IStageContext
{
    public ITapeHead TapeHead { get; }

    public IEnumerable<VisitationResult> VisitationResults { get; }
    public IEnumerable<BehaviorContext> Visitations { get; }

    public StageContext(ITapeHead tapeHead)
    {
        TapeHead = tapeHead;
    }

    public void RecordVisitation(BehaviorContext visitation)
    {
    }

    public void RecordResult(VisitationResult result)
    {
    }
}
