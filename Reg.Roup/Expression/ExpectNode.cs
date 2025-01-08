namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public static class ExpectNode
    {
        public static IExpectation<TNode> OfType<TNode>()
            where TNode : Expression
            => new Expect().NodeType<TNode>();

        public static IExpectation OfType(ExpressionType nodeType)
            => new Expect().NodeType(nodeType);
    }
}
