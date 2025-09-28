using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Eval;

public interface IEvaluateConditionsStageBuilder : IVisitationStageBuilder
{
    void AddConditions(Func<IEnumerable<ICondition>> getConditions);
}
