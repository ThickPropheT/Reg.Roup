using TreeVal.Media;
using TreeVal.Scaffolding;
using TreeVal.Scaffolding.Stage;

namespace TreeVal.Stage.Eval;

public static class EvaluateConditionsStage
{
    public interface IBuilder : IVisitationStageBuilder
    {
        void AddConditions(Func<IEnumerable<ICondition>> getConditions);
    }

    public static IVisitationStageBuilder.Identity<IBuilder> Key { get; } = new();

    public static IStageQuery<IBuilder> GetEvaluationStage(this IVisitorBuilder builder)
        => builder.Get<IBuilder>();

    public static void AddCondition(this IVisitorBuilder builder, ICondition condition)
        => builder
            .GetEvaluationStage()
            .OrCreateStage((_, conditionsStage) => conditionsStage.AddConditions(() => [condition]));

    public static void AddCondition<T>(this IVisitorBuilder<T> builder, ICondition<T> condition)
        => builder.AddCondition((ICondition) condition);

    public static void AddConditions(this IVisitorBuilder builder, Func<Node, IEnumerable<ICondition>> getConditions)
        => builder
            .GetEvaluationStage()
            .OrCreateStage((n, conditionsStage) => conditionsStage.AddConditions(() => getConditions(n)));

    public static void AddConditions<T>(
        this IVisitorBuilder<T> builder, Func<T, IEnumerable<ICondition>> getConditions)
        => builder.AddConditions(n => getConditions((T) n.Value));
}
