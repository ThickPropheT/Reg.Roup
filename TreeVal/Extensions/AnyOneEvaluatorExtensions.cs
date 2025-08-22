using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class AnyOneEvaluatorExtensions
{
    private static readonly VisitorNodeFactory Factory = new();
    
    public static IEvaluatorBuilder AnyOne(this IVisitorNodeFactory factory)
        => factory.OfType<Expression>();

    public static IEvaluatorConditionBuilder AcceptChildren(this IVisitorNodeFactory _, Expression parent)
        => new ProxyEvaluatorBuilder((conditions, _) => new AcceptChildrenEvaluatorNode(conditions, parent));

    public static IEvaluatorConditionBuilder<TNode> AcceptChildren<TNode>(this IEvaluatorBuilder<TNode> builder)
        where TNode : Expression
        => builder.HavingChild(parent => Factory.AcceptChildren(parent));
}
