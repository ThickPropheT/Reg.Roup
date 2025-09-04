using TreeVal.Media;

namespace TreeVal.Eval.Condition;

public interface ICondition
{
    void Evaluate(Node node, IConditionEvaluation evaluation);
}

public interface ICondition<T> : ICondition
{
    void Evaluate(Node<T> node, IConditionEvaluation evaluation);
}
