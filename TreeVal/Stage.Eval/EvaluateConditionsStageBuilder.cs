using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Eval;

public class EvaluateConditionsStageBuilder : VisitationStageBuilder, IEvaluateConditionsStageBuilder
{
    public EvaluateConditionsStageBuilder()
        : base(new IVisitationStageBuilder.Identity<IEvaluateConditionsStageBuilder>())
    {
        BeforeLeaving((_, c) => new RejectIfAnyBehaviorFailed());
    }

    public void AddConditions(Func<IEnumerable<ICondition>> getConditions)
        => AddBehaviors(n => getConditions().Select(condition => new Evaluate(condition, n)));
}
