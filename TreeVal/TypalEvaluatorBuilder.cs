using TreeVal.Condition;

namespace TreeVal;

public class TypalEvaluatorBuilder<T> : EvaluatorBuilder, IEvaluatorBuilder<T>
{
    public TypalEvaluatorBuilder()
    {
        AddCondition(NodeTypeCondition.AssertMatching<T>());
    }
}
