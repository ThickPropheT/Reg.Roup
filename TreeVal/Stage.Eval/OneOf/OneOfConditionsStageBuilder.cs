using TreeVal.Stage.Children;

namespace TreeVal.Stage.Eval.OneOf;

public class OneOfConditionsStageBuilder : VisitChildrenStageBuilder
{
    public OneOfConditionsStageBuilder()
    {
        BeforeLeaving((_, _) => new RejectIfAllBehaviorsFailed());
    }
}
