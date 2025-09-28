using TreeVal.Visit.Stage;

namespace TreeVal.Visit;

public class Visitor : IVisitor
{
    private readonly IEnumerable<IVisitationStage> _stages;

    public Visitor(IEnumerable<IVisitationStage> stages)
    {
        _stages = stages;
    }

    public void Visit(IVisitorContext visitorContext)
    {
        var stageContext = visitorContext.CreateStageContext();

        foreach (var stage in _stages)
        {
            try
            {
                stageContext = stage.Visit(stageContext);

                visitorContext.RecordVisitation(new StageVisitationResult(stageContext));
            }
            catch (Exception ex)
            {
                visitorContext.RecordVisitation(StageVisitationResult.ForError(stageContext, ex));
            }
        }
    }
}
