using TreeVal.Media;
using TreeVal.Scaffolding;

namespace TreeVal.Eval;

public static class CaseEvaluatorExtensions
{
    public static IEvaluatorBuilder<TNode> Case<TNode>(
        this IEvaluatorBuilder<TNode> builder,
        Func<ICase<IEvaluatorBuilder<TNode>>, TNode, IThen[]> body
    )
    {
        builder.AddChildren(node =>
        {
            var (_, childLookups) = ProxyEvaluatorBuilder<TNode>
                .Scoped(standIn =>
                    body(new CaseImpl<IEvaluatorBuilder<TNode>>(standIn), node)
                );

            return childLookups.SelectMany(childLookup => childLookup(new Node(node!)));
        });

        return builder;
    }

    private class CaseImpl<TBuilder> : ICase<TBuilder>
        where TBuilder : IEvaluatorBuilder
    {
        private readonly TBuilder _builder;

        public CaseImpl(TBuilder builder)
        {
            _builder = builder;
        }

        public ITarget<TBuilder, T> When<T>(T? target)
            => new TargetImpl<TBuilder, T>(_builder, target);
    }

    private class TargetImpl<TBuilder, T> : ITarget<TBuilder, T>
        where TBuilder : IEvaluatorBuilder
    {
        private readonly TBuilder _builder;
        private readonly T? _target;

        public TargetImpl(TBuilder builder, T? target)
        {
            _builder = builder;
            _target = target;
        }

        public IWhen<TBuilder, T> IsNotNull()
            => new WhenImpl<TBuilder, T>(_builder, _target);

        public IWhen<TBuilder, T> IsTrue(Func<T, bool> predicate)
            => new WhenImpl<TBuilder, T>(_builder, _target, predicate);
    }

    private class WhenImpl<TBuilder, T> : IWhen<TBuilder, T>
        where TBuilder : IEvaluatorBuilder
    {
        private readonly TBuilder _builder;
        private readonly T? _target;
        private readonly Func<T, bool>? _predicate;

        public WhenImpl(TBuilder builder, T? target, Func<T, bool>? predicate = null)
        {
            _builder = builder;
            _target = target;
            _predicate = predicate;
        }

        public IThen Then(Func<TBuilder, T, TBuilder> condition)
        {
            if (_target == null || _predicate?.Invoke(_target) == false)
                return ThenInstance;

            condition(_builder, _target!);
            return ThenInstance;
        }
    }

    // TODO this is sort of a weird thing to do. see TODO below
    private static readonly ThenImpl ThenInstance = new();

    private class ThenImpl : IThen
    {
    }
}

public interface IThen
{
}

public interface IWhen<TBuilder, out T>
    where TBuilder : IEvaluatorBuilder
{
    // TODO
    //  does this actually need to return IThen?
    //  it /does/ provide a warm & fuzzy that the caller
    //  of the outer Case(...) method is using it correctly.
    IThen Then(Func<TBuilder, T, TBuilder> condition);
}

public interface ITarget<TBuilder, out T>
    where TBuilder : IEvaluatorBuilder
{
    IWhen<TBuilder, T> IsNotNull();
    IWhen<TBuilder, T> IsTrue(Func<T, bool> predicate);
}

public interface ICase<TBuilder>
    where TBuilder : IEvaluatorBuilder
{
    ITarget<TBuilder, T> When<T>(T? target);
}
