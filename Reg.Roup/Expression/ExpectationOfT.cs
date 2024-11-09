namespace Reg.Roup.Expression
{
    using System;
    using System.Linq.Expressions;

    public class Expectation<TNode> : BaseExpectation, IExpectation<TNode>
        where TNode : Expression
    {
        public Expectation(IExpectationOptions options)
            : base(n => n is TNode, options)
        {
        }

        public IExpectation<TNode> Transform(IExpectation<TNode>.Transformer transformer)
        {
            throw new NotImplementedException();
        }
    }
}