using TreeVal.Eval.Condition;

namespace TreeVal.Eval;

public class EvaluateConditionsStageBuilder : VisitationStageBuilder, IEvaluateConditionsStageBuilder
{
    public EvaluateConditionsStageBuilder()
    {
        BeforeLeaving(_ => new RejectIfAnyBehaviorFailed());
    }

    public void AddConditions(Func<IEnumerable<ICondition>> getConditions)
        => AddBehaviors(_ => getConditions().Select(condition => new Evaluate(condition)));
}
