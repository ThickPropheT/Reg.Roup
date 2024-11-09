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

        IExpectationEvaluator OneOf(params IExpectationEvaluator[] options);
        IExpectationEvaluator Each<T>(IEnumerator<T> enumerator, Func<T, IExpectationEvaluator> body);
    }
}