using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public static class VisitorBuilderExtensions_Condition
{
    public static void AddCondition(this IVisitorBuilder builder, ICondition condition)
        => builder
            .Get<IEvaluateConditionsStageBuilder>()
            .OrCreateStage()
            .AddConditions(_ => [condition]);

    public static void AddCondition<T>(this IVisitorBuilder<T> builder, ICondition<T> condition)
        => builder.AddCondition((ICondition)condition);
    
    public static void AddConditions(this IVisitorBuilder builder, Func<Node, IEnumerable<ICondition>> getConditions)
        => builder
            .Get<IEvaluateConditionsStageBuilder>()
            .OrCreateStage()
            .AddConditions(n => getConditions(n));
    
    public static void AddConditions<T>(
        this IVisitorBuilder<T> builder, Func<T, IEnumerable<ICondition>> getConditions)
        => builder.AddConditions(n => getConditions((T) n.Value));
}

public static class VisitorBuilderExtensions_Children
{
    public static void AddChildren(this IVisitorBuilder builder, Func<Node, IEnumerable<IVisitorFactory>> getChildren)
        => builder
            .Get<IEvaluateChildrenStageBuilder>()
            .OrCreateStage()
            .AddChildren(getChildren);
    
    public static void AddChildren<TNode>(
        this IVisitorBuilder<TNode> builder, Func<TNode, IEnumerable<IVisitorFactory>> getChildren)
        => builder.AddChildren(n => getChildren((TNode) n.Value));
}
