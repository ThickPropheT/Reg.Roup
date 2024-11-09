namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public interface IExpectation<TNode> : IBaseExpectation
        where TNode : Expression
    {
        public delegate Expression Transformer(TNode node);

        IExpectation<TNode> Transform(Transformer transformer);
    }
}