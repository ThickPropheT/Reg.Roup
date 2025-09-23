using TreeVal.Eval;
using TreeVal.Eval.Condition;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public interface INodeEvaluatorFactory
{
    INodeEvaluator ToEvaluator();
}

public interface IEvaluatorBuilder : INodeEvaluatorFactory
{
    void AddConditions(Func<Node, IEnumerable<ICondition>> getConditions);
    void AddChildren(Func<Node, IEnumerable<INodeEvaluatorFactory>> getChildren);
}

public interface IEvaluatorBuilder<T> : IEvaluatorBuilder
{
}
