using System.Runtime.CompilerServices;
using TreeVal.Eval.Condition;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public static class IsEvaluatorExtensions
{
    public static IVisitorBuilder<T> Is<T>(
        this IVisitorBuilder<T> builder,
        Func<T, Type> getLeft,
        Type? right,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = ""
    )
    {
        builder.AddCondition(
            new WhereCondition<T>($"{getLeftExpression} is {right}", e => getLeft(e).IsAssignableTo(right)));
        return builder;
    }

    public static IVisitorBuilder<T> Is<T>(
        this IVisitorBuilder<T> builder,
        Func<T, Type> getLeft,
        Func<Type?> getRight,
        [CallerArgumentExpression(nameof(getLeft))]
        string getLeftExpression = "",
        [CallerArgumentExpression(nameof(getRight))]
        string getRightExpression = ""
    )
    {
        builder.AddCondition(
            new WhereCondition<T>(
                $"{getLeftExpression} is {getRightExpression}", e => getLeft(e).IsAssignableTo(getRight())));
        return builder;
    }
}
