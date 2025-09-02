using TreeVal.Condition;

namespace TreeVal;

public class TypalEvaluatorBuilder<T> : EvaluatorBuilder, IEvaluatorBuilder<T>
{
    public TypalEvaluatorBuilder()
    {
        AddCondition(NodeTypeCondition.AssertMatching<T>());
    }

    public void AddChildren(Func<T, IEnumerable<IEvaluatorNodeFactory>> getChildren)
        => base.AddChildren(e => getChildren((T) e.Value));
}
