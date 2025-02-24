using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class EvaluatorBuilder : IEvaluatorBuilder
{
    private readonly List<ICondition> _conditions = new(1);
    private readonly List<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> _childLookups = new(1);

    public EvaluatorBuilder()
    {
        AddCondition(new NotNullCondition());
    }

    public void AddCondition(ICondition condition)
        => _conditions.Add(condition);

    public void AddChildren(Func<Expression, IEnumerable<IEvaluatorNodeFactory>> getChildren)
        => _childLookups.Add(getChildren);

    public IEvaluatorNode ToEvaluator()
        => ToEvaluatorImpl(_conditions, _childLookups);

    protected virtual EvaluatorNode ToEvaluatorImpl(
        IEnumerable<ICondition> conditions,
        IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> childLookups)
        => new(conditions.ToArray(), childLookups);
}
