namespace TreeVal.Scaffolding.Stage;

public interface IStageDirector
{
    TKey ValidateKey<TKey>(TKey key)
        where TKey : IVisitationStageBuilder.Identity;

    TStage Create<TStage>(IVisitationStageBuilder.Identity<TStage> key)
        where TStage : IVisitationStageBuilder;

    IEnumerable<IVisitationStageBuilder> Arrange(IEnumerable<IVisitationStageBuilder> stages);
}
