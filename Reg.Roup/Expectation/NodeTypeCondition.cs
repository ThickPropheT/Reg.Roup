using System;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public class NodeTypeCondition : ICondition
{
    private readonly string _description;
    private readonly Func<Expression, bool> _predicate;
    
    public NodeTypeCondition(ExpressionType nodeType)
    {
        _description = $"ExpressionType: {nodeType}";
        _predicate = node => node.NodeType == nodeType;
    }
    
    public NodeTypeCondition(Type type, ExpressionType? nodeType = null)
    {
        if (nodeType == null)
        {
            _description = $"Type: {type.FullName}";
            _predicate = node => node.GetType().IsAssignableTo(type);
        }
        else
        {
            _description = $"Type: {type.FullName}, ExpressionType: {nodeType}";
            _predicate = node => node.NodeType == nodeType && node.GetType() == type;
        }
    }
    
    public string Describe(Expression? node)
        => $"Condition.OfType: {{ {_description} }}";

    public bool Evaluate(Expression? node)
        => _predicate(node!);
}
