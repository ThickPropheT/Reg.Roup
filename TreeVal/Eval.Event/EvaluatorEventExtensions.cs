using TreeVal.Eval.Debug;
using TreeVal.Scaffolding;

namespace TreeVal.Eval.Event;

public static class EvaluatorEventExtensions
{
    public static IEvaluatorBuilder OnAccept(this IEvaluatorBuilder builder, Action<object, Evaluation> callback)
        => OnStatus(builder, Evaluation.Status.Accepted, callback);

    public static IEvaluatorBuilder OnReject(this IEvaluatorBuilder builder, Action<object, Evaluation> callback)
        => OnStatus(builder, Evaluation.Status.Rejected, callback);
    
    public static IEvaluatorBuilder<T> OnAccept<T>(this IEvaluatorBuilder<T> builder, Action<T, Evaluation> callback)
        => OnStatus(builder, Evaluation.Status.Accepted, callback);

    public static IEvaluatorBuilder OnReject<T>(this IEvaluatorBuilder<T> builder, Action<T, Evaluation> callback)
        => OnStatus(builder, Evaluation.Status.Rejected, callback);

    private static TBuilder OnStatus<TBuilder, T>(TBuilder builder, Evaluation.Status status, Action<T, Evaluation> callback)
        where TBuilder : IEvaluatorBuilder
    {
        builder.AddCondition(new Observer<T>((t, evaluation) =>
        {
            if (evaluation.CurrentStatus != status)
                return;

            callback(t, evaluation);
        }));

        return builder;
    }
}
