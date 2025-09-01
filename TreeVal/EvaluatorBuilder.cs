using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class EvaluatorBuilder : IEvaluatorBuilder<Expression>
{
    private readonly List<ICondition<Expression>> _conditions = new(1);
    private readonly List<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> _childLookups = new(1);

    public void AddCondition(ICondition<Expression> condition)
        => _conditions.Add(condition);

    public void AddChildren(Func<Expression, IEnumerable<IEvaluatorNodeFactory>> getChildren)
        => _childLookups.Add(getChildren);

    public IEvaluatorNode ToEvaluator()
        => ToEvaluatorImpl(_conditions, _childLookups);

    protected virtual EvaluatorNode ToEvaluatorImpl(
        IEnumerable<ICondition<Expression>> conditions,
        IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> childLookups)
        => new(conditions.ToArray(), childLookups);
}
