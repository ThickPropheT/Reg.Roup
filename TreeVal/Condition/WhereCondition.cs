using TreeVal.Media;

namespace TreeVal.Condition;

public class WhereCondition : ICondition
{
    private readonly string _message;
    private readonly Func<Node, bool> _predicate;

    public WhereCondition(string message, Func<Node, bool> predicate)
    {
        _message = message;
        _predicate = predicate;
    }

    public void Describe(IDescription description)
        => description.EmitWhereCondition(_message);

    public void Evaluate(Node node, Evaluation evaluation)
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

    public void Describe(IDescription description)
        => description.EmitWhereCondition(_message);

    public void Evaluate(Node node, Evaluation evaluation)
    {
        if (node.Value is not T t)
        {
            throw new ConditionFailedException(this);
        }

        if (_predicate(t))
            return;

        evaluation.Reject();
    }

    public override string ToString() => _message;
}
