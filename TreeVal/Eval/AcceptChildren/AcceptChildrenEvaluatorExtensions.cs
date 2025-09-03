using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval.AcceptChildren;

public static class AcceptChildrenEvaluatorExtensions
{
    private static readonly DefaultEvaluatorBuilderFactory Factory = new();

    public static IEvaluatorBuilder AcceptChildren<T>(this IEvaluatorBuilderFactory _, T parent)
        => new ProxyEvaluatorBuilder((conditions, _) =>
            new AcceptChildrenNodeEvaluator(conditions, new Node<T>(parent), new LinearExpressionTreeRecorder()));

    public static IEvaluatorBuilder<T> AcceptChildren<T>(this IEvaluatorBuilder<T> builder)
        => builder.HavingChild(parent => Factory.AcceptChildren(parent));
}
