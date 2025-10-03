using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TreeVal.Media;

namespace TreeVal.Scaffolding.Stage;

public static class ScaffoldingExtensions
{
    public static IStageQuery<TStage> Get<TStage>(this IVisitorBuilder builder)
        where TStage : IVisitationStageBuilder
        => new StageQuery<TStage>(
            builder,
            builder.Originator.StageDirector.ValidateKey(new IVisitationStageBuilder.Identity<TStage>()));

    private class StageQuery<TStage> : IStageQuery<TStage>
        where TStage : IVisitationStageBuilder
    {
        private readonly IVisitorBuilder _builder;
        private readonly IVisitationStageBuilder.Identity<TStage> _key;

        public StageQuery(IVisitorBuilder builder, IVisitationStageBuilder.Identity<TStage> key)
        {
            _builder = builder;
            _key = key;
        }

        public void Stage(Action<TStage?> callback)
            => _builder.OnDiscovery((_, discovered) => callback((TStage?) discovered.Get(_key)));

        public IVisitorBuilder OrCreateStage([CallerMemberName] string callerMemberName = "")
            => OrCreateStage(_ =>
            {
                var s = _builder.Originator.StageDirector.Create(_key);
                s.CreationSite = callerMemberName;
                return s;
            });

        public IVisitorBuilder OrCreateStage(Func<Node, TStage> createStage)
        {
            _builder.OnDiscovery((n, discovered) =>
            {
                if (!TryGetStage(discovered, out var stage))
                {
                    stage = createStage(n);
                    discovered.Set(stage);
                }
            });

            return _builder;
        }

        public IVisitorBuilder OrCreateStage(
            Action<Node, TStage> callback, [CallerMemberName] string callerMemberName = "")
            => OrCreateStage(
                callback,
                _ =>
                {
                    var s = _builder.Originator.StageDirector.Create(_key);
                    s.CreationSite = callerMemberName;
                    return s;
                });

        public IVisitorBuilder OrCreateStage(Action<Node, TStage> callback, Func<Node, TStage> createStage)
        {
            _builder.OnDiscovery((n, discovered) =>
            {
                if (!TryGetStage(discovered, out var stage))
                {
                    stage = createStage(n);
                    discovered.Set(stage);
                }

                callback(n, stage);
            });

            return _builder;
        }

        private bool TryGetStage(IVisitorBuilder.IDiscovered discovered, [MaybeNullWhen(false)] out TStage stage)
        {
            var s = discovered.Get(_key);

            if (s is TStage ts)
            {
                stage = ts;
                return true;
            }

            stage = default;
            return false;
        }
    }
}
