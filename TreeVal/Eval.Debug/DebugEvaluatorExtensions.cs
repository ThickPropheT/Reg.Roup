using TreeVal.Scaffolding;

namespace TreeVal.Eval.Debug;

public static class DebugEvaluatorExtensions
{
    public static IEvaluatorBuilder Debug(this IEvaluatorBuilderFactory _, Action<object, Evaluation> observe)
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(new Observer(observe));
        return builder;
    }

    public static IEvaluatorBuilder Debug(this IEvaluatorBuilder builder, Action<object, Evaluation> observe)
    {
        builder.AddCondition(new Observer(observe));
        return builder;
    }
    
    public static IEvaluatorBuilder<T> Debug<T>(this IEvaluatorBuilder<T> builder, Action<T, Evaluation> observe)
    {
        builder.AddCondition(new Observer<T>(observe));
        return builder;
    }
}
