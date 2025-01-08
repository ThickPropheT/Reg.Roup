namespace Reg.Roup.Expression;

using System.Linq.Expressions;

public static partial class ExpectationExtensions
{
    public static IBaseExpectation WithChildren(
        this IBaseExpectation e,
        IBaseExpectation.Next<Expression> seek
    )
    {
        e.SetNext(seek);
        return e;
    }

    public static IExpectation<TNode> WithChildren<TNode>(
        this IBaseExpectation e,
        IBaseExpectation.Next<TNode> seek
    )
        where TNode : Expression
    {
        var typal = e.TransferTo(new Expectation<TNode>(e.Options));
        typal.SetNext((n, options) => seek((TNode) n, options));
        return typal;
    }

    public static IExpectation<TNode> WithChildren<TNode>(
        this IExpectation<TNode> e,
        IBaseExpectation.Next<TNode> seek
    )
        where TNode : Expression
    {
        e.SetNext((n, options) => seek((TNode) n, options));
        return e;
    }
}
