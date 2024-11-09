using System;

namespace Reg.Roup.Expression
{
    using System.Linq.Expressions;

    public class StatefulExpectation<TNode, T> : BaseExpectation, IStatefulExpectation<T>
        where TNode : Expression
    {
        private readonly IBaseExpectation.State<TNode, T> state;

        public StatefulExpectation(IBaseExpectation.State<TNode, T> state, IExpectationOptions options)
            : base(n => true, options)
        {
            this.state = state;
        }

        public IStatefulExpectation<T> Where(IBaseExpectation.Condition<T> condition)
        {
            throw new NotImplementedException();
            //AddCondition(n => condition(state()));
            //return this;
        }

        public IStatefulExpectation<T> With(IBaseExpectation.Next<T> nested)
        {
            SetNext((n, options) => nested(state((TNode)n), options));
            return this;
        }
    }
}