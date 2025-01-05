using Reg.Roup.Expression;

namespace Reg.Roup.Tests.Resources
{
    using System.Linq.Expressions;

    // TODO consider making this class a real boy
    public class ExpectationProxy : IBaseExpectation
    {
        private readonly IExpectationOptions options;
        private readonly IBaseExpectation.Next<Expression> seek;

        public IExpectationOptions Options => options;

        public ExpectationProxy(IExpectationOptions options, IBaseExpectation.Next<Expression> seek)
        {
            this.options = options;
            this.seek = seek;
        }

        public void AddCondition(IBaseExpectation.Condition<Expression> condition)
            => throw new NotSupportedException();

        public void SetNext(IBaseExpectation.Next<Expression> seek)
            => throw new NotSupportedException();

        public TExpectation TransferTo<TExpectation>(TExpectation expectation) where TExpectation : IBaseExpectation
        {
            expectation.SetNext(seek);
            return expectation;
        }

        public IEvaluationFrame BuildFrame(Expression? node)
            => seek!.Invoke(node!, options).BuildFrame(node);
    }
}
