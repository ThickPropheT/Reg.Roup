using System.Runtime.CompilerServices;
using TreeVal.Condition;
using TreeVal.Media;

namespace TreeVal;

// TODO try to come up with a better name
public interface IVisitorNodeFactory
{
    IEvaluatorBuilder Where(
        Func<Node, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "");

    IEvaluatorBuilder<T> OfType<T>();

    IEvaluatorConditionBuilder OneOf(
        INodeEvaluatorFactory option1, INodeEvaluatorFactory option2, params INodeEvaluatorFactory[] options);
}
