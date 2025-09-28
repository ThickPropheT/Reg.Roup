using TreeVal.Scaffolding;

namespace TreeVal.Stage.Children.EachChild;

public static class ScaffoldingExtensions
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
