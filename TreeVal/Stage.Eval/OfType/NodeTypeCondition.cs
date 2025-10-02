using TreeVal.Media;

namespace TreeVal.Stage.Eval.OfType;

public class NodeTypeCondition : ICondition
{
    private readonly Type _type;

    private Action<IConditionContext>? _matchFailed;

    private NodeTypeCondition(Type type)
    {
        _type = type;
    }

    public static NodeTypeCondition RejectNonMatching<T>()
        => new(typeof(T));

    public static NodeTypeCondition AssertMatching<T>()
        => new(typeof(T)) { _matchFailed = evaluation => throw ConditionFailedException.ExpectedNode<T>(evaluation) };

    public void Evaluate(Node node, IConditionContext context)
    {
        var doesMatch = DoesMatch(node);

        if (_matchFailed == null)
        {
            if (!doesMatch)
                context.Reject();

            return;
        }

        if (doesMatch)
            return;

        _matchFailed(context);
    }

    private bool DoesMatch(Node node)
        => node.Value.GetType().IsAssignableTo(_type);

    public override string ToString()
        => $"node is {_type}";
}
