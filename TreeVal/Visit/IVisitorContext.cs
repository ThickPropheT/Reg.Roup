using TreeVal.Visit.Stage;

namespace TreeVal.Visit;

public interface IVisitorContext
{
    IEnumerable<StageVisitationResult> StageVisitations { get; }
    
    IStageContext CreateStageContext();

    void RecordVisitation(StageVisitationResult result);
}
