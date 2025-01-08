namespace Reg.Roup.Expression;

using System.Linq.Expressions;

public static partial class ExpectationExtensions
{
    public static IStatefulExpectation<T> Using<T>(
        this IBaseExpectation e,
        IBaseExpectation.State<Expression, T> state
    )
        => e.TransferTo(new StatefulExpectation<Expression, T>(state));

    public static IStatefulExpectation<T> Using<TNode, T>(
        this IExpectation<TNode> e,
        IBaseExpectation.State<TNode, T> state
    )
        where TNode : Expression
        => e.TransferTo(new StatefulExpectation<TNode, T>(state));
}
