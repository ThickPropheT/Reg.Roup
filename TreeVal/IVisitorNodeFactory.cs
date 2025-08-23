using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace TreeVal;

// TODO try to come up with a better name
public interface IVisitorNodeFactory
{
    IEvaluatorBuilder Where(
        Func<Expression, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "");

    IEvaluatorBuilder OfType(ExpressionType nodeType);

    IEvaluatorBuilder<TExpression> OfType<TExpression>(ExpressionType? nodeType = null)
        where TExpression : Expression;

    IEvaluatorConditionBuilder OneOf(
        IEvaluatorNodeFactory option1, IEvaluatorNodeFactory option2, params IEvaluatorNodeFactory[] options);
}
