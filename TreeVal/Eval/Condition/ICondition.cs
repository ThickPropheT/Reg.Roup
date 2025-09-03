using TreeVal.Media;

namespace TreeVal.Eval.Condition;

public interface ICondition
{
    void Evaluate(Node node, Evaluation evaluation);
}
