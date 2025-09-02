using System.Runtime.CompilerServices;
using TreeVal.Condition;
using TreeVal.Extensions;

namespace TreeVal;

public class DefaultVisitorNodeFactory : IVisitorNodeFactory
{
    public IEvaluatorBuilder Where(
        Func<Node, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "")
        => new EvaluatorBuilder().Where(predicate, predicateExpression);

    public IEvaluatorBuilder<T> OfType<T>()
        => new TypalEvaluatorBuilder<T>();

    public IEvaluatorConditionBuilder OneOf(
        IEvaluatorNodeFactory option1, IEvaluatorNodeFactory option2, params IEvaluatorNodeFactory[] options)
        => new ProxyEvaluatorBuilder((conditions, _) =>
            new OneOfEvaluatorNode(conditions, new[] { option1, option2 }.Concat(options).ToArray()));
}
