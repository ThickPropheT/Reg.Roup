namespace Reg.Roup.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class ExpectEach<T> : IExpectationEvaluator
    {
        private readonly IEnumerator<T> enumerator;
        private readonly Func<T, IExpectationEvaluator> body;

        public ExpectEach(IEnumerator<T> enumerator, Func<T, IExpectationEvaluator> body)
        {
            this.enumerator = enumerator;
            this.body = body;
        }

        public EvaluationResult Evaluate(Expression? node)
        {
            if (!enumerator.MoveNext())
            {
                return EvaluationResult.FailWith(new Exception());
            }

            var inner = body(enumerator.Current);

            var result = inner.Evaluate(node);

            //return new EvaluationResult(
            //    result.IsMatch,

            //);
            return result;
        }
    }
}