using TreeVal.Media;

namespace TreeVal.Condition;

public interface ICondition : IDescribable
{
    void Evaluate(Node node, Evaluation evaluation);
}
