namespace TreeVal.Extensions;

public static class EachChildEvaluatorExtensions
{
    public static IEvaluatorBuilder<TNode> WithEachChildBeing<TNode, TChild>(
        this IEvaluatorBuilder<TNode> builder, 
        Func<TNode, IEnumerable<TChild>> selectChildren,
        Func<TChild, IEvaluatorNodeFactory> getEvaluator)
    {
        builder.AddChildren(node =>
        {
            if (node is not TNode n)
            {
                throw new InvalidOperationException();
            }

            return selectChildren(n).Select(getEvaluator);
        });

        return builder;
    }
}
