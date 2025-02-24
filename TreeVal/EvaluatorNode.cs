using System.Linq.Expressions;
using TreeVal.Condition;

namespace TreeVal;

public class EvaluatorNode : IEvaluatorNode
{
    private readonly ICondition[] _conditions;
    private readonly IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> _childLookups;

    public EvaluatorNode(
        ICondition[] conditions, IEnumerable<Func<Expression, IEnumerable<IEvaluatorNodeFactory>>> childLookups)
    {
        _conditions = conditions;
        _childLookups = childLookups;
    }

    public Expression? Evaluate(IVisitationContext context)
    {
        var current = context.MoveForward();

        try
        {
            var failedConditions = _conditions.Where(c => !c.Evaluate(current)).ToArray();

            if (failedConditions.Any())
            {
                // TODO pass in failedConditions
                context.Reject(this);

                // TODO figure out method of returning an Expression
                return null;
            }
        }
        catch (TreeRejectedException)
        {
            context.Reject(this);
            return null;
        }

        foreach (var child in _childLookups.SelectMany(lookup => lookup(current!)))
        {
            var visitor = child.ToEvaluator();
            visitor.Evaluate(context);

            if (context.HasRejection)
            {
                // TODO figure out method of returning an Expression
                context.Reject(this);
                return null;
            }
        }

        context.Accept(this);

        // TODO figure out method of returning an Expression
        return null;
    }
}
