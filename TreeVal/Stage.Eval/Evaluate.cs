using TreeVal.Eval;
using TreeVal.Media;
using TreeVal.Visit.Behavior;

namespace TreeVal.Stage.Eval;

public class Evaluate : IBehavior
{
    private readonly ICondition _condition;
    private readonly Node _node;

    public Evaluate(ICondition condition, Node node)
    {
        _condition = condition;
        _node = node;
    }

    public void Perform(BehaviorContext context)
    {
        throw new NotImplementedException();
        // _condition.Evaluate(_node, new DefaultConditionEvaluation());
    }
}
