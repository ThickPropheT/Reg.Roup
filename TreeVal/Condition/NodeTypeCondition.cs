namespace TreeVal.Condition;

public class NodeTypeCondition : ICondition
{
    private readonly Type _type;

    private Action<NodeTypeCondition>? _matchFailed;

    private NodeTypeCondition(Type type)
    {
        _type = type;
    }

    public static NodeTypeCondition RejectNonMatching<T>()
        => new(typeof(T));

    public static NodeTypeCondition AssertMatching<T>()
        => new(typeof(T)) { _matchFailed = condition => throw new ConditionFailedException(condition) };

    public void Evaluate(Node node, Evaluation evaluation)
    {
        var doesMatch = DoesMatch(node);

        if (_matchFailed == null)
        {
            if (!doesMatch)
                evaluation.Reject();

            return;
        }

        if (doesMatch)
            return;

        _matchFailed(this);
    }

    private bool DoesMatch(Node node)
        => node.Value.GetType().IsAssignableTo(_type);

    public virtual void Describe(IDescription description)
        => description.EmitNodeTypeCondition(_type);

    public override string ToString()
        => $"node is {_type}";
}
