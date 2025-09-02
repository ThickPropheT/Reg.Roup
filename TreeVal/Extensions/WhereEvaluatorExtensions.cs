using System.Runtime.CompilerServices;
using TreeVal.Condition;

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

// public static class WithEvaluatorExtensions
// {
//     public static IEvaluatorBuilder<T> With<T>(
//         this IEvaluatorBuilder node,
//         Func<Expression, T> selector
//     )
//     {
//         node.AddCondition(new WhereCondition(predicateExpression, e => predicate(selector(e))));
//         return node;
//     }
//
//     public static IEvaluatorBuilder<T> With<TExpression, T>(
//         this IEvaluatorBuilder<TExpression> node,
//         Func<TExpression, T> selector
//     )
//         where TExpression : Expression
//     {
//         node.AddCondition(new WhereCondition<TExpression>(predicateExpression, e => predicate(selector(e))));
//         return node;
//     }
// }
