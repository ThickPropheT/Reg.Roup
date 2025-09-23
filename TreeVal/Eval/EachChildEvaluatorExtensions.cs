using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public static class EachChildEvaluatorExtensions
{
    public static IVisitorBuilder<TNode> WithEachChildBeing<TNode, TChild>(
        this IVisitorBuilder<TNode> builder,
        Func<TNode, IEnumerable<TChild>> selectChildren,
        Func<TChild, IVisitorFactory> getEvaluator
    )
    {
        builder.AddChildren(node => selectChildren(node).Select(getEvaluator));
        return builder;
    }
}
