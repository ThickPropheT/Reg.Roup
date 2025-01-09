using System.Linq.Expressions;
using Reg.Roup.Expectation.Common.OfType;

namespace Reg.Roup.Expectation.Common.WithChildren;

public static class ExpectationExtensions
{
    public static IBaseExpectation WithChildren(
        this IBaseExpectation e, IBaseExpectation.Next<Expression> seek
    )
    {
        e.SetNext(seek);
        return e;
    }

    public static IExpectation<TNode> WithChildren<TNode>(
        this IBaseExpectation e, IBaseExpectation.Next<TNode> seek
    )
        where TNode : Expression
    {
        var typal = e.TransferTo(new NodeTypeExpectation<TNode>());
        typal.SetNext((n, options) => seek((TNode) n, options));
        return typal;
    }

    public static IExpectation<TNode> WithChildren<TNode>(
        this IExpectation<TNode> e, IBaseExpectation.Next<TNode> seek
    )
        where TNode : Expression
    {
        e.SetNext((n, options) => seek((TNode) n, options));
        return e;
    }
}
