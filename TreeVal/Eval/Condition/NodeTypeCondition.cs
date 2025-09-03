using TreeVal.Media;

namespace TreeVal.Eval.Condition;

public class NodeTypeCondition : ICondition
{
    private readonly Type _type;

    private Action<NodeTypeCondition, Node>? _matchFailed;

    private NodeTypeCondition(Type type)
    {
        _type = type;
    }

    public static NodeTypeCondition RejectNonMatching<T>()
        => new(typeof(T));

    public static NodeTypeCondition AssertMatching<T>()
        => new(typeof(T)) { _matchFailed = (condition, node) => throw new ConditionFailedException(condition, node) };

    public void Evaluate(Node node, Evaluation evaluation)
    {
        var doesMatch = DoesMatch(node);

        if (_matchFailed == null)
        {
            if (!doesMatch)
                evaluation.Reject(this, node);

            return;
        }

        if (doesMatch)
            return;

        _matchFailed(this, node);
    }

    private bool DoesMatch(Node node)
        => node.Value.GetType().IsAssignableTo(_type);

    public override string ToString()
        => $"node is {_type}";
}
