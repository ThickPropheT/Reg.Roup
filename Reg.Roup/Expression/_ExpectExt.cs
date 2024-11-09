namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public class _ExpectExt
    {
        public static IExpectation<TNode> NodeType<TNode>()
            where TNode : Expression
            => new Expect().NodeType<TNode>();

        public static IExpectation NodeType(ExpressionType nodeType)
            => new Expect().NodeType(nodeType);
    }
}