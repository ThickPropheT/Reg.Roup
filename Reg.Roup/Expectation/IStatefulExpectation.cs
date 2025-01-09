namespace Reg.Roup.Expectation;

public interface IStatefulExpectation<out T> : IBaseExpectation
{
    IStatefulExpectation<T> Where(Condition<T> condition);
    IStatefulExpectation<T> WithChildren(Next<T> next);
}
