using TreeVal.Media;
using TreeVal.Scaffolding.Stage;
using TreeVal.Visit.Behavior;

namespace TreeVal.Stage.Eval;

public class EvaluateConditionsStageBuilder : VisitationStageBuilder, IEvaluateConditionsStageBuilder
{
    public EvaluateConditionsStageBuilder()
        : base(new IVisitationStageBuilder.Identity<IEvaluateConditionsStageBuilder>())
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

        public void Perform(BehaviorContext behaviorContext)
        {
            throw new NotImplementedException();
            
            try
            {
                // _condition.Evaluate(_node, new DefaultConditionEvaluation());
            }
            catch (Exception ex)
            {
                behaviorContext.RecordResult(new RejectionResult(ex));
            }
        }
    }
}
