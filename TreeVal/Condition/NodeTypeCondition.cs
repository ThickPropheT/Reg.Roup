using System.Linq.Expressions;

namespace TreeVal.Condition;

public class NodeTypeCondition : ICondition
{
    private readonly string _description;
    private readonly Func<Expression, bool> _predicate;

    private Exception? MismatchException { get; init; }

    private NodeTypeCondition(ExpressionType nodeType)
    {
        _description = $"ExpressionType: {nodeType}";
        _predicate = node => node.NodeType == nodeType;
    }

    private NodeTypeCondition(Type type, ExpressionType? nodeType = null)
    {
        if (nodeType == null)
        {
            _description = $"Type: {type.FullName}";
            _predicate = node => node.GetType().IsAssignableTo(type);
        }
        else
        {
            _description = $"Type: {type.FullName}, ExpressionType: {nodeType}";
            _predicate = node => node.NodeType == nodeType && node.GetType().IsAssignableTo(type);
        }
    }

    public static NodeTypeCondition RejectNonMatching(ExpressionType nodeType)
        => new(nodeType);

    public static NodeTypeCondition RejectNonMatching<TNode>(ExpressionType? nodeType = null)
        => new(typeof(TNode), nodeType);

    public static NodeTypeCondition AssertMatching(ExpressionType nodeType)
        => new(nodeType) {MismatchException = new TreeRejectedException()};

    public static NodeTypeCondition AssertMatching<TNode>(ExpressionType? nodeType = null)
        => new(typeof(TNode), nodeType) {MismatchException = new TreeRejectedException()};

    public string Describe(Expression? node)
        => $"Condition.OfType: {{ {_description} }}";

    public bool Evaluate(Expression node)
    {
        var doesMatch = _predicate(node);

        if (MismatchException == null)
        {
            return doesMatch;
        }

        if (!doesMatch)
        {
            throw MismatchException;
        }

        return true;
    }
}
