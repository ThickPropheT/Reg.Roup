using System;

namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public interface IBaseExpectation : IExpectationEvaluator
    {
        public delegate bool Condition<TNode>(TNode node);
        public delegate IExpectationEvaluator Next<TNode>(TNode node, IExpectationOptions options);
        public delegate T State<TNode, T>(TNode node);
        public delegate Expression Transformer<TNode>(TNode node);

        IExpectationOptions Options { get; }

        void AddCondition(Condition<Expression> condition);
        void SetNext(Next<Expression> seek);

        TExpectation TransferTo<TExpectation>(TExpectation expectation)
            where TExpectation : IBaseExpectation;
    }
}