using TreeVal.Media;

namespace TreeVal.Scaffolding.Stage.Create;

public static class ScaffoldingExtensions
{
    public static ICreateStage<TStage> Create<TStage>(this Accessors<TStage> accessors)
        where TStage : IVisitationStageBuilder
        => new CreateStage<TStage>(accessors.Builder);

    private class CreateStage<TStage> : ICreateStage<TStage>
        where TStage : IVisitationStageBuilder
    {
        private readonly IVisitorBuilder _builder;

        public CreateStage(IVisitorBuilder builder)
        {
            _builder = builder;
        }

        public IVisitorBuilder OrUpdate(Func<Node, TStage> createStage)
        {
            _builder.OnDiscovery((n, discovered) =>
            {
                var stage = createStage(n);
                discovered.Set(stage);
            });

            return _builder;
        }

        public IVisitorBuilder OrUpdate(Action<Node, TStage> callback, Func<Node, TStage> createStage)
        {
            _builder.OnDiscovery((n, discovered) =>
            {
                var stage = createStage(n);
                discovered.Set(stage);
                callback(n, stage);
            });

            return _builder;
        }
    }
}
