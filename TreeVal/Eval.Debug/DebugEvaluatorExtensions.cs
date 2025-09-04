using TreeVal.Eval.Condition;
using TreeVal.Expr;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval.Debug;

public static class DebugEvaluatorExtensions
{
    public static IEvaluatorBuilder Debug(this IEvaluatorBuilder builder, Action<object, IConditionEvaluation> observe)
    {
        builder.AddCondition(new Observer(observe));
        return builder;
    }

    public static IEvaluatorBuilder<T> Debug<T>(
        this IEvaluatorBuilder<T> builder, Action<T, IConditionEvaluation> observe)
    {
        builder.AddCondition(new Observer<T>(observe));
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

    private class AttachDebuggerEvaluator : INodeEvaluator
    {
        private readonly INodeEvaluator _target;
        private readonly Func<INodeEvaluatorFactory> _buildDebugEvaluator;

        public IEnumerable<ICondition> Conditions => _target.Conditions;

        public VisitationContext.MovementStrategy HeadMovementStrategy => _target.HeadMovementStrategy;

        public VisitationContext.EvaluationStrategy? ChildEvaluationStrategy => _target.ChildEvaluationStrategy;

        public AttachDebuggerEvaluator(INodeEvaluator target, Func<INodeEvaluatorFactory> buildDebugEvaluator)
        {
            _target = target;
            _buildDebugEvaluator = buildDebugEvaluator;
        }

        public IEnumerable<INodeEvaluatorFactory> EnumerateChildren(Node current)
        {
            yield return _buildDebugEvaluator();

            foreach (var childBuilder in _target.EnumerateChildren(current))
            {
                yield return childBuilder;
            }

            ;
        }
    }
}
