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
            => ((IExpectNode)new ExpectNode()).OfType<TNode>(nodeType);

        public static IExpectation<Expression> OfType(ExpressionType nodeType)
            => ((IExpectNode)new ExpectNode()).OfType(nodeType);
        
        IExpectation<TNode> IExpectNode.OfType<TNode>(ExpressionType? nodeType)
            => new NodeTypeExpectation<TNode>(nodeType);

        IExpectation<Expression> IExpectNode.OfType(ExpressionType nodeType)
            => new NodeTypeExpectation<Expression>(nodeType);

        public IEvaluationFrameBuilder OneOf(params IEvaluationFrameBuilder[] options)
            => new ExpectOneOf(options);

        public IEvaluationFrameBuilder Each<T>(IEnumerator<T> enumerator, Func<T, IEvaluationFrameBuilder> body)
            => new ExpectEach<T>(enumerator, body);
    }
}
