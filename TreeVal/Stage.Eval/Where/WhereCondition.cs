using TreeVal.Media;

namespace TreeVal.Stage.Eval.Where;

public class WhereCondition : ICondition
{
    private readonly string _message;
    private readonly Func<Node, bool> _predicate;

    public WhereCondition(string message, Func<Node, bool> predicate)
    {
        _message = message;
        _predicate = predicate;
    }

    public void Evaluate(Node node, IConditionEvaluation evaluation)
    {
        if (_predicate(node))
            return;

        evaluation.Reject();
    }

    public override string ToString() => _message;
}

public class WhereCondition<T> : ICondition
{
    private readonly string _message;
    private readonly Func<T, bool> _predicate;

    public WhereCondition(string message, Func<T, bool> predicate)
    {
        _message = message;
        _predicate = predicate;
    }

    public void Evaluate(Node node, IConditionEvaluation evaluation)
    {
        if (node.Value is not T t)
        {
            throw ConditionFailedException.ExpectedNode<T>(evaluation);
        }

        if (_predicate(t))
            return;

        evaluation.Reject();
    }

    public override string ToString() => _message;
}
