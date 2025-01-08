namespace Reg.Roup.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    // TODO figure out this naming & that of _ExpectExt
    public class ExpectNode : IExpectNode
    {
        public static IExpectation<TNode> OfType<TNode>(ExpressionType? nodeType = null)
            where TNode : Expression
            => new ExpectNode().NodeType<TNode>(nodeType);

        public static IExpectation<Expression> OfType(ExpressionType nodeType)
            => new ExpectNode().NodeType(nodeType);
        
        public IExpectation<TNode> NodeType<TNode>(ExpressionType? nodeType = null)
            where TNode : Expression
            => new NodeTypeExpectation<TNode>(nodeType);

        public IExpectation<Expression> NodeType(ExpressionType nodeType)
            => new NodeTypeExpectation<Expression>(nodeType);

        public IEvaluationFrameBuilder OneOf(params IEvaluationFrameBuilder[] options)
            => new ExpectOneOf(options);

        public IEvaluationFrameBuilder Each<T>(IEnumerator<T> enumerator, Func<T, IEvaluationFrameBuilder> body)
            => new ExpectEach<T>(enumerator, body);
    }
}
