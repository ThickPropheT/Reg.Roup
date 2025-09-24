using TreeVal.Eval.Condition;

namespace TreeVal.Eval;

public interface IEvaluateConditionsStageBuilder : IVisitationStageBuilder
{
    void AddConditions(Func<IEnumerable<ICondition>> getConditions);
}
