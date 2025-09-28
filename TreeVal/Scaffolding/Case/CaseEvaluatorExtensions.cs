using TreeVal.Media;

namespace TreeVal.Scaffolding.Case;

public static class CaseEvaluatorExtensions
{
    public static IVisitorBuilder<TNode> Case<TNode>(
        this IVisitorBuilder<TNode> builder,
        Func<ICase<IVisitorBuilder<TNode>>, TNode, IThen[]> body
    )
    {
        builder.OnDiscovery((n, discovered) =>
            body(
                new CaseImpl<TNode>(new DiscoveryRecorder<TNode>(builder.Originator, n, discovered)),
                (TNode) n.Value
            ));

        return builder;
    }

    class DiscoveryRecorder<TNode> : VisitorBuilder<TNode>
    {
        private readonly IVisitorBuilder.IDiscovered _discovered;
        private readonly Node _node;

        public DiscoveryRecorder(IVisitorBuilderFactory originator, Node node, IVisitorBuilder.IDiscovered discovered)
            : base(originator)
        {
            _node = node;
            _discovered = discovered;
        }

        public void RecordDiscoveries(Action during)
        {
            during();

            foreach (var stage in DiscoverStages(_node))
            {
                _discovered.Set(stage);
            }
        }
    }

    private class CaseImpl<TNode> : ICase<IVisitorBuilder<TNode>>
    {
        private readonly DiscoveryRecorder<TNode> _recorder;

        public CaseImpl(DiscoveryRecorder<TNode> recorder)
        {
            _recorder = recorder;
        }

        public ITarget<IVisitorBuilder<TNode>, T> When<T>(T? target)
            => new TargetImpl<TNode, T>(_recorder, target);
    }

    private class TargetImpl<TNode, T> : ITarget<IVisitorBuilder<TNode>, T>
    {
        private readonly DiscoveryRecorder<TNode> _recorder;
        private readonly T? _target;

        public TargetImpl(DiscoveryRecorder<TNode> recorder, T? target)
        {
            _recorder = recorder;
            _target = target;
        }

        public IWhen<IVisitorBuilder<TNode>, T> IsNotNull()
            => new WhenImpl<TNode, T>(_recorder, _target);

        public IWhen<IVisitorBuilder<TNode>, T> IsTrue(Func<T, bool> predicate)
            => new WhenImpl<TNode, T>(_recorder, _target, predicate);
    }

    private class WhenImpl<TNode, T> : IWhen<IVisitorBuilder<TNode>, T>
    {
        private readonly DiscoveryRecorder<TNode> _recorder;
        private readonly T? _target;
        private readonly Func<T, bool>? _predicate;

        public WhenImpl(DiscoveryRecorder<TNode> recorder, T? target, Func<T, bool>? predicate = null)
        {
            _recorder = recorder;
            _target = target;
            _predicate = predicate;
        }

        public IThen Then(Func<IVisitorBuilder<TNode>, T, IVisitorBuilder<TNode>> callback)
        {
            if (_target != null && _predicate?.Invoke(_target) != false)
            {
                _recorder.RecordDiscoveries(during: () => callback(_recorder, _target));
            }

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
    where TBuilder : IVisitorBuilder
{
    // TODO
    //  does this actually need to return IThen?
    //  it /does/ provide a warm & fuzzy that the caller
    //  of the outer Case(...) method is using it correctly.
    IThen Then(Func<TBuilder, T, TBuilder> callback);
}

public interface ITarget<TBuilder, out T>
    where TBuilder : IVisitorBuilder
{
    IWhen<TBuilder, T> IsNotNull();
    IWhen<TBuilder, T> IsTrue(Func<T, bool> predicate);
}

public interface ICase<TBuilder>
    where TBuilder : IVisitorBuilder
{
    ITarget<TBuilder, T> When<T>(T? target);
}
