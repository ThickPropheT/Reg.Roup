using TreeVal.Diagnosticts;
using TreeVal.Media;

namespace TreeVal.Eval.Condition;

public interface ICondition : IDescribable
{
    void Evaluate(Node node, Evaluation evaluation);
}
