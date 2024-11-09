using System.Collections.Generic;

namespace Reg.Roup.Expression
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class BaseExpectation : IBaseExpectation
    {
        protected readonly IExpectationOptions options;

        private readonly List<IConditionProxy> conditions;
        private IBaseExpectation.Next<Expression>? seek;

        IExpectationOptions IBaseExpectation.Options => options;

        protected BaseExpectation(IBaseExpectation.Condition<Expression> nodeTypeCondition, IExpectationOptions options)
        {
            conditions = [new TestNull(nodeTypeCondition)];
            this.options = options;
        }

        public void AddCondition(IBaseExpectation.Condition<Expression> condition)
            => conditions.Add(new ForgiveNull(condition));

        public void SetNext(IBaseExpectation.Next<Expression> seek)
            => this.seek = seek;

        public TExpectation TransferTo<TExpectation>(TExpectation expectation)
            where TExpectation : IBaseExpectation
        {
            foreach (var proxy in conditions)
            {
                expectation.AddCondition(proxy.Condition);
            }

            if (seek != null)
            {
                expectation.SetNext(seek);
            }

            return expectation;
        }

        public EvaluationResult Evaluate(Expression? node)
        {
            var failed = conditions.FirstOrDefault(c => !c.Evaluate(node));
            var isMatch = failed == null;

            Func<SeekResult>? seekNext = null;
            Func<IBaseExpectation.Transformer<Expression?>?>? findTransformer = null;

            if (!isMatch)
            {
                return EvaluationResult.FailWith(new Exception());
            }

            if (isMatch && seek != null)
            {
                seekNext = () => new SeekResult(seek(node!, options));
            }

            // TODO
            if (isMatch && false)
            {
                findTransformer = null;
            }

            return new EvaluationResult(
                isMatch,
                seekNext,
                findTransformer
            );
        }

        private interface IConditionProxy
        {
            IBaseExpectation.Condition<Expression> Condition { get; }

            bool Evaluate(Expression? node);
        }

        private class TestNull(IBaseExpectation.Condition<Expression> condition) : IConditionProxy
        {
            public IBaseExpectation.Condition<Expression> Condition { get; } = condition;

            public bool Evaluate(Expression? node)
                => node != null && Condition(node);
        }

        private class ForgiveNull(IBaseExpectation.Condition<Expression> condition) : IConditionProxy
        {
            public IBaseExpectation.Condition<Expression> Condition { get; } = condition;

            public bool Evaluate(Expression? node)
                => Condition(node!);
        }
    }
}