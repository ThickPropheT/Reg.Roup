using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class ProxyEvaluatorBuilder : EvaluatorBuilder
{
    private readonly Func<
            IEnumerable<ICondition<Expression>>,
            IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>>, EvaluatorNode>
        _toEvaluator;

    public ProxyEvaluatorBuilder(
        Func<
                IEnumerable<ICondition<Expression>>,
                IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>>, EvaluatorNode>
            toEvaluator)
    {
        _toEvaluator = toEvaluator;
    }

    protected override EvaluatorNode ToEvaluatorImpl(
        IEnumerable<ICondition<Expression>> conditions,
        IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> childLookups)
        => _toEvaluator(conditions.ToArray(), childLookups);
}

public class ProxyEvaluatorBuilder<TExpression> : ProxyEvaluatorBuilder, IEvaluatorBuilder<TExpression>
    where TExpression : Expression
{
    public ProxyEvaluatorBuilder(
        Func<IEnumerable<ICondition<Expression>>, IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>>, EvaluatorNode>
            toEvaluator) : base(toEvaluator)
    {
    }

    public void AddChildren(Func<TExpression, IEnumerable<IEvaluatorNodeFactory>> getChildren) 
        => base.AddChildren(e => getChildren((TExpression) e));
}
