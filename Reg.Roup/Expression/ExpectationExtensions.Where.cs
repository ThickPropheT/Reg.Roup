namespace Reg.Roup.Expression;

using System.Linq.Expressions;

public static partial class ExpectationExtensions
{
    public static IBaseExpectation Where(
        this IBaseExpectation e,
        IBaseExpectation.Condition<Expression> condition
    )
    {
        e.AddCondition(condition);
        return e;
    }

    public static IExpectation<TNode> Where<TNode>(
        this IBaseExpectation e,
        IBaseExpectation.Condition<TNode> condition
    )
        where TNode : Expression
    {
        var typal = e.TransferTo(new NodeTypeExpectation<TNode>(e.Options));
        typal.AddCondition(n => condition((TNode) n));
        return typal;
    }

    public static IExpectation<TNode> Where<TNode>(
        this IExpectation<TNode> e,
        IBaseExpectation.Condition<TNode> condition
    )
        where TNode : Expression
    {
        e.AddCondition(n => condition((TNode) n));
        return e;
    }
}
