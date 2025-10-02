using TreeVal.Media;
using TreeVal.Scaffolding.Stage;
using TreeVal.Stage.Read;

namespace TreeVal.Stage.Children.SkipChildren;

public class MovePastChildrenStageBuilder : VisitationStageBuilder, ReadNodeStage.IBuilder
{
    public MovePastChildrenStageBuilder(Node parent, IVisitationRecorder recorder)
        : base(ReadNodeStage.Key)
    {
        AfterEntering((_, _) => new MediaBehavior.SkipChildren(parent, recorder));
    }
}