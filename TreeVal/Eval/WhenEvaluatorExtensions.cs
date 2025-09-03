using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public static class WhenEvaluatorExtensions
{
    public static WhenEvaluatorBuilder<IEvaluatorBuilder, T> When<T>(
        this IEvaluatorBuilder builder, T? target)
        => new(builder, () => target);

    public static WhenEvaluatorBuilder<IEvaluatorBuilder, T> When<T>(
        this IEvaluatorBuilder builder, Func<T?> getTarget)
        => new(builder, getTarget);

    public static WhenEvaluatorBuilder<IEvaluatorBuilder<TNode>, T> When<TNode, T>(
        this IEvaluatorBuilder<TNode> builder, T? target)
        => new(builder, () => target);

    public static WhenEvaluatorBuilder<IEvaluatorBuilder<TNode>, T> When<TNode, T>(
        this IEvaluatorBuilder<TNode> builder, Func<T?> getTarget)
    
        => new(builder, getTarget);
}

public class WhenEvaluatorBuilder<TBuilder, T>
    where TBuilder : IEvaluatorBuilder
{
    private readonly TBuilder _builder;
    private readonly Func<T?> _getTarget;

    public WhenEvaluatorBuilder(
        TBuilder builder, Func<T?> getTarget)
    {
        _builder = builder;
        _getTarget = getTarget;
    }

    public NotNull IsNotNull()
        => new(_builder, _getTarget);

    // TODO is this even useful?
    public True IsTrue(Func<T, bool> predicate)
        => new(_builder, _getTarget, predicate);

    public class NotNull
    {
        private readonly TBuilder _builder;
        private readonly Func<T?> _getTarget;

        public NotNull(TBuilder builder, Func<T?> getTarget)
        {
            _builder = builder;
            _getTarget = getTarget;
        }

        public TBuilder Then(Func<TBuilder, T, TBuilder> condition)
        {
            var target = _getTarget();

            return target != null
                ? condition(_builder, target)
                : _builder;
        }
    }

    public class True
    {
        private readonly TBuilder _builder;
        private readonly Func<T?> _getTarget;
        private readonly Func<T, bool> _predicate;

        public True(TBuilder builder, Func<T?> getTarget, Func<T, bool> predicate)
        {
            _builder = builder;
            _getTarget = getTarget;
            _predicate = predicate;
        }

        public TBuilder Then(Func<TBuilder, T, TBuilder> condition)
        {
            var target = _getTarget();

            return target != null && _predicate(target)
                ? condition(_builder, target)
                : _builder;
        }
    }
}
