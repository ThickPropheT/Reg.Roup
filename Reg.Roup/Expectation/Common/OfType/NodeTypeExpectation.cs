using System;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation.Common.OfType;

// TODO reconcile the naming convention for this and others under Common namespace
public class NodeTypeExpectation<TNode> : BaseExpectation, IExpectation<TNode>
    where TNode : Expression
{
    public ExpressionType? NodeType { get; }

    public NodeTypeExpectation(ExpressionType? nodeType = null)
        : base(n => n is TNode && nodeType == null || n.NodeType == nodeType)
    {
        NodeType = nodeType;
    }

    public IExpectation<TNode> Transform(IExpectation<TNode>.Transformer transformer)
    {
        throw new NotImplementedException();
    }
}
