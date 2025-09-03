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
    void AddCondition(ICondition condition);
    void AddChildren(Func<Node, IEnumerable<INodeEvaluatorFactory>> getChildren);
}

public interface IEvaluatorBuilder<T> : IEvaluatorBuilder
{
}
