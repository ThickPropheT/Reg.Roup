namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public static class ExpectNode
    {
        public static IExpectation<TNode> OfType<TNode>(ExpressionType? nodeType = null)
            where TNode : Expression
            => new Expect().NodeType<TNode>(nodeType);

        public static IExpectation<Expression> OfType(ExpressionType nodeType)
            => new Expect().NodeType(nodeType);
    }
}
