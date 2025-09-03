using System.Runtime.CompilerServices;
using TreeVal.Eval.Condition;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

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
