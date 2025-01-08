using System.Collections.Generic;
using System;

namespace Reg.Roup.Expression;

using System.Linq.Expressions;

public interface IExpectNode
{
    IExpectation<TNode> OfType<TNode>(ExpressionType? nodeType = null)
        where TNode : Expression;

    IExpectation<Expression> OfType(ExpressionType nodeType);

    IEvaluationFrameBuilder OneOf(params IEvaluationFrameBuilder[] options);
    IEvaluationFrameBuilder Each<T>(IEnumerator<T> enumerator, Func<T, IEvaluationFrameBuilder> body);
}
