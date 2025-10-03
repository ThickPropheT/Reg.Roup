namespace TreeVal.Visit.Stage;

public interface IVisitationStage
{
    public string CreatedBy { get; }
    public string? CreationSite { get; }

    IStageContext Visit(IStageContext stageContext);
}
