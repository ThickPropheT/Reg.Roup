using TreeVal.Stage.Children;

namespace TreeVal.Stage.Eval.OneOf;

public class OneOfConditionsStageBuilder : VisitChildrenStageBuilder
{
    public OneOfConditionsStageBuilder()
    {
        BeforeLeaving((_, c) => new RejectIfAllBehaviorsFailed());
    }
}
