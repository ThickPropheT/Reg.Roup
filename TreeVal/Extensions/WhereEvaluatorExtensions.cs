using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TreeVal.Condition;

namespace TreeVal.Extensions;

public static class WhereEvaluatorExtensions
{
    public static IEvaluatorBuilder<TExpression> Where<TExpression>(
        this IEvaluatorBuilder<TExpression> node,
        Func<Expression, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = ""
    )
    {
        node.AddCondition(new WhereCondition(predicateExpression, predicate));
        return node;
    }

    public static IEvaluatorBuilder<TExpression> Where<TExpression>(
        this IEvaluatorBuilder<TExpression> node,
        Func<TExpression, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = ""
    )
        where TExpression : Expression
    {
        node.AddCondition(new WhereCondition<TExpression>(predicateExpression, predicate));
        return node;
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
