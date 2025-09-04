using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Eval;

public class Observer : ICondition
{
    private readonly Action<object, IConditionEvaluation> _observe;

    public Observer(Action<object, IConditionEvaluation> observe)
    {
        _observe = observe;
    }

    public void Evaluate(Node node, IConditionEvaluation evaluation)
        => _observe(node.Value, evaluation);
}

public class Observer<T> : ICondition<T>
{
    private readonly Action<T, IConditionEvaluation> _observe;

    public Observer(Action<T, IConditionEvaluation> observe)
    {
        _observe = observe;
    }

    public void Evaluate(Node node, IConditionEvaluation evaluation)
    {
        if (node.Value is not T t)
        {
            throw ConditionFailedException.ExpectedNode<T>(evaluation);
        }
        
        _observe(t, evaluation);
    }

    public void Evaluate(Node<T> node, IConditionEvaluation evaluation)
        => _observe(node.Value, evaluation);
}
