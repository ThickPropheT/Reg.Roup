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

    public void Evaluate(Node node, IConditionContext context)
    {
        if (_predicate(node))
            return;

        context.Reject();
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

    public void Evaluate(Node node, IConditionContext context)
    {
        if (node.Value is not T t)
        {
            throw ConditionFailedException.ExpectedNode<T>(context);
        }

        if (_predicate(t))
            return;

        context.Reject();
    }

    public override string ToString() => _message;
}
