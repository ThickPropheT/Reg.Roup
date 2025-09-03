using System.Runtime.CompilerServices;
using TreeVal.Media;

namespace TreeVal.Scaffolding;

public interface IEvaluatorBuilderFactory
{
    IEvaluatorBuilder Where(
        Func<Node, bool> predicate, [CallerArgumentExpression(nameof(predicate))] string predicateExpression = "");
    
    IEvaluatorBuilder<T> OfType<T>();
    
    IEvaluatorBuilder OneOf(
        INodeEvaluatorFactory option1, INodeEvaluatorFactory option2, params INodeEvaluatorFactory[] options);
}
