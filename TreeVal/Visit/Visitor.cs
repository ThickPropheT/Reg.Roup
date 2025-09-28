using TreeVal.Media;
using TreeVal.Visit.Behavior;
using TreeVal.Visit.Stage;

namespace TreeVal.Visit;

public class Visitor : IVisitor
{
    private readonly IEnumerable<IVisitationStage> _stages;

    public Visitor(IEnumerable<IVisitationStage> stages)
    {
        _stages = stages;
    }

    public void Visit(TapeHead head)
    {
        IStageContext stageContext = new InitialStageContext(head);

        foreach (var stage in _stages)
        {
            stageContext = stage.Visit(head, stageContext);
        }
    }

    // TODO should this actually do anything or just throw?
    private class InitialStageContext : IStageContext
    {
        public ITapeHead TapeHead { get; }

        public IEnumerable<BehaviorContext> Visitations { get; }

        public InitialStageContext(ITapeHead tapeHead)
        {
            TapeHead = tapeHead;
        }

        public void RecordVisitation(BehaviorContext visitation)
        {
            throw new NotImplementedException();
        }
    }
}
