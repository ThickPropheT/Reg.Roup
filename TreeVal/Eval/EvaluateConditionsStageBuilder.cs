using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Eval;

public class EvaluateConditionsStageBuilder : VisitationStageBuilder, IEvaluateConditionsStageBuilder
{
    public EvaluateConditionsStageBuilder()
    {
        BeforeLeaving(_ => new RejectIfAnyBehaviorFailed());
    }

    public void AddConditions(Func<Node, IEnumerable<ICondition>> getConditions)
        => AddBehaviors(n => getConditions(n).Select(condition => new Evaluate(condition)));
}
