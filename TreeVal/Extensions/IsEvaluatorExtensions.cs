using System.Runtime.CompilerServices;
using TreeVal.Condition;

namespace TreeVal.Extensions;

public static class IsEvaluatorExtensions
{
    public static IEvaluatorBuilder<T> Is<T>(
        this IEvaluatorBuilder<T> node,
        Func<T, Type> getLeft,
        Type? right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
    {
        node.AddCondition(
            new WhereCondition<T>($"{getLeftExpression} is {right}", e => getLeft(e).IsAssignableTo(right)));
        return node;
    }

    public static IEvaluatorBuilder<T> Is<T>(
        this IEvaluatorBuilder<T> node,
        Func<T, Type> getLeft,
        Func<Type?> getRight,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = "",
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        node.AddCondition(
            new WhereCondition<T>(
                $"{getLeftExpression} is {getRightExpression}", e => getLeft(e).IsAssignableTo(getRight())));
        return node;
    }
}
