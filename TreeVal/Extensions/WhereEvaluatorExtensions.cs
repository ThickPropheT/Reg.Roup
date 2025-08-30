using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TreeVal.Condition;

namespace TreeVal.Extensions;

public static class WhereEvaluatorExtensions
{
    public static TBuilder Where<TBuilder>(
        this TBuilder node,
        Func<Expression, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = ""
    )
        where TBuilder : IEvaluatorConditionBuilder
    {
        node.AddCondition(new WhereCondition(predicateExpression, predicate));
        return node;
    }

    public static IEvaluatorBuilder<T> Where<T>(
        this IEvaluatorBuilder<T> node,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = ""
    )
    {
        node.AddCondition(new WhereCondition(predicateExpression, e =>
        {
            if (e is not T t)
            {
                Debug.Assert(false,
                    "Would this be better off handled at the NodeTypeConditionLevel? It can already do that...");
                throw UnmetPreconditionException.WrongExpressionType<T>(e);
            }

            return predicate(t);
        }));

        return node;
    }
}
