using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TreeVal.Condition;

namespace TreeVal;

public static class WhereVisitorNodeExtensions
{
    public static IEvaluatorBuilder Where(
        this IEvaluatorBuilder node,
        Func<Expression?, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = "")
    {
        node.AddCondition(new WhereCondition(predicateExpression, predicate));
        return node;
    }

    public static IEvaluatorBuilder<T> Where<T>(
        this IEvaluatorBuilder<T> node,
        Func<T, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = "")
    {
        node.AddCondition(new WhereCondition(predicateExpression, n =>
        {
            if (n is not T t)
            {
                throw new InvalidOperationException();
            }

            return predicate(t);
        }));

        return node;
    }
}
