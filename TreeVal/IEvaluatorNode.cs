using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public interface IEvaluatorNode
{
    IEnumerable<ICondition<Expression>> Conditions { get; }
    VisitationContext.MovementStrategy HeadMovementStrategy { get; }
    VisitationContext.EvaluationStrategy? ChildEvaluationStrategy { get; }

    IEnumerable<IEvaluatorNodeFactory> EnumerateChildren(Expression current);
}
