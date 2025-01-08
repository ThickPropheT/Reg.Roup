namespace Reg.Roup.Expression
{
    using System;
    using System.Linq.Expressions;

    public class NodeTypeExpectation<TNode> : BaseExpectation, IExpectation<TNode>
        where TNode : Expression
    {
        public ExpressionType? NodeType { get; }
        
        public NodeTypeExpectation(IExpectationOptions options, ExpressionType? nodeType = null)
            : base(n => n is TNode && nodeType == null || n.NodeType == nodeType, options)
        {
            NodeType = nodeType;
        }

        public IExpectation<TNode> Transform(IExpectation<TNode>.Transformer transformer)
        {
            throw new NotImplementedException();
        }
    }
}
