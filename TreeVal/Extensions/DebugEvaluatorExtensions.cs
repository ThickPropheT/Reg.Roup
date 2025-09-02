using TreeVal.Condition;

namespace TreeVal.Extensions;

public static class DebugEvaluatorExtensions
{
    public static IEvaluatorBuilder Debug(this IVisitorNodeFactory _, Action<object, Evaluation> observe)
    {
        var builder = new EvaluatorBuilder();
        builder.AddCondition(new Observer(observe));
        return builder;
    }


    public static IEvaluatorBuilder<T> Debug<T>(this IEvaluatorBuilder<T> builder, Action<T, Evaluation> observe)
    {
        builder.AddCondition(new Observer((o, evaluation) => observe((T) o, evaluation)));
        return builder;
    }

    private class Observer : ICondition
    {
        private readonly Action<object, Evaluation> _observe;

        public Observer(Action<object, Evaluation> observe)
        {
            _observe = observe;
        }

        public void Describe(IDescription description)
        {
            throw new NotImplementedException();
        }

        public void Evaluate(Node node, Evaluation evaluation)
            => _observe(node.Value, evaluation);
    }
}
