using TreeVal.Stage.Children;
using TreeVal.Stage.Eval.Rejection;

namespace TreeVal.Stage.Eval.OneOf;

public class OneOfConditionsStageBuilder : VisitChildrenStageBuilder
{
    public OneOfConditionsStageBuilder()
    {
        BeforeLeaving((_, _) => new RejectIfAllBehaviorsFailed());
    }
}
