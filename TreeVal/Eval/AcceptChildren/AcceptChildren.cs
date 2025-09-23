using TreeVal.Media;

namespace TreeVal.Eval.AcceptChildren;

public class AcceptChildren : IAfterEnteringBehavior
{
    private readonly Node _parent;
    private readonly IVisitationRecorder _recorder;

    public AcceptChildren(Node parent, IVisitationRecorder recorder)
    {
        _parent = parent;
        _recorder = recorder;
    }

    public IStageContext Perform(IStageContext current)
    {
        return new StageContext();
    }
}
