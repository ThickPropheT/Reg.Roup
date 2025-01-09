using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public interface IExpectNode
{
    IExpectation<TNode> OfType<TNode>(ExpressionType? nodeType = null)
        where TNode : Expression;

    IExpectation<Expression> OfType(ExpressionType nodeType);

    IEvaluationFrameBuilder OneOf(params IEvaluationFrameBuilder[] options);
    IEvaluationFrameBuilder Each<T>(IEnumerator<T> enumerator, Func<T, IEvaluationFrameBuilder> body);
}