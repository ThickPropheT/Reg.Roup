using System.Runtime.CompilerServices;
using TreeVal.Eval;
using TreeVal.Extensions;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public class DefaultEvaluatorBuilderFactory : IEvaluatorBuilderFactory
{
    public IEvaluatorBuilder Where(
        Func<Node, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "")
        => new EvaluatorBuilder().Where(predicate, predicateExpression);

    public IEvaluatorBuilder<T> OfType<T>()
        => new TypalEvaluatorBuilder<T>();

    public IEvaluatorBuilder OneOf(
        INodeEvaluatorFactory option1, INodeEvaluatorFactory option2, params INodeEvaluatorFactory[] options)
        => new ProxyEvaluatorBuilder((conditions, _) =>
            new OneOfNodeEvaluator(conditions, new[] { option1, option2 }.Concat(options).ToArray()));
}
