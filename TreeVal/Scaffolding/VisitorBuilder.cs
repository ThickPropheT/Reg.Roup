using TreeVal.Media;
using TreeVal.Scaffolding.Stage;
using TreeVal.Visit;

namespace TreeVal.Scaffolding;

public class VisitorBuilder : IVisitorBuilder
{
    private readonly IStageDirector _director;
    private Action<Node, IVisitorBuilder.IDiscovered>? _discover;

    public IVisitorBuilderFactory Originator { get; }
    public string CreatedBy { get; init; } = "";

    public VisitorBuilder(IVisitorBuilderFactory originator)
    {
        _director = originator.StageDirector;
        Originator = originator;
    }

    public void OnDiscovery(Action<Node, IVisitorBuilder.IDiscovered> callback)
    {
        var discover = _discover;
        _discover = (n, discovered) =>
        {
            discover?.Invoke(n, discovered);
            callback(n, discovered);
        };
    }

    public virtual IVisitor CreateVisitor(Node node)
        => new Visitor(
            _director
                .Arrange(DiscoverStages(node))
                .Select(builder => builder.CreateStage())
        )
        {
            CreatedBy = CreatedBy
        };

    protected IEnumerable<IVisitationStageBuilder> DiscoverStages(Node node)
    {
        if (_discover == null)
            return [];

        var discovered = new Discovered();
        _discover(node, discovered);
        return discovered.Stages;
    }

    private class Discovered : IVisitorBuilder.IDiscovered
    {
        private readonly Dictionary<IVisitationStageBuilder.Identity, IVisitationStageBuilder> _stages = new(1);

        public IVisitationStageBuilder? Get(IVisitationStageBuilder.Identity key)
            => _stages.GetValueOrDefault(key);

        public void Set(IVisitationStageBuilder builder)
            => _stages[builder.Key] = builder;

        public IEnumerable<IVisitationStageBuilder> Stages => _stages.Values;
    }
}

public class VisitorBuilder<TNode> : VisitorBuilder, IVisitorBuilder<TNode>
{
    public VisitorBuilder(IVisitorBuilderFactory originator)
        : base(originator)
    {
    }
}
