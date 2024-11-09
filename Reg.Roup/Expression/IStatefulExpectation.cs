using System;

namespace Reg.Roup.Expression
{
    public interface IStatefulExpectation<T> : IBaseExpectation
    {
        IStatefulExpectation<T> Where(Condition<T> condition);
        IStatefulExpectation<T> With(Next<T> next);
    }
}