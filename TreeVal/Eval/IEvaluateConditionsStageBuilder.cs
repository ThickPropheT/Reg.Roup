using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Eval;

public interface IEvaluateConditionsStageBuilder : IVisitationStageBuilder
{
    void AddConditions(Func<Node, IEnumerable<ICondition>> getConditions);
}
