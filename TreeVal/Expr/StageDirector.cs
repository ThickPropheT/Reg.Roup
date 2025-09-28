using TreeVal.Scaffolding.Stage;

namespace TreeVal.Expr;

public class StageDirector : IStageDirector
{
    private readonly Dictionary<IVisitationStageBuilder.Identity, StageInfo> _stagesInfo = new(1);

    public Func<IVisitationStageBuilder> this[IVisitationStageBuilder.Identity key]
    {
        set => _stagesInfo.Add(key, new StageInfo(value) { Priority = _stagesInfo.Count });
    }

    public TKey ValidateKey<TKey>(TKey key)
        where TKey : IVisitationStageBuilder.Identity
        => !_stagesInfo.ContainsKey(key)
            // TODO
            ? throw new InvalidOperationException()
            : key;

    public TStage Create<TStage>(IVisitationStageBuilder.Identity<TStage> key)
        where TStage : IVisitationStageBuilder
    {
        key = ValidateKey(key);
        return (TStage) _stagesInfo[key].BuildStage();
    }

    public IEnumerable<IVisitationStageBuilder> Arrange(IEnumerable<IVisitationStageBuilder> stages)
        => stages.OrderBy(s =>
        {
            var key = ValidateKey(s.Key);
            return _stagesInfo[key].Priority;
        });

    private class StageInfo
    {
        private readonly Func<IVisitationStageBuilder> _buildStage;

        public int Priority { get; init; }

        public StageInfo(Func<IVisitationStageBuilder> buildStage)
        {
            _buildStage = buildStage;
        }

        public IVisitationStageBuilder BuildStage()
            => _buildStage();
    }
}
