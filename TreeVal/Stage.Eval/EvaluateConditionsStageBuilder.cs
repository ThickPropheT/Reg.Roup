using TreeVal.Eval;
using TreeVal.Media;
using TreeVal.Scaffolding.Stage;
using TreeVal.Stage.Eval.Rejection;
using TreeVal.Visit.Behavior;

namespace TreeVal.Stage.Eval;

public class EvaluateConditionsStageBuilder : VisitationStageBuilder, EvaluateConditionsStage.IBuilder
{
    public EvaluateConditionsStageBuilder()
        : base(EvaluateConditionsStage.Key)
    {
        BeforeLeaving((_, _) => new RejectIfAnyBehaviorFailed());
    }

    public void AddConditions(Func<IEnumerable<ICondition>> getConditions)
        => AddBehaviors(n => getConditions().Select(condition => new Evaluate(condition, n)));

    private class Evaluate : IBehavior
    {
        private readonly ICondition _condition;
        private readonly Node _node;

        public Evaluate(ICondition condition, Node node)
        {
            _condition = condition;
            _node = node;
        }

        public void Perform(IBehaviorContext behaviorContext)
        {
            var context = new ConditionContext(behaviorContext, _condition, _node);

            try
            {
                _condition.Evaluate(_node, context);

                behaviorContext.RecordResult(new ConditionEvaluationResult(context));
            }
            catch (Exception ex)
            {
                behaviorContext.RecordResult(ConditionEvaluationResult.ForError(context, ex));
                throw BehaviorVisitationException.ForError(ex, behaviorContext);
            }
        }
    }
}
