namespace Reg.Roup.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    // TODO figure out this naming & that of _ExpectExt
    public class Expect : IExpectationOptions
    {
        public IExpectation<TNode> NodeType<TNode>()
            where TNode : Expression
            => new Expectation<TNode>(this);

        public IExpectation NodeType(ExpressionType nodeType)
            => new Expectation(nodeType, this);

        public IEvaluationFrameBuilder OneOf(params IEvaluationFrameBuilder[] options)
            => new ExpectOneOf(options);

        public IEvaluationFrameBuilder Each<T>(IEnumerator<T> enumerator, Func<T, IEvaluationFrameBuilder> body)
            => new ExpectEach<T>(enumerator, body);
    }
}