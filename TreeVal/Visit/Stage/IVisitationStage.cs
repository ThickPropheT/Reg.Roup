namespace TreeVal.Visit.Stage;

public interface IVisitationStage
{
    public string CreatedBy { get; }

    IStageContext Visit(IStageContext stageContext);
}
