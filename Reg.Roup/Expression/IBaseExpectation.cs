namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public interface IBaseExpectation : IEvaluationFrameBuilder
    {
        // TODO consider renaming to Filter
        public delegate bool Condition<in TNode>(TNode node);
        public delegate T Selector<in TNode, out T>(TNode node);
        public delegate IEvaluationFrameBuilder Next<in TNode>(TNode node, IExpectNode options);
        public delegate IEvaluationFrameBuilder NextChild<in TNode, in TChild>(TNode node, TChild child, int i, IExpectNode options);
        // TODO consider merging w/ Selector
        public delegate T State<in TNode, out T>(TNode node);
        public delegate Expression Transformer<in TNode>(TNode node);

        void AddCondition(Condition<Expression> condition);
        void SetNext(Next<Expression> seek);

        TExpectation TransferTo<TExpectation>(TExpectation expectation)
            where TExpectation : IBaseExpectation;
    }
}
