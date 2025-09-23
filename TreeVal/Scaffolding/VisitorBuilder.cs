using TreeVal.Eval;

namespace TreeVal.Scaffolding;

public class VisitorBuilder : IVisitorBuilder
{
    private readonly IStageDirector _director;
    private readonly Dictionary<object, IVisitationStageBuilder> _stages = new(1);
    
    public IVisitorBuilderFactory Originator { get; }

    public VisitorBuilder(IVisitorBuilderFactory originator)
    {
        _director = originator.StageDirector;
        Originator = originator;
    }

    public IVisitorBuilder.IStageQuery<TStage> Get<TStage>()
        where TStage : IVisitationStageBuilder
        => Get(new IVisitorBuilder.Key<TStage>(this));

    public IVisitorBuilder.IStageQuery<TStage> Get<TStage>(IVisitorBuilder.Key<TStage> key)
        where TStage : IVisitationStageBuilder
        => new StageQuery<TStage>(this, _director.ValidateKey(key));

    public virtual IVisitor CreateVisitor()
        => new Visitor(
            _director
                .Arrange(_stages.Values)
                .Select(builder => builder.CreateStage())
        );

    private class StageQuery<TStage> : IVisitorBuilder.IStageQuery<TStage>
        where TStage : IVisitationStageBuilder
    {
        private readonly VisitorBuilder _builder;
        private readonly IVisitorBuilder.Key<TStage> _key;

        public StageQuery(VisitorBuilder builder, IVisitorBuilder.Key<TStage> key)
        {
            _builder = builder;
            _key = key;
        }

        public TStage? Stage()
            => _builder._stages.TryGetValue(_key, out var stage)
                ? (TStage) stage
                : default;

        public TStage OrCreateStage(Func<TStage>? createStage = null)
        {
            createStage ??= () => _builder._director.Create<TStage>();

            if (!_builder._stages.TryGetValue(_key, out var stage))
            {
                stage = createStage();
                _builder._stages[_key] = stage;
            }

            return (TStage) stage;
        }
    }
}

public class VisitorBuilder<TNode> : VisitorBuilder, IVisitorBuilder<TNode>
{
    public VisitorBuilder(IVisitorBuilderFactory originator)
        : base(originator)
    {
    }
}

public static class DefaultVisitorBuilder
{
    public static VisitorBuilder Create(IVisitorBuilderFactory creator)
    {
        var builder = new VisitorBuilder(creator);

        builder
            .Get<IReadNodeStageBuilder>()
            .OrCreateStage(() => new MoveForwardStageBuilder());

        return builder;
    }
    
    public static VisitorBuilder<TNode> Create<TNode>(IVisitorBuilderFactory creator)
    {
        var builder = new VisitorBuilder<TNode>(creator);

        builder
            .Get<IReadNodeStageBuilder>()
            .OrCreateStage(() => new MoveForwardStageBuilder());

        return builder;
    }
}

public class MoveForwardStageBuilder : VisitationStageBuilder, IReadNodeStageBuilder
{
    public MoveForwardStageBuilder()
    {
        AfterEntering(_ => new MediaBehavior.MoveForward());
    }
}
