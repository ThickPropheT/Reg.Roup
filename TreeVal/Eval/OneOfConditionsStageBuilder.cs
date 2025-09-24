namespace TreeVal.Eval;

public class OneOfConditionsStageBuilder : VisitChildrenStageBuilder
{
    public OneOfConditionsStageBuilder()
    {
        BeforeLeaving(_ => new RejectIfAllBehaviorsFailed());
    }
}
