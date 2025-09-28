namespace TreeVal.Scaffolding.Stage;

public interface IStageDirector
{
    IVisitationStageBuilder.Identity ValidateKey(IVisitationStageBuilder.Identity key);

    TStage Create<TStage>()
        where TStage : IVisitationStageBuilder;

    IEnumerable<IVisitationStageBuilder> Arrange(IEnumerable<IVisitationStageBuilder> stages);
}
