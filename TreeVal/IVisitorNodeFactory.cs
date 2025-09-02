using System.Runtime.CompilerServices;
using TreeVal.Condition;

namespace TreeVal;

// TODO try to come up with a better name
public interface IVisitorNodeFactory
{
    IEvaluatorBuilder Where(
        Func<Node, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "");

    IEvaluatorBuilder<T> OfType<T>();

    IEvaluatorConditionBuilder OneOf(
        IEvaluatorNodeFactory option1, IEvaluatorNodeFactory option2, params IEvaluatorNodeFactory[] options);
}
