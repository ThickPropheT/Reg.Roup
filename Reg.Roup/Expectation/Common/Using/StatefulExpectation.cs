using System;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation.Common.Using;

public class StatefulExpectation<TNode, T> : BaseExpectation, IStatefulExpectation<T>
    where TNode : Expression
{
    private readonly IBaseExpectation.State<TNode, T> _state;

    public StatefulExpectation(IBaseExpectation.State<TNode, T> state)
        : base(n => true)
    {
        _state = state;
    }

    public IStatefulExpectation<T> Where(IBaseExpectation.Condition<T> condition)
    {
        throw new NotImplementedException();
        //AddCondition(n => condition(state()));
        //return this;
    }

    public IStatefulExpectation<T> WithChildren(IBaseExpectation.Next<T> nested)
    {
        SetNext((n, options) => nested(_state((TNode) n), options));
        return this;
    }
}
