using System.Linq.Expressions;

namespace TreeVal;

public class VisitorNodeFactory
{
    public IEvaluatorBuilder OfType(ExpressionType nodeType)
        => new EvaluatorBuilder(nodeType);

    public IEvaluatorBuilder<TExpression> OfType<TExpression>(ExpressionType? nodeType = null)
        where TExpression : Expression
        => new EvaluatorBuilder<TExpression>(nodeType);

    public IEvaluatorBuilder OneOf(params IEvaluatorBuilder[] children)
        => new OneOfEvaluatorBuilder(children);
}