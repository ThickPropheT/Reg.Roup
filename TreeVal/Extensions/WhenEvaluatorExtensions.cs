using System.Linq.Expressions;

namespace TreeVal.Extensions;

public static class WhenEvaluatorExtensions
{
    public static WhenEvaluatorBuilder<IEvaluatorBuilder<Expression>, T> When<T>(
        this IEvaluatorBuilder<Expression> builder, T? target)
        => new(builder, () => target);

    public static WhenEvaluatorBuilder<IEvaluatorBuilder<Expression>, T> When<T>(
        this IEvaluatorBuilder<Expression> builder, Func<T?> getTarget)
        => new(builder, getTarget);

    public static WhenEvaluatorBuilder<IEvaluatorBuilder<TExpression>, T> When<TExpression, T>(
        this IEvaluatorBuilder<TExpression> builder, T? target
    )
        where TExpression : Expression
        => new(builder, () => target);

    public static WhenEvaluatorBuilder<IEvaluatorBuilder<TExpression>, T> When<TExpression, T>(
        this IEvaluatorBuilder<TExpression> builder, Func<T?> getTarget
    )
        where TExpression : Expression
        => new(builder, getTarget);
}

public class WhenEvaluatorBuilder<TBuilder, T>
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
