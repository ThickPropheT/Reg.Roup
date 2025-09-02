using TreeVal.Condition;
using TreeVal.Media;

namespace TreeVal;

public interface INodeEvaluatorFactory
{
    INodeEvaluator ToEvaluator();
}

public interface IEvaluatorConditionBuilder : INodeEvaluatorFactory
{
    void AddCondition(ICondition condition);
}

public interface IEvaluatorConditionBuilder<T> : IEvaluatorConditionBuilder
{
}

public interface IEvaluatorBuilder : IEvaluatorConditionBuilder
{
    void AddChildren(Func<Node, IEnumerable<INodeEvaluatorFactory>> getChildren);
}

public interface IEvaluatorBuilder<T> : IEvaluatorBuilder, IEvaluatorConditionBuilder<T>
{
}
