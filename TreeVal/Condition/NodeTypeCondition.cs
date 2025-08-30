using System.Linq.Expressions;

namespace TreeVal.Condition;

public class NodeTypeCondition : ICondition
{
    private readonly Type? _type;
    private readonly ExpressionType? _nodeType;

    private readonly State _state;

    private Action<NodeTypeCondition>? _matchFailed;

    private NodeTypeCondition(ExpressionType nodeType)
    {
        _nodeType = nodeType;
        _state = State.ByNodeType;
    }

    private NodeTypeCondition(Type type, ExpressionType? nodeType = null)
    {
        _type = type;
        _nodeType = nodeType;

        _state = nodeType is not null
            ? State.ByTypeAndNodeType
            : State.ByType;
    }

    public static NodeTypeCondition RejectNonMatching(ExpressionType nodeType)
        => new(nodeType);

    public static NodeTypeCondition RejectNonMatching<TNode>(ExpressionType? nodeType = null)
        => new(typeof(TNode), nodeType);

    public static NodeTypeCondition AssertMatching(ExpressionType nodeType)
        => new(nodeType) { _matchFailed = condition => throw new ConditionFailedException(condition) };

    public static NodeTypeCondition AssertMatching<TNode>(ExpressionType? nodeType = null)
        => new(typeof(TNode), nodeType) { _matchFailed = condition => throw new ConditionFailedException(condition) };

    public void Evaluate(Expression node, Evaluation evaluation)
    {
        var doesMatch = DoesMatch(node);

        if (_matchFailed == null)
        {
            if (!doesMatch)
                evaluation.Reject();

            return;
        }

        if (!doesMatch)
            _matchFailed(this);
    }

    private bool DoesMatch(Expression node)
    {
        switch (_state)
        {
            case State.ByNodeType:
                return node.NodeType == _nodeType;
            case State.ByType:
                return node.GetType().IsAssignableTo(_type);
            case State.ByTypeAndNodeType:
                return node.NodeType == _nodeType
                       && node.GetType().IsAssignableTo(_type);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Describe(IDescription description)
    {
        switch (_state)
        {
            case State.ByNodeType:
                description.EmitNodeTypeCondition((ExpressionType) _nodeType!);
                break;
            case State.ByType:
            case State.ByTypeAndNodeType:
                description.EmitNodeTypeCondition(_type!, _nodeType);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override string ToString()
    {
        switch (_state)
        {
            case State.ByNodeType:
                return $"node.NodeType == ExpressionType.{_nodeType}";
            case State.ByType:
                return $"node is {_type}";
            case State.ByTypeAndNodeType:
                return $"node is {_type} && node.NodeType == ExpressionType.{_nodeType}";
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private enum State
    {
        ByNodeType = 0,
        ByType,
        ByTypeAndNodeType
    }
}
