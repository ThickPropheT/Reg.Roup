using System.Linq.Expressions;

namespace Reg.Roup.Expectation
{
    public interface IExpectation<TNode> : IBaseExpectation
        where TNode : Expression
    {
        public delegate Expression Transformer(TNode node);

        IExpectation<TNode> Transform(Transformer transformer);
    }
}