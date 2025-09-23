using TreeVal.Eval;

namespace TreeVal.Scaffolding;

public interface IStageDirector
{
    IVisitorBuilder.Key<TStage> ValidateKey<TStage>(IVisitorBuilder.Key<TStage> key);
    
    TStage Create<TStage>()
        where TStage : IVisitationStageBuilder;

    IEnumerable<IVisitationStageBuilder> Arrange(IEnumerable<IVisitationStageBuilder> stages);
}
