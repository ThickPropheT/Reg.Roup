using TreeVal.Condition;

namespace TreeVal;

public interface IEvaluatorNodeFactory
{
    IEvaluatorNode ToEvaluator();
}


public interface IEvaluatorConditionBuilder : IEvaluatorNodeFactory
{
    void AddCondition(ICondition condition);
}

public interface IEvaluatorConditionBuilder<T> : IEvaluatorConditionBuilder
{
}


public interface IEvaluatorBuilder : IEvaluatorConditionBuilder
{
    void AddChildren(Func<Node, IEnumerable<IEvaluatorNodeFactory>> getChildren);
}

public interface IEvaluatorBuilder<T> : IEvaluatorBuilder, IEvaluatorConditionBuilder<T>
{
    void AddChildren(Func<T, IEnumerable<IEvaluatorNodeFactory>> getChildren);
}
