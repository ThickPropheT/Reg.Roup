using TreeVal.Media;

namespace TreeVal.Eval.Condition;

public interface ICondition
{
    void Evaluate(Node node, Evaluation evaluation);
}

public interface ICondition<T> : ICondition
{
    void Evaluate(Node<T> node, Evaluation evaluation);
}
