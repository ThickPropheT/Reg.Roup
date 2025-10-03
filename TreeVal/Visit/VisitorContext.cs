using TreeVal.Media;
using TreeVal.Visit.Stage;

namespace TreeVal.Visit;

public class VisitorContext : IVisitorContext
{
    private readonly List<StageVisitationResult> _visitations = new(1);

    public ITapeHead TapeHead { get; }

    public IVisitor Visitor { get; }

    public IEnumerable<StageVisitationResult> StageVisitations => _visitations.AsReadOnly();

    public VisitorContext(ITapeHead tapeHead, IVisitor visitor)
    {
        TapeHead = tapeHead;
        Visitor = visitor;
    }

    public IStageContext CreateStageContext(IVisitationStage stage, IStageContext? stageContext)
        => stageContext != null
            ? new StageContext(stageContext.VisitorContext, stage, stageContext.TapeHead)
            : new StageContext(this, stage, TapeHead);

    public void RecordVisitation(StageVisitationResult result)
        => _visitations.Add(result);
}
