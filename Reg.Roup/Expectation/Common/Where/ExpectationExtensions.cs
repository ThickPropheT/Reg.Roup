using System.Linq.Expressions;
using Reg.Roup.Expectation.Common.OfType;

namespace Reg.Roup.Expectation.Common.Where;

public static class ExpectationExtensions
{
    public static IBaseExpectation Where(
        this IBaseExpectation e, IBaseExpectation.Condition<Expression> condition
    )
    {
        e.AddCondition(condition);
        return e;
    }

    public static IExpectation<TNode> Where<TNode>(
        this IBaseExpectation e, IBaseExpectation.Condition<TNode> condition
    )
        where TNode : Expression
    {
        var typal = e.TransferTo(new NodeTypeExpectation<TNode>());
        typal.AddCondition(n => condition((TNode) n));
        return typal;
    }

    public static IExpectation<TNode> Where<TNode>(
        this IExpectation<TNode> e, IBaseExpectation.Condition<TNode> condition
    )
        where TNode : Expression
    {
        e.AddCondition(n => condition((TNode) n));
        return e;
    }
}
