using TreeVal.Eval.Condition;

namespace TreeVal.Scaffolding;

public static class EvaluatorBuilderExtensions
{
    public static void AddCondition<T>(this IEvaluatorBuilder<T> builder, ICondition<T> condition)
        => builder.AddCondition(condition);
    
    public static void AddChildren<T>(
        this IEvaluatorBuilder<T> builder, Func<T, IEnumerable<INodeEvaluatorFactory>> getChildren)
        => builder.AddChildren(n => getChildren((T) n.Value));
}
