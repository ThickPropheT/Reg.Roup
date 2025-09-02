using System.Runtime.CompilerServices;
using TreeVal.Extensions;
using TreeVal.Media;

namespace TreeVal;

public class DefaultVisitorNodeFactory : IVisitorNodeFactory
{
    public IEvaluatorBuilder Where(
        Func<Node, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "")
        => new EvaluatorBuilder().Where(predicate, predicateExpression);

    public IEvaluatorBuilder<T> OfType<T>()
        => new TypalEvaluatorBuilder<T>();

    public IEvaluatorConditionBuilder OneOf(
        INodeEvaluatorFactory option1, INodeEvaluatorFactory option2, params INodeEvaluatorFactory[] options)
        => new ProxyEvaluatorBuilder((conditions, _) =>
            new OneOfNodeEvaluator(conditions, new[] { option1, option2 }.Concat(options).ToArray()));
}
