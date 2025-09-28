using System.Runtime.CompilerServices;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Stage.Eval.Where;

public static class ScaffoldingExtensions
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
