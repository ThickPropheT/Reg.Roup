using TreeVal.Eval.Condition;

namespace TreeVal.Scaffolding;

public class TypalEvaluatorBuilder<T> : EvaluatorBuilder, IEvaluatorBuilder<T>
{
    public TypalEvaluatorBuilder()
    {
        AddCondition(NodeTypeCondition.AssertMatching<T>());
    }
}
