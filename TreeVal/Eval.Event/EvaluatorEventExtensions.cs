using TreeVal.Scaffolding;

namespace TreeVal.Eval.Event;

public static class EvaluatorEventExtensions
{
    public static IEvaluatorBuilder OnAccept(
        this IEvaluatorBuilder builder, Action<object, IConditionEvaluation> callback)
        => OnStatus(builder, EvaluationStatus.Accepted, callback);

    public static IEvaluatorBuilder OnReject(
        this IEvaluatorBuilder builder, Action<object, IConditionEvaluation> callback)
        => OnStatus(builder, EvaluationStatus.Rejected, callback);

    public static IEvaluatorBuilder<T> OnAccept<T>(
        this IEvaluatorBuilder<T> builder, Action<T, IConditionEvaluation> callback)
        => OnStatus(builder, EvaluationStatus.Accepted, callback);

    public static IEvaluatorBuilder OnReject<T>(
        this IEvaluatorBuilder<T> builder, Action<T, IConditionEvaluation> callback)
        => OnStatus(builder, EvaluationStatus.Rejected, callback);

    private static TBuilder OnStatus<TBuilder, T>(
        TBuilder builder, EvaluationStatus status, Action<T, IConditionEvaluation> callback)
        where TBuilder : IEvaluatorBuilder
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
