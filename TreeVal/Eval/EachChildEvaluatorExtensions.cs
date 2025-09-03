using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public static class EachChildEvaluatorExtensions
{
    public static IEvaluatorBuilder<TNode> WithEachChildBeing<TNode, TChild>(
        this IEvaluatorBuilder<TNode> builder,
        Func<TNode, IEnumerable<TChild>> selectChildren,
        Func<TChild, INodeEvaluatorFactory> getEvaluator
    )
    {
        builder.AddChildren(node => selectChildren(node).Select(getEvaluator));
        return builder;
    }
}
