using System.Runtime.CompilerServices;
using TreeVal.Condition;
using TreeVal.Media;

namespace TreeVal.Extensions;

public static class WhereEvaluatorExtensions
{
    public static IEvaluatorBuilder Where(
        this IEvaluatorBuilder builder,
        Func<Node, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = ""
    )
    {
        builder.AddCondition(new WhereCondition(predicateExpression, predicate));
        return builder;
    }

    public static IEvaluatorBuilder<T> Where<T>(
        this IEvaluatorBuilder<T> builder,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = ""
    )
    {
        builder.AddCondition(new WhereCondition<T>(predicateExpression, predicate));
        return builder;
    }
}
