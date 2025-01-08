using System.Collections.Generic;
using System;

namespace Reg.Roup.Expression;

using System.Linq.Expressions;

public interface IExpectationOptions
{
    IExpectation<TNode> NodeType<TNode>(ExpressionType? nodeType = null)
        where TNode : Expression;

    IExpectation<Expression> NodeType(ExpressionType nodeType);

    IEvaluationFrameBuilder OneOf(params IEvaluationFrameBuilder[] options);
    IEvaluationFrameBuilder Each<T>(IEnumerator<T> enumerator, Func<T, IEvaluationFrameBuilder> body);
}
