namespace Reg.Roup.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class Expect : IExpectationOptions
    {
        public IExpectation<TNode> NodeType<TNode>()
            where TNode : Expression
            => new Expectation<TNode>(this);

        public IExpectation NodeType(ExpressionType nodeType)
            => new Expectation(nodeType, this);

        public IExpectationEvaluator OneOf(params IExpectationEvaluator[] options)
            => new ExpectOneOf(options);

        public IExpectationEvaluator Each<T>(IEnumerator<T> enumerator, Func<T, IExpectationEvaluator> body)
            => new ExpectEach<T>(enumerator, body);
    }
}