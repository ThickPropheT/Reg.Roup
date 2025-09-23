using System.Runtime.CompilerServices;
using TreeVal.Eval.Condition;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public static class WhereEvaluatorExtensions
{
    public static IVisitorBuilder Where(
        this IVisitorBuilder builder,
        Func<Node, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = ""
    )
    {
        builder.AddCondition(new WhereCondition(predicateExpression, predicate));
        return builder;
    }

    public static IVisitorBuilder<TNode> Where<TNode>(
        this IVisitorBuilder<TNode> builder,
        Func<TNode, bool> predicate,
        [CallerArgumentExpression(nameof(predicate))]
        string predicateExpression = ""
    )
    {
        builder.AddCondition(new WhereCondition<TNode>(predicateExpression, predicate));
        return builder;
    }
}
