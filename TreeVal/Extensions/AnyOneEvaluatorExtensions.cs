using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class AnyOneEvaluatorExtensions
{
    private static readonly VisitorNodeFactory Factory = new();
    
    public static IEvaluatorBuilder AnyOne(this VisitorNodeFactory factory)
        => factory.OfType<Expression>();

    public static IEvaluatorConditionBuilder AcceptChildren(this VisitorNodeFactory _, Expression parent)
        => new ProxyEvaluatorBuilder((conditions, _) => new AcceptChildrenEvaluatorNode(conditions, parent));

    public static IEvaluatorConditionBuilder<TNode> AcceptChildren<TNode>(this IEvaluatorBuilder<TNode> builder)
        where TNode : Expression
        => builder.HavingChild(parent => Factory.AcceptChildren(parent));
}
