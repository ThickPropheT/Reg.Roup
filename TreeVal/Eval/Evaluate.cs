using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Eval;

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
        _condition.Evaluate(_node, new DefaultConditionEvaluation());
    }
}
