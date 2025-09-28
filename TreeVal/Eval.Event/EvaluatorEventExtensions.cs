using TreeVal.Scaffolding;
using TreeVal.Stage.Eval;
using TreeVal.Visit;

namespace TreeVal.Eval.Event;

public static class EvaluatorEventExtensions
{
    public static IVisitorBuilder OnAccept(
        this IVisitorBuilder builder, Action<object, IConditionEvaluation> callback)
        => OnStatus(builder, EvaluationStatus.Accepted, callback);

    public static IVisitorBuilder OnReject(
        this IVisitorBuilder builder, Action<object, IConditionEvaluation> callback)
        => OnStatus(builder, EvaluationStatus.Rejected, callback);

    public static IVisitorBuilder<T> OnAccept<T>(
        this IVisitorBuilder<T> builder, Action<T, IConditionEvaluation> callback)
        => OnStatus(builder, EvaluationStatus.Accepted, callback);

    public static IVisitorBuilder OnReject<T>(
        this IVisitorBuilder<T> builder, Action<T, IConditionEvaluation> callback)
        => OnStatus(builder, EvaluationStatus.Rejected, callback);

    private static TBuilder OnStatus<TBuilder, T>(
        TBuilder builder, EvaluationStatus status, Action<T, IConditionEvaluation> callback)
        where TBuilder : IVisitorBuilder
    {
        builder.AddCondition(
            new Observer<T>((t, evaluation) =>
            {
                if (evaluation.Status != status)
                    return;

                callback(t, evaluation);
            }));

        return builder;
    }
}
