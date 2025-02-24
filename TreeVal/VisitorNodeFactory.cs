using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using TreeVal.Extensions;

namespace TreeVal;

public class VisitorNodeFactory
{
    public IEvaluatorBuilder Where(
        Func<Expression, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "")
        => new EvaluatorBuilder().Where(predicate, predicateExpression);

    public IEvaluatorBuilder OfType(ExpressionType nodeType)
        => new TypalEvaluatorBuilder(nodeType);

    public IEvaluatorBuilder<TExpression> OfType<TExpression>(ExpressionType? nodeType = null)
        where TExpression : Expression
        => new TypalEvaluatorBuilder<TExpression>(nodeType);

    public IEvaluatorConditionBuilder OneOf(params IEvaluatorNodeFactory[] options)
        => new ProxyEvaluatorBuilder((conditions, _) => new OneOfEvaluatorNode(conditions, options));
}
