namespace TreeVal.Visit.Stage;

public interface IVisitationStage
{
    IStageContext Visit(IStageContext stageContext);
}
