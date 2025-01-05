using System.Collections.Generic;
using System;

namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public interface IExpectationOptions
    {
        IExpectation<TNode> NodeType<TNode>()
            where TNode : Expression;

        IExpectation NodeType(ExpressionType nodeType);

        IEvaluationFrameBuilder OneOf(params IEvaluationFrameBuilder[] options);
        IEvaluationFrameBuilder Each<T>(IEnumerator<T> enumerator, Func<T, IEvaluationFrameBuilder> body);
    }
}