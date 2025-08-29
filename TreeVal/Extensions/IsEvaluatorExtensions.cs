using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TreeVal.Condition;

namespace TreeVal.Extensions;

public static class IsEvaluatorExtensions
{
    public static TBuilder Is<TBuilder>(
        this TBuilder node,
        Func<Expression, Type> getLeft,
        Type? right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
        where TBuilder : IEvaluatorConditionBuilder
    {
        node.AddCondition(
            new WhereCondition($"{getLeftExpression} is {right}", e => getLeft(e).IsAssignableTo(right)));
        return node;
    }

    public static TBuilder Is<TBuilder>(
        this TBuilder node,
        Func<Expression, Type> getLeft,
        Func<Type?> getRight,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = "",
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
        where TBuilder : IEvaluatorConditionBuilder
    {
        node.AddCondition(
            new WhereCondition($"{getLeftExpression} is {getRightExpression}", e => getLeft(e).IsAssignableTo(getRight())));
        return node;
    }
}
