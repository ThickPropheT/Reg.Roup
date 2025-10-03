using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using TreeVal.Media;

namespace TreeVal.Scaffolding.Stage.Get;

public static class ScaffoldingExtensions
{
    public static IGetStage<TStage> Get<TStage>(this Accessors<TStage> accessors)
        where TStage : IVisitationStageBuilder
        => new GetStage<TStage>(
            accessors.Builder,
            accessors.StageDirector.ValidateKey(new IVisitationStageBuilder.Identity<TStage>())
        );

    private class GetStage<TStage> : IGetStage<TStage>
        where TStage : IVisitationStageBuilder
    {
        private readonly IVisitorBuilder _builder;
        private readonly IVisitationStageBuilder.Identity<TStage> _key;

        public GetStage(IVisitorBuilder builder, IVisitationStageBuilder.Identity<TStage> key)
        {
            _builder = builder;
            _key = key;
        }

        public void Stage(Action<TStage?> callback)
            => _builder.OnDiscovery((_, discovered) => callback((TStage?) discovered.Get(_key)));

        public IVisitorBuilder OrCreate([CallerMemberName] string callerMemberName = "")
            => OrCreate(_ =>
            {
                var s = _builder.StageDirector.Create(_key);
                s.CreationSite = callerMemberName;
                return s;
            });

        public IVisitorBuilder OrCreate(Func<Node, TStage> createStage)
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

        public IVisitorBuilder OrCreate(
            Action<Node, TStage> callback, [CallerMemberName] string callerMemberName = "")
            => OrCreate(
                callback,
                _ =>
                {
                    var s = _builder.StageDirector.Create(_key);
                    s.CreationSite = callerMemberName;
                    return s;
                });

        public IVisitorBuilder OrCreate(Action<Node, TStage> callback, Func<Node, TStage> createStage)
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
