using TreeVal.Media;

namespace TreeVal.Stage.Eval;

public interface ICondition
{
    void Evaluate(Node node, IConditionContext context);
}

public interface ICondition<T> : ICondition
{
    void Evaluate(Node<T> node, IConditionContext context);
}
