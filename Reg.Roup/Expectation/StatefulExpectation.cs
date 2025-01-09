using System;
using System.Linq.Expressions;

namespace Reg.Roup.Expectation;

public class StatefulExpectation<TNode, T> : BaseExpectation, IStatefulExpectation<T>
    where TNode : Expression
{
    private readonly IBaseExpectation.State<TNode, T> state;

    public StatefulExpectation(IBaseExpectation.State<TNode, T> state)
        : base(n => true)
    {
        this.state = state;
    }

    public IStatefulExpectation<T> Where(IBaseExpectation.Condition<T> condition)
    {
        throw new NotImplementedException();
        //AddCondition(n => condition(state()));
        //return this;
    }

    public IStatefulExpectation<T> WithChildren(IBaseExpectation.Next<T> nested)
    {
        SetNext((n, options) => nested(state((TNode) n), options));
        return this;
    }
}