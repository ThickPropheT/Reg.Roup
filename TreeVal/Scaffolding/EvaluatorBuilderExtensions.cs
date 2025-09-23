using TreeVal.Eval.Condition;

namespace TreeVal.Scaffolding;

public static class EvaluatorBuilderExtensions
{
    public static void AddCondition(this IEvaluatorBuilder builder, ICondition condition)
        => builder.AddConditions(_ => [condition]);
    
    public static void AddCondition<T>(this IEvaluatorBuilder<T> builder, ICondition<T> condition)
        => builder.AddConditions(_ => [condition]);
    
    public static void AddConditions<T>(
        this IEvaluatorBuilder<T> builder, Func<T, IEnumerable<ICondition>> getConditions)
        => builder.AddConditions(n => getConditions((T) n.Value));
    
    public static void AddChildren<T>(
        this IEvaluatorBuilder<T> builder, Func<T, IEnumerable<INodeEvaluatorFactory>> getChildren)
        => builder.AddChildren(n => getChildren((T) n.Value));
}
