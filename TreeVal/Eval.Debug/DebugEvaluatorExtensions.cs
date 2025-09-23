using TreeVal.Eval.Condition;
using TreeVal.Expr;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval.Debug;

public static class DebugEvaluatorExtensions
{
    public static IEvaluatorBuilder Debug(
        this IEvaluatorBuilder builder, Action<object, IConditionEvaluation> observe)
    {
        builder.AddCondition(new Observer(observe));
        return builder;
    }

    public static IEvaluatorBuilder Debug(
        this IEvaluatorBuilder builder, string label, Action<object, IConditionEvaluation> observe)
    {
        builder.AddCondition(new Observer(observe) { Label = label });
        return builder;
    }

    public static IEvaluatorBuilder<T> Debug<T>(
        this IEvaluatorBuilder<T> builder, Action<T, IConditionEvaluation> observe)
    {
        builder.AddCondition(new Observer<T>(observe));
        return builder;
    }

    public static IEvaluatorBuilder<T> Debug<T>(
        this IEvaluatorBuilder<T> builder, string label, Action<T, IConditionEvaluation> observe)
    {
        builder.AddCondition(new Observer<T>(observe) { Label = label });
        return builder;
    }

    public static IEvaluatorBuilderFactory Debug(
        this IEvaluatorBuilderFactory factory, Action<object, IConditionEvaluation> observe)
        => new BuilderFactoryAspect(
            factory,
            evaluator => new AttachDebuggerEvaluator(
                evaluator,
                () =>
                    factory
                        .AnyOne()
                        .Debug((o, evaluation) => observe(o, evaluation))));

    public static IEvaluatorBuilderFactory Debug(
        this IEvaluatorBuilderFactory factory, string label, Action<object, IConditionEvaluation> observe)
        => new BuilderFactoryAspect(
            factory,
            evaluator => new AttachDebuggerEvaluator(
                evaluator,
                () =>
                    factory
                        .AnyOne()
                        .Debug(label, (o, evaluation) => observe(o, evaluation))));

    private class AttachDebuggerEvaluator : INodeEvaluator
    {
        private readonly INodeEvaluator _target;
        private readonly Func<INodeEvaluatorFactory> _buildDebugEvaluator;

        public VisitationContext.MovementStrategy HeadMovementStrategy => _target.HeadMovementStrategy;

        public VisitationContext.EvaluationStrategy? ChildEvaluationStrategy => _target.ChildEvaluationStrategy;

        public AttachDebuggerEvaluator(INodeEvaluator target, Func<INodeEvaluatorFactory> buildDebugEvaluator)
        {
            _target = target;
            _buildDebugEvaluator = buildDebugEvaluator;
        }

        public IEnumerable<ICondition> EnumerateConditions(Node current)
            => _target.EnumerateConditions(current);

        public IEnumerable<INodeEvaluatorFactory> EnumerateChildren(Node current)
        {
            throw new NotImplementedException(
                "I think this debug option may not be 'invisible'. I think it's inclusion in a schema causes a double move-forward of the tape head.");

            yield return _buildDebugEvaluator();

            foreach (var childBuilder in _target.EnumerateChildren(current))
            {
                yield return childBuilder;
            }
        }
    }
}
