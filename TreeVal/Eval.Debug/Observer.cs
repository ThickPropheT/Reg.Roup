using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Eval.Debug;

public class Observer : ICondition
{
    private readonly Action<object, Evaluation> _observe;

    public Observer(Action<object, Evaluation> observe)
    {
        _observe = observe;
    }

    public void Evaluate(Node node, Evaluation evaluation)
        => _observe(node.Value, evaluation);
}

public class Observer<T> : ICondition<T>
{
    private readonly Action<T, Evaluation> _observe;

    public Observer(Action<T, Evaluation> observe)
    {
        _observe = observe;
    }

    public void Evaluate(Node node, Evaluation evaluation)
    {
        if (node.Value is not T t)
        {
            throw new ConditionFailedException(this, node);
        }
        
        _observe(t, evaluation);
    }

    public void Evaluate(Node<T> node, Evaluation evaluation)
        => _observe(node.Value, evaluation);
}
