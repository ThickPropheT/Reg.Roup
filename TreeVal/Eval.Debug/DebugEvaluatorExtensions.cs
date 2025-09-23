using TreeVal.Eval.Condition;
using TreeVal.Expr;
using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval.Debug;

public static class DebugEvaluatorExtensions
{
    public static IVisitorBuilder Debug(
        this IVisitorBuilder builder, Action<object, IConditionEvaluation> observe)
    {
        builder.AddCondition(new Observer(observe));
        return builder;
    }

    public static IVisitorBuilder Debug(
        this IVisitorBuilder builder, string label, Action<object, IConditionEvaluation> observe)
    {
        builder.AddCondition(new Observer(observe) { Label = label });
        return builder;
    }

    public static IVisitorBuilder<T> Debug<T>(
        this IVisitorBuilder<T> builder, Action<T, IConditionEvaluation> observe)
    {
        builder.AddCondition(new Observer<T>(observe));
        return builder;
    }

    public static IVisitorBuilder<T> Debug<T>(
        this IVisitorBuilder<T> builder, string label, Action<T, IConditionEvaluation> observe)
    {
        builder.AddCondition(new Observer<T>(observe) { Label = label });
        return builder;
    }

    public static IVisitorBuilderFactory Debug(
        this IVisitorBuilderFactory factory, Action<object, IConditionEvaluation> observe)
        => new BuilderFactoryAspect(
            factory,
            visitor => new AttachDebuggerVisitor(
                visitor,
                () =>
                    factory
                        .AnyOne()
                        .Debug((o, evaluation) => observe(o, evaluation))));

    public static IVisitorBuilderFactory Debug(
        this IVisitorBuilderFactory factory, string label, Action<object, IConditionEvaluation> observe)
        => new BuilderFactoryAspect(
            factory,
            visitor => new AttachDebuggerVisitor(
                visitor,
                () =>
                    factory
                        .AnyOne()
                        .Debug(label, (o, evaluation) => observe(o, evaluation))));

    private class AttachDebuggerVisitor : IVisitor
    {
        private readonly IVisitor _target;
        private readonly Func<IVisitorFactory> _buildDebugEvaluator;

        public AttachDebuggerVisitor(IVisitor target, Func<IVisitorFactory> buildDebugEvaluator)
        {
            _target = target;
            _buildDebugEvaluator = buildDebugEvaluator;
        }
        
        public IEnumerable<ICondition> EnumerateConditions(Node current)
            => _target.EnumerateConditions(current);

        public IEnumerable<IVisitorFactory> EnumerateChildren(Node current)
        {
            throw new NotImplementedException(
                "I think this debug option may not be 'invisible'. I think it's inclusion in a schema causes a double move-forward of the tape head.");
            
            yield return _buildDebugEvaluator();

            foreach (var childBuilder in _target.EnumerateChildren(current))
            {
                yield return childBuilder;
            }
        }

        public void Visit(TapeHead head)
        {
            
        }
    }
}
