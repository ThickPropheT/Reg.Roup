using TreeVal.Condition;
using TreeVal.Media;

namespace TreeVal.Extensions;

public static class AcceptChildrenEvaluatorExtensions
{
    private static readonly DefaultVisitorNodeFactory Factory = new();

    public static IEvaluatorConditionBuilder AcceptChildren<T>(this IVisitorNodeFactory _, T parent)
        => new ProxyEvaluatorBuilder((conditions, _) => 
            new AcceptChildrenNodeEvaluator(conditions, new Node<T>(parent), new LinearExpressionTreeRecorder()));

    public static IEvaluatorConditionBuilder<T> AcceptChildren<T>(this IEvaluatorBuilder<T> builder)
        => builder.HavingChild(parent => Factory.AcceptChildren(parent));
}
