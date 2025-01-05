namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public static class ExpectationExtensions
    {
        public static IBaseExpectation Where(this IBaseExpectation e, IBaseExpectation.Condition<Expression> condition)
        {
            e.AddCondition(condition);
            return e;
        }

        public static IExpectation<TNode> Where<TNode>(this IBaseExpectation e, IBaseExpectation.Condition<TNode> condition)
            where TNode : Expression
        {
            var typal = e.TransferTo(new Expectation<TNode>(e.Options));
            typal.AddCondition(n => condition((TNode)n));
            return typal;
        }

        public static IExpectation<TNode> Where<TNode>(this IExpectation<TNode> e, IBaseExpectation.Condition<TNode> condition)
            where TNode : Expression
        {
            e.AddCondition(n => condition((TNode)n));
            return e;
        }

        public static IBaseExpectation WithChildren(this IBaseExpectation e, IBaseExpectation.Next<Expression> seek)
        {
            e.SetNext(seek);
            return e;
        }

        public static IExpectation<TNode> WithChildren<TNode>(this IBaseExpectation e, IBaseExpectation.Next<TNode> seek)
            where TNode : Expression
        {
            var typal = e.TransferTo(new Expectation<TNode>(e.Options));
            typal.SetNext((n, options) => seek((TNode)n, options));
            return typal;
        }

        public static IExpectation<TNode> WithChildren<TNode>(this IExpectation<TNode> e, IBaseExpectation.Next<TNode> seek)
            where TNode : Expression
        {
            e.SetNext((n, options) => seek((TNode)n, options));
            return e;
        }

        public static IStatefulExpectation<T> Using<T>(this IBaseExpectation e, IBaseExpectation.State<Expression, T> state)
            => e.TransferTo(new StatefulExpectation<Expression, T>(state, e.Options));

        public static IStatefulExpectation<T> Using<TNode, T>(this IExpectation<TNode> e, IBaseExpectation.State<TNode, T> state)
            where TNode : Expression
            => e.TransferTo(new StatefulExpectation<TNode, T>(state, e.Options));
    }
}